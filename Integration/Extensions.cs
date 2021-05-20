using CustomCommandSystem.Common.Delegates;
using CustomCommandSystem.Services;
using CustomCommandSystem.Services.Loader;
using CustomCommandSystem.Services.Parser;
using GTANetworkMethods;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace CustomCommandSystem.Integration
{
    public static class Extensions
    {
        static Extensions()
        {
            new Main();
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
        ///     Creates instances of all classes with atleast one non-static command!<br/>
        ///     So choose atleast one of these rules:<br/>
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Only use static methods for commands</description>
        ///         </item>
        ///         <item>
        ///             <description>Don't create any object of a class which has atleast one non-static command</description>
        ///         </item>
        ///         <item>
        ///             <description>Don't have any code in the parameterless constructor which should only be run once</description>
        ///         </item>
        ///         <item>
        ///             <description>Put code which should only be run once in a static or constructor with parameters instead</description>
        ///         </item>
        ///     </list></para>
        /// </remarks>
        public static void RegisterCustom(this Command napiCommand)
            => RegisterCustom(napiCommand, Assembly.GetCallingAssembly());

        /// <summary>
        ///     Registers all commands (doesn't matter if private, static, not static etc.) in the specified assembly.
        /// </summary>
        /// <inheritdoc cref="RegisterCustom(Command)" path="/remarks"/>
        /// <param name="assembly"/>
        public static void RegisterCustom(this Command _, Assembly assembly)
            => CommandsLoader.Instance?.LoadCommands(assembly);

        /// <summary>
        ///     Unregisters all commands in the current assembly.
        /// </summary>
        public static void UnregisterCustom(this Command napiCommand)
            => UnregisterCustom(napiCommand, Assembly.GetCallingAssembly());

        /// <summary>
        ///     Unregisters all commands in the specified assembly.
        /// </summary>
        public static void UnregisterCustom(this Command _, Assembly assembly)
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
        public static void SetConverter<T>(this Command _, int amountArgumentsNeeded, ConverterDelegate converter)
          => ArgumentsConverter.Instance.SetConverter(typeof(T), amountArgumentsNeeded, converter);

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
        public static void SetAsyncConverter<T>(this Command _, int amountArgumentsNeeded, AsyncConverterDelegate asyncConverter)
          => ArgumentsConverter.Instance.SetAsyncConverter(typeof(T), amountArgumentsNeeded, asyncConverter);

        /// <summary>
        ///     Executes a command.
        /// </summary>
        /// <param name="player">Player to be passed to the command methods and to be error messages to.</param>
        /// <param name="command">The command with prefix or without prefix.</param>
        public static void Execute(this Command _, GTANetworkAPI.Player player, string command)
            => CommandsHandler.Instance?.ExecuteCommand(player, command);
    }
}
