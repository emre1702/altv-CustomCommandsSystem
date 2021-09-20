using CustomCommandsSystem.Common.Attributes;
using CustomCommandsSystem.Common.Interfaces.Services;
using CustomCommandsSystem.Common.Models;
using CustomCommandsSystem.Services.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CustomCommandsSystem.Services.Loader
{
    internal class CommandsLoader : ICommandsLoader
    {
        public static CommandsLoader? Instance { get; private set; }

        private readonly Dictionary<string, CommandData> _commandDatas = new(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<Type, object> _instancesPerClass = new();
        private readonly HashSet<Assembly> _assembliesRegistered = new();

        private readonly FastMethodInvoker _fastMethodInvoker;
        private readonly ILogger _logger;
        private readonly ICommandArgumentsConverter _argumentsConverter;
        private readonly ICommandsConfiguration _configuration;

        public CommandsLoader(FastMethodInvoker fastMethodInvoker, ILogger logger, ICommandArgumentsConverter argumentsConverter, ICommandsConfiguration configuration)
        {
            _fastMethodInvoker = fastMethodInvoker;
            _logger = logger;
            _argumentsConverter = argumentsConverter;
            _configuration = configuration;

            Instance = this;

            _argumentsConverter.ConverterChanged += ReloadUserParameters;
        }

        public void LoadCommands(Assembly assembly)
        {
            lock (_assembliesRegistered)
            {
                if (_assembliesRegistered.Contains(assembly)) return;
                _assembliesRegistered.Add(assembly);

                lock (_commandDatas)
                {
                    var methods = GetCommandMethods(assembly);

                    foreach (var method in methods)
                        AddMethod(method);
                    SortMethodDatasByPriority();
                }
            } 
        }

        private IEnumerable<MethodInfo> GetCommandMethods(Assembly assembly)
            => assembly.GetTypes().SelectMany(type => type
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttribute<CustomCommandAttribute>(false) != null));

        private void AddMethod(MethodInfo method)
        {
            var cmdAttribute = method.GetCustomAttribute<CustomCommandAttribute>()!;
            var fastInvokeHandler = _fastMethodInvoker.GetMethodInvoker(method);
            var methodData = new CommandMethodData(method, fastInvokeHandler, cmdAttribute.Priority);
            methodData.UserParameters = GetUserParameterInformation(method, methodData).ToList();
            if (!AddInstanceIfRequired(method, methodData)) return;

            methodData.CompleteInitialization();
            AddCmdAndAliasesToCommandDatas(methodData, cmdAttribute.Command);
        }

        private void AddCmdAndAliasesToCommandDatas(CommandMethodData methodData, string cmd)
        {
            AddToCommandData(methodData, cmd);
            foreach (var alias in methodData.Aliases)
                AddToCommandData(methodData, alias);
        }

        private void AddToCommandData(CommandMethodData methodData, string cmdOrAlias)
        {
            if (!_commandDatas.TryGetValue(cmdOrAlias, out var data))
            {
                data = new CommandData(cmdOrAlias);
                _commandDatas[cmdOrAlias] = data;
            }
            data.Methods.Add(methodData);
        }

        private IEnumerable<CommandParameterData> GetUserParameterInformation(MethodInfo method, CommandMethodData commandMethodData)
        {
            var userInputParameters = GetUserInputParameters(method, commandMethodData).ToList();
            for (var i = 0; i < userInputParameters.Count; ++i)
            {
                if (commandMethodData.RemainingParamsToStringIndex.HasValue) yield break;

                var parameter = userInputParameters[i];
                var parameterData = new CommandParameterData
                {
                    UserInputLength = GetUserInputAmountForArg(parameter),
                    HasDefaultValue = parameter.HasDefaultValue,
                    DefaultValue = parameter.DefaultValue,
                    IsRemainingText = IsRemainingText(parameter),
                    IsNullable = IsNullable(parameter),
                    Name = parameter.Name,
                    Type = parameter.ParameterType
                };
                yield return parameterData;
            }
        }

        private bool AddInstanceIfRequired(MethodInfo method, CommandMethodData methodData)
        {
            if (method.IsStatic) return true;

            var instance = GetMethodInstance(method, _configuration.ServiceProviderForInstances);
            if (instance is null)
            {
                _logger.LogWarning($"Method {method.Name} in class {method.DeclaringType!.FullName} can not be added because " +
                    $"it's neither static nor an object of the class can be created (e.g. because of missing parameterless constructor or being a static/abstract class)." +
                    $"If you use Dependency Injection, " + 
                    (_configuration.ServiceProviderForInstances is null 
                        ? "you'll have to set \"ServiceProviderForInstances\" in the commands configuration"
                        : "make sure the class is registered in the service collection and can be created by the service provider."));
                return false;
            }
            methodData.Instance = instance;
            return true;
        }

        internal object? GetMethodInstance(MethodInfo method, IServiceProvider? serviceProvider)
        {
            var classType = method.DeclaringType!;
            if (_instancesPerClass.TryGetValue(classType, out var instance))
                return instance;

            if (serviceProvider is not null)
                instance = GetMethodInstanceByServiceProvider(classType, serviceProvider);
            else
                instance = Activator.CreateInstance(classType);

            if (instance is null) return null;
            _instancesPerClass[classType] = instance;
            return instance;
        }

        private object? GetMethodInstanceByServiceProvider(Type classType, IServiceProvider serviceProvider)
        {
            var instance = serviceProvider.GetService(classType);
            if (instance is not null) return instance;

            if (classType.GetConstructor(Type.EmptyTypes) is not null) return Activator.CreateInstance(classType);

            return null;
        }

        private IEnumerable<ParameterInfo> GetUserInputParameters(MethodInfo method, CommandMethodData commandMethodData)
        {
            var methodParameters = method.GetParameters();
            var userParametersStartIndex = commandMethodData.UserParametersStartIndex;
            return methodParameters.Skip(userParametersStartIndex);
        }

        private int GetUserInputAmountForArg(ParameterInfo parameter)
        {
            var paramType = parameter.ParameterType;

            var argumentsLength = _argumentsConverter.GetTypeArgumentsLength(paramType);
            if (argumentsLength.HasValue) return argumentsLength.Value;

            return 1;
        }

        private bool IsRemainingText(ParameterInfo parameter)
            => parameter.GetCustomAttribute<CustomCommandRemainingTextAttribute>() != null;

        private bool IsNullable(ParameterInfo parameter)
            => Nullable.GetUnderlyingType(parameter.ParameterType) != null;

        private void SortMethodDatasByPriority()
        {
            foreach (var entry in _commandDatas)
                entry.Value.Methods.Sort((a, b) => -1 * a.Priority.CompareTo(b.Priority));
        }

        public void UnloadCommands(Assembly assembly)
        {
            lock (_assembliesRegistered)
            {
                if (!_assembliesRegistered.Contains(assembly)) return;
                _assembliesRegistered.Remove(assembly);

                lock (_commandDatas)
                {
                    RemoveAllMethodsInAssembly(assembly);
                    RemoveEmptyMethodDataEntries();
                }
            }
        }

        private void RemoveAllMethodsInAssembly(Assembly assembly)
        {
            foreach (var entry in _commandDatas)
                entry.Value.Methods.RemoveAll(v => v.Method.DeclaringType!.Assembly == assembly);
        }

        private void RemoveEmptyMethodDataEntries()
        {
            foreach (var entry in _commandDatas.Where(d => d.Value.Methods.Count == 0).ToList())
                _commandDatas.Remove(entry.Key);
        }

        public void ReloadCommands(Assembly assembly)
        {
            UnloadCommands(assembly);
            LoadCommands(assembly);
        }

        CommandData? ICommandsLoader.GetCommandData(string cmd)
        {
            lock (_commandDatas) 
            {
                return _commandDatas.TryGetValue(cmd, out var data) ? data : null;
            }
        }

        internal Dictionary<string, CommandData> GetCommandDatas() => _commandDatas;

        private void ReloadUserParameters()
        {
            lock (_commandDatas)
            {
                foreach (var commandData in _commandDatas.Values)
                    foreach (var method in commandData.Methods)
                    {
                        method.UserParameters = GetUserParameterInformation(method.Method, method).ToList();
                        method.CompleteInitialization();
                    }
            }
        }

    }
}
