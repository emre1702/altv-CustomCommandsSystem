using CustomCommandSystem.Common.Datas;
using CustomCommandSystem.Common.Delegates;
using CustomCommandSystem.Common.Interfaces.Services;
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

        private readonly Dictionary<Type, (int ArgsLength, Func<ArraySegment<string>, object> Converter)> _converters = DefaultConverters.Data;
        

        public ArgumentsConverter()
        {
            Instance = this;
        }

        public void SetAsyncConverter(Type forType, int argumentsLength, Func<ArraySegment<string>, Task<object>> asyncConverter)
            => SetConverter(forType, argumentsLength, (Func<ArraySegment<string>, object>) asyncConverter);

        public void SetConverter(Type forType, int argumentsLength, Func<ArraySegment<string>, object> converter)
        {
            lock (_converters)
            {
                _converters[forType] = (argumentsLength, converter);
            }
            ConverterChanged?.Invoke();
        }

        public async ValueTask<(object ConvertedValue, int AmountArgsUsed)> Convert(string[] userArgs, int atIndex, Type toType)
        {
            (int ArgsLength, Func<ArraySegment<string>, object> Converter) converterData;
            lock (_converters)
            {
                if (!_converters.TryGetValue(toType, out converterData))
                    return (System.Convert.ChangeType(userArgs[atIndex], toType), 1);
            }

            var argsToUse = new ArraySegment<string>(userArgs, atIndex, converterData.ArgsLength);
            var ret = converterData.Converter(argsToUse);

            if (ret is Task<object> task)
                return (await task, converterData.ArgsLength);
            return (ret, converterData.ArgsLength);
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
