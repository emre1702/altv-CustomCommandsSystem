using CustomCommandSystem.Common.Datas;
using CustomCommandSystem.Common.Delegates;
using CustomCommandSystem.Common.Interfaces.Services;
using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomCommandSystem.Services.Parser
{
    internal class ArgumentsConverter : ICommandArgumentsConverter
    {
        public event EmptyDelegate? ConverterChanged;

#nullable disable
        internal static ArgumentsConverter Instance { get; private set; }
#nullable restore

        private readonly Dictionary<Type, (int ArgsLength, AsyncConverterDelegate Converter)> _asyncConverters = new Dictionary<Type, (int ArgsLength, AsyncConverterDelegate Converter)>();
        private readonly Dictionary<Type, (int ArgsLength, ConverterDelegate Converter)> _converters = DefaultConverters.Data;
        

        public ArgumentsConverter()
        {
            Instance = this;
        }

        public void SetAsyncConverter(Type forType, int argumentsLength, AsyncConverterDelegate asyncConverter)
        {
            lock (_converters)
            {
                _asyncConverters[forType] = (argumentsLength, asyncConverter);
            }
            ConverterChanged?.Invoke();
        }

        public void SetConverter(Type forType, int argumentsLength, ConverterDelegate converter)
        {
            lock (_converters)
            {
                _converters[forType] = (argumentsLength, converter);
            }
            ConverterChanged?.Invoke();
        }

        public async ValueTask<(object? ConvertedValue, int AmountArgsUsed)> Convert(Player player, string[] userArgs, int atIndex, Type toType, CancelEventArgs errorMessageCancel)
        {
            var asyncResult = await TryConvertAsync(player, userArgs, atIndex, toType, errorMessageCancel);
            if (asyncResult.AmountArgsUsed > 0) return asyncResult;

            (int ArgsLength, ConverterDelegate Converter) converterData;
            lock (_converters)
            {
                if (!_converters.TryGetValue(toType, out converterData))
                    return (System.Convert.ChangeType(userArgs[atIndex], toType), 1);
            }

            var argsToUse = new ArraySegment<string>(userArgs, atIndex, converterData.ArgsLength);
            var ret = converterData.Converter(player, argsToUse, errorMessageCancel);

            if (ret is Task<object> task)
                return (await task, converterData.ArgsLength);
            return (ret, converterData.ArgsLength);
        }

        private async ValueTask<(object? ConvertedValue, int AmountArgsUsed)> TryConvertAsync(Player player, string[] userArgs, int atIndex, Type toType, CancelEventArgs errorMessageCancel)
        {
            (int ArgsLength, AsyncConverterDelegate Converter) converterData;
            lock (_asyncConverters)
            {
                if (!_asyncConverters.TryGetValue(toType, out converterData))
                    return (null, 0);
            }
            var argsToUse = new ArraySegment<string>(userArgs, atIndex, converterData.ArgsLength);
            var ret = converterData.Converter(player, argsToUse, errorMessageCancel);

            return (await ret, converterData.ArgsLength);
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
