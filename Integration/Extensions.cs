using AltV.Net;
using AltV.Net.Elements.Entities;
using CustomCommandsSystem.Common.Delegates;
using CustomCommandsSystem.Services;
using CustomCommandsSystem.Services.Loader;
using CustomCommandsSystem.Services.Parser;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace CustomCommandsSystem.Integration
{
    public static class Extensions
    {
        static Extensions()
        {
            _ = new Main();
        }

        /// <summary>
        ///     Registers all commands (doesn't matter if private, static, not static etc.) in the current assembly.
        /// </summary>
        /// <remarks>
        ///     <para>1. WARNING: <br/>
        ///     If your class has atleast one non-static command method, it also needs a parameterless constructor.<br/>
        ///     So if you have a constructor with parameters in this class, also add one parameterless constructor.
        /// </para>
        ///     <para>2. WARNING:<br/>
        ///     Creates instances of all classes with atleast one non-static command if ServiceProvider is not set in the config!<br/>
        ///     So choose atleast one of these rules:<br/>
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Only use static methods for commands</description>
        ///         </item>
        ///         <item>
        ///             <description>Use Dependency Injection to pass custom parameters or create the instance by yourself./description>
        ///         </item>
        ///         <item>
        ///             <description>Don't create any instance of a class which has atleast one non-static command</description>
        ///         </item>
        ///         <item>
        ///             <description>Don't have any code in the parameterless constructor which should only be run once</description>
        ///         </item>
        ///         <item>
        ///             <description>Put code which should only be run once in a static or constructor with parameters instead</description>
        ///         </item>
        ///     </list></para>
        /// </remarks>
        public static void RegisterCustomCommands(this ICore core)
            => RegisterCustomCommands(core, Assembly.GetCallingAssembly());

        /// <summary>
        ///     Registers all commands (doesn't matter if private, static, not static etc.) in the specified assembly.
        /// </summary>
        /// <inheritdoc cref="RegisterCustom(IServer)" path="/remarks"/>
        /// <param name="assembly"/>
        public static void RegisterCustomCommands(this ICore _, Assembly assembly)
            => CommandsLoader.Instance?.LoadCommands(assembly);

        /// <summary>
        ///     Unregisters all commands in the current assembly.
        /// </summary>
        public static void UnregisterCustomCommands(this ICore core)
            => UnregisterCustomCommands(core, Assembly.GetCallingAssembly());

        /// <summary>
        ///     Unregisters all commands in the specified assembly.
        /// </summary>
        public static void UnregisterCustomCommands(this ICore _, Assembly assembly)
            => CommandsLoader.Instance?.UnloadCommands(assembly);

        /// <summary>
        ///     Adds a converter for a type to be able to use that type in a command.
        /// </summary>
        /// <typeparam name="T">Type of the object you want to use in your commands</typeparam>
        /// <param name="amountArgumentsNeeded">Amount of arguments needed for that commands (e.g. Vector3 needs <u>3</u> because of X, Y and Z)</param>
        /// <param name="converter">
        ///     The function to convert the arguments to the type you want to have.<br/> 
        ///     The <see cref="ArraySegment{T}"/> of <see cref="string"/> exactly contains the arguments (length = <paramref name="amountArgumentsNeeded"/>) used for this type.<br/>
        ///     Return of the converter needs to be of type <typeparamref name="T"/>
        ///     Use the CancelEventArgs if you want to output the error message by yourself (like "Player not found").
        /// </param>
        /// <param name="allowNull">
        ///     Is it allowed for the converter to return null?<br/>
        ///     Set null to make it depending on the nullability of the type.<br/>
        ///     E.g. int is not nullable so a <see langword="null"/> return would tell the code to ignore that method.<br/>
        ///     To make reference types nullable, enable nullable in your project.<br/>
        ///     Default: false
        /// </param>
        public static void SetCustomCommandsConverter<T>(this ICore _, int amountArgumentsNeeded, ConverterDelegate converter, bool? allowNull = false)
          => ArgumentsConverter.Instance.SetConverter(typeof(T), amountArgumentsNeeded, converter, allowNull);

        /// <summary>
        ///     Adds an async converter for a type to be able to use that type in a command.
        /// </summary>
        /// <typeparam name="T">Type of the object you want to use in your commands</typeparam>
        /// <param name="amountArgumentsNeeded">Amount of arguments needed for that commands (e.g. Vector3 needs <u>3</u> because of X, Y and Z)</param>
        /// <param name="asyncConverter">
        ///     The function to convert the arguments to the type you want to have.<br/> 
        ///     The <see cref="ArraySegment{T}"/> of <see cref="string"/> exactly contains the arguments (length = <paramref name="amountArgumentsNeeded"/>) used for this type.<br/>
        ///     Return of the converter needs to be of type <see cref="Task{T}" /> of <typeparamref name="T"/>.
        ///     Use the CancelEventArgs if you want to output the error message by yourself (like "Player not found").
        /// </param>
        /// <param name="allowNull">
        ///     Is it allowed for the converter to return null?<br/>
        ///     Set null to make it depending on the nullability of the type.<br/>
        ///     E.g. int is not nullable so a <see langword="null"/> return would tell the code to ignore that method.<br/>
        ///     To make reference types nullable, enable nullable in your project.<br/>
        ///     Default: false
        /// </param>
        public static void SetCustomCommandsAsyncConverter<T>(this ICore _, int amountArgumentsNeeded, AsyncConverterDelegate asyncConverter, bool? allowNull = false)
          => ArgumentsConverter.Instance.SetAsyncConverter(typeof(T), amountArgumentsNeeded, asyncConverter, allowNull);

        /// <summary>
        ///     Executes a command.
        /// </summary>
        /// <param name="player">Player to be passed to the command methods and to be error messages to.</param>
        /// <param name="command">The command with prefix or without prefix.</param>
        public static void ExecuteCustomCommand(this ICore _, IPlayer player, string command)
            => CommandsHandler.Instance?.ExecuteCommand(player, command);
    }
}
