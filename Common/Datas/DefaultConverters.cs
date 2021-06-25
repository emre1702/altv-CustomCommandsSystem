using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using CustomCommandsSystem.Common.Delegates;
using CustomCommandsSystem.Common.Models;
using CustomCommandsSystem.Services.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CustomCommandsSystem.Common.Datas
{
    public static class DefaultConverters
    {
        public static Dictionary<Type, (int ArgsLength, bool? AllowNull, ConverterDelegate Converter)> Data(ICommandsConfiguration config) => new()
        {
            [typeof(bool)] = (1, false, (player, userInputData, args, cancelErrorMsgOnFail) => ParseBool(args[0])),
            [typeof(byte)] = (1, false, (player, userInputData, args, cancelErrorMsgOnFail) => byte.Parse(args[0])),
            [typeof(sbyte)] = (1, false, (player, userInputData, args, cancelErrorMsgOnFail) => sbyte.Parse(args[0])),
            [typeof(char)] = (1, false, (player, userInputData, args, cancelErrorMsgOnFail) => char.Parse(args[0])),
            [typeof(decimal)] = (1, false, (player, userInputData, args, cancelErrorMsgOnFail) 
                => decimal.Parse(args[0].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture)),
            [typeof(double)] = (1, false, (player, userInputData, args, cancelErrorMsgOnFail) => ParseDouble(args[0])),
            [typeof(float)] = (1, false, (player, userInputData, args, cancelErrorMsgOnFail) => ParseFloat(args[0].Replace(',', '.'))),
            [typeof(int)] = (1, false, (player, userInputData, args, cancelErrorMsgOnFail) => int.Parse(args[0])),
            [typeof(uint)] = (1, false, (player, userInputData, args, cancelErrorMsgOnFail) => uint.Parse(args[0])),
            [typeof(long)] = (1, false, (player, userInputData, args, cancelErrorMsgOnFail) => long.Parse(args[0])),
            [typeof(ulong)] = (1, false, (player, userInputData, args, cancelErrorMsgOnFail) => ulong.Parse(args[0])),
            [typeof(short)] = (1, false, (player, userInputData, args, cancelErrorMsgOnFail) => short.Parse(args[0])),
            [typeof(ushort)] = (1, false, (player, userInputData, args, cancelErrorMsgOnFail) => ushort.Parse(args[0])),
            [typeof(string)] = (1, false, (player, userInputData, args, cancelErrorMsgOnFail) => args[0]),

            [typeof(Position)] = (3, false, (player, userInputData, args, cancelErrorMsgOnFail) => new Position(ParseFloat(args[0]), ParseFloat(args[1]), ParseFloat(args[2]))),
            [typeof(Rgba)] = (4, false, (player, userInputData, args, cancelErrorMsgOnFail) => new Rgba(byte.Parse(args[0]), byte.Parse(args[1]), byte.Parse(args[2]), byte.Parse(args[3]))),
            [typeof(DegreeRotation)] = (3, false, (player, userInputData, args, cancelErrorMsgOnFail) => new DegreeRotation(ParseFloat(args[0]), ParseFloat(args[1]), ParseFloat(args[2]))),
            [typeof(Rotation)] = (3, false, (player, userInputData, args, cancelErrorMsgOnFail) => new Rotation(ParseFloat(args[0]), ParseFloat(args[1]), ParseFloat(args[2]))),

            [typeof(IPlayer)] = (1, false, (player, userInputData, args, cancelErrorMsgOnFail) =>
            {
                var target = AltV.Net.Alt.GetAllPlayers().FirstOrDefault(p => p.Name == args[0]);
                if (target is null && config.PlayerNotFoundErrorMessage is { } text)
                {
                    cancelErrorMsgOnFail.Cancel = true;
                    config.MessageOutputHandler.Invoke(new CommandOutputData(player, text, userInputData));
                }
                return target;
            })
        };
                

        private static bool ParseBool(string val) 
            => val.ToLower() switch
            {
                "yes" => true,
                "ja" => true,
                "si" => true,
                "sí" => true,
                "sì" => true,
                "oui" => true,
                "да" => true,
                "no" => false,
                "nein" => false,
                "non" => false,
                "нет" => false,

                _ => bool.Parse(val)
            };

        private static double ParseDouble(string val)
            => double.Parse(val.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture);

        private static float ParseFloat(string val)
            => float.Parse(val.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture);
    }
}
