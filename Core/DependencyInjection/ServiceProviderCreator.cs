using CustomCommandsSystem.Common.Interfaces.Services;
using CustomCommandsSystem.Services;
using CustomCommandsSystem.Services.Cleaner;
using CustomCommandsSystem.Services.Executer;
using CustomCommandsSystem.Services.Loader;
using CustomCommandsSystem.Services.Parser;
using CustomCommandsSystem.Services.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CustomCommandsSystem.Core.DependencyInjection
{
    internal static class ServiceProviderCreator
    {
        internal static IServiceProvider Create()
        {
            var seviceCollection = new ServiceCollection()

                .WithCleaner()
                .WithExecuter()
                .WithLoader()
                .WithParser()
                .WithUtils()

                .AddSingleton<ICommandsHandler, CommandsHandler>();

            return seviceCollection.BuildServiceProvider();
        }

        private static IServiceCollection WithCleaner(this IServiceCollection serviceCollection)
            => serviceCollection.AddSingleton<ICommandMessageCleaner, MessageCleaner>();

        private static IServiceCollection WithExecuter(this IServiceCollection serviceCollection)
            => serviceCollection.AddSingleton<ICommandMethodExecuter, MethodExecuter>();

        private static IServiceCollection WithLoader(this IServiceCollection serviceCollection)
            => serviceCollection.AddSingleton<ICommandsLoader, CommandsLoader>();

        private static IServiceCollection WithParser(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<ICommandArgumentsConverter, ArgumentsConverter>()
                .AddSingleton<ICommandArgumentsParser, ArgumentsParser>()
                .AddSingleton<ICommandParser, CommandParser>()
                .AddSingleton<ICommandMethodsParser, MethodsParser>();

        private static IServiceCollection WithUtils(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<ICommandsConfiguration>(CommandsConfiguration.LazyInstance.Value)
                .AddSingleton<ILogger, ConsoleLogger>()
                .AddSingleton<FastMethodInvoker>()
                .AddSingleton<IWrongUsageHandler, WrongUsageHandler>();

    }
}
