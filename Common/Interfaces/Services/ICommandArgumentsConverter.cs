using CustomCommandSystem.Common.Delegates;
using GTANetworkAPI;
using System;
using System.Threading.Tasks;

namespace CustomCommandSystem.Common.Interfaces.Services
{
    internal interface ICommandArgumentsConverter
    {
        event EmptyDelegate? ConverterChanged;

        ValueTask<(object? ConvertedValue, int AmountArgsUsed)> Convert(Player player, string[] userArgs, int atIndex, Type toType, CancelEventArgs errorMessageCancel);
        void SetAsyncConverter(Type forType, int argumentsLength, AsyncConverterDelegate asyncConverter);
        void SetConverter(Type forType, int argumentsLength, ConverterDelegate converter);
        internal int? GetTypeArgumentsLength(Type type); 
    }
}