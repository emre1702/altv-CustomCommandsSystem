using CustomCommandSystem.Common.Delegates;
using System;
using System.Threading.Tasks;

namespace CustomCommandSystem.Common.Interfaces.Services
{
    internal interface ICommandArgumentsConverter
    {
        event EmptyDelegate? ConverterChanged;

        ValueTask<(object ConvertedValue, int AmountArgsUsed)> Convert(string[] userArgs, int atIndex, Type toType);
        void SetAsyncConverter(Type forType, int argumentsLength, Func<ArraySegment<string>, Task<object>> asyncConverter);
        void SetConverter(Type forType, int argumentsLength, Func<ArraySegment<string>, object> converter);
        internal int? GetTypeArgumentsLength(Type type); 
    }
}