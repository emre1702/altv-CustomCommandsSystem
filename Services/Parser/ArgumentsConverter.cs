using CustomCommandsSystem.Common.Datas;
using CustomCommandsSystem.Common.Delegates;
using CustomCommandsSystem.Common.Interfaces.Services;
using CustomCommandsSystem.Common.Models;
using CustomCommandsSystem.Services.Utils;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CustomCommandsSystem.Services.Parser
{
    internal class ArgumentsConverter : ICommandArgumentsConverter
    {
        public event EmptyDelegate? ConverterChanged;

#nullable disable
        internal static ArgumentsConverter Instance { get; private set; }
#nullable restore

        private readonly ILogger _logger;
        private readonly Dictionary<Type, (int ArgsLength, bool? AllowNull, AsyncConverterDelegate Converter)> _asyncConverters;
        private readonly Dictionary<Type, (int ArgsLength, bool? AllowNull, ConverterDelegate Converter)> _converters;
        

        public ArgumentsConverter(ICommandsConfiguration config, ILogger logger)
        {
            _logger = logger;
            _converters = DefaultConverters.Data(config);
            _asyncConverters = new();
            Instance = this;
        }

        public void SetAsyncConverter(Type forType, int argumentsLength, AsyncConverterDelegate asyncConverter, bool? allowNull = false)
        {
            lock (_converters)
            {
                _asyncConverters[forType] = (argumentsLength, allowNull, asyncConverter);
            }
            ConverterChanged?.Invoke();
        }

        public void SetConverter(Type forType, int argumentsLength, ConverterDelegate converter, bool? allowNull = false)
        {
            lock (_converters)
            {
                _converters[forType] = (argumentsLength, allowNull, converter);
            }
            ConverterChanged?.Invoke();
        }

        public async ValueTask<(object? ConvertedValue, int AmountArgsUsed, bool? AllowNull)> Convert(IPlayer player, UserInputData userInputData, int atIndex, Type toType, CancelEventArgs errorMessageCancel)
        {
            var asyncResult = await TryConvertAsync(player, userInputData, atIndex, toType, errorMessageCancel);
            if (asyncResult.AmountArgsUsed > 0) return asyncResult;

            (int ArgsLength, bool? AllowNull, ConverterDelegate Converter) converterData;
            lock (_converters)
            {
                if (!_converters.TryGetValue(toType, out converterData))
                {
                    _logger.LogWarning($"Type {toType.FullName} has no converter but is used as a parameter in command '{userInputData.Command}'. See https://github.com/emre1702/altv-CustomCommandsSystem/wiki/Custom-Converters for more information.");
                    return (System.Convert.ChangeType(userInputData.Arguments[atIndex], toType), 1, null);
                }
                    
            }

            var argsToUse = new ArraySegment<string>(userInputData.Arguments, atIndex, converterData.ArgsLength);
            var ret = converterData.Converter(player, userInputData, argsToUse, errorMessageCancel);

            if (ret is Task<object> task)
                return (await task, converterData.ArgsLength, converterData.AllowNull);
            return (ret, converterData.ArgsLength, converterData.AllowNull);
        }

        private async ValueTask<(object? ConvertedValue, int AmountArgsUsed, bool? AllowNull)> TryConvertAsync(IPlayer player, UserInputData userInputData, int atIndex, Type toType, CancelEventArgs errorMessageCancel)
        {
            (int ArgsLength, bool? AllowNull, AsyncConverterDelegate Converter) converterData;
            lock (_asyncConverters)
            {
                if (!_asyncConverters.TryGetValue(toType, out converterData))
                    return (null, 0, null);
            }
            var argsToUse = new ArraySegment<string>(userInputData.Arguments, atIndex, converterData.ArgsLength);
            var ret = converterData.Converter(player, userInputData, argsToUse, errorMessageCancel);

            return (await ret, converterData.ArgsLength, converterData.AllowNull);
        }

        int? ICommandArgumentsConverter.GetTypeArgumentsLength(Type type)
        {
            lock (_converters)
            {
                return _converters.TryGetValue(type, out var data) ? data.ArgsLength : (int?)null;
            }
        }
    }
}
