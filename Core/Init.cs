using CustomCommandSystem.Common.Interfaces.Services;
using CustomCommandSystem.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CustomCommandSystem.Core
{
    internal class Init
    {
        public Init()
        {
            var serviceProvider = ServiceProviderCreator.Create();
            InitializeServices(serviceProvider);
        }

        private void InitializeServices(IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<ICommandsHandler>();
        }
    }
}
