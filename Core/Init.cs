using CustomCommandsSystem.Common.Interfaces.Services;
using CustomCommandsSystem.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CustomCommandsSystem.Core
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
