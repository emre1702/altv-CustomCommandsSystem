using CustomCommandSystem.Common.Delegates;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace CustomCommandSystem.Common.Datas
{
    public static class DefaultConverters
    {
        public static string? PlayerNotFoundErrorMessage = null;

        public static Dictionary<Type, (int ArgsLength, ConverterDelegate Converter)> Data { get; } 
            = new Dictionary<Type, (int ArgsLength, ConverterDelegate Converter)>
        {
            [typeof(bool)] = (1, (player, args, cancelErrorMsgOnFail) => ParseBool(args[0])),
            [typeof(byte)] = (1, (player, args, cancelErrorMsgOnFail) => byte.Parse(args[0])),
            [typeof(sbyte)] = (1, (player, args, cancelErrorMsgOnFail) => sbyte.Parse(args[0])),
            [typeof(char)] = (1, (player, args, cancelErrorMsgOnFail) => char.Parse(args[0])),
            [typeof(decimal)] = (1, (player, args, cancelErrorMsgOnFail) => decimal.Parse(args[0].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture)),
            [typeof(double)] = (1, (player, args, cancelErrorMsgOnFail) => ParseDouble(args[0])),
            [typeof(float)] = (1, (player, args, cancelErrorMsgOnFail) => ParseFloat(args[0].Replace(',', '.'))),
            [typeof(int)] = (1, (player, args, cancelErrorMsgOnFail) => int.Parse(args[0])),
            [typeof(uint)] = (1, (player, args, cancelErrorMsgOnFail) => uint.Parse(args[0])),
            [typeof(long)] = (1, (player, args, cancelErrorMsgOnFail) => long.Parse(args[0])),
            [typeof(ulong)] = (1, (player, args, cancelErrorMsgOnFail) => ulong.Parse(args[0])),
            [typeof(short)] = (1, (player, args, cancelErrorMsgOnFail) => short.Parse(args[0])),
            [typeof(ushort)] = (1, (player, args, cancelErrorMsgOnFail) => ushort.Parse(args[0])),
            [typeof(string)] = (1, (player, args, cancelErrorMsgOnFail) => args[0]),

            [typeof(GTANetworkAPI.Player)] = (1, (player, args, cancelErrorMsgOnFail) => 
            {
                var target = GTANetworkAPI.NAPI.Player.GetPlayerFromName(args[0]);
                if (target is null && PlayerNotFoundErrorMessage is { })
                {
                    cancelErrorMsgOnFail.Cancel = true;
                    GTANetworkAPI.NAPI.Task.Run(() => player.SendChatMessage(PlayerNotFoundErrorMessage));
                }
                return target;
            }),
            [typeof(GTANetworkAPI.Vector3)] = (3, (player, args, cancelErrorMsgOnFail) => new GTANetworkAPI.Vector3(ParseDouble(args[0]), ParseDouble(args[1]), ParseDouble(args[2]))),
            [typeof(GTANetworkAPI.Color)] = (3, (player, args, cancelErrorMsgOnFail) => new GTANetworkAPI.Color(int.Parse(args[0]), int.Parse(args[1]), int.Parse(args[2]))),
            [typeof(GTANetworkAPI.ComponentVariation)] = (3, (player, args, cancelErrorMsgOnFail) => new GTANetworkAPI.ComponentVariation(int.Parse(args[0]), int.Parse(args[1]), int.Parse(args[2]))),
            [typeof(GTANetworkAPI.Decoration)] = (2, (player, args, cancelErrorMsgOnFail) => new GTANetworkAPI.Decoration { Collection = uint.Parse(args[0]), Overlay = uint.Parse(args[1]) }),
            [typeof(GTANetworkAPI.HeadBlend)] = (9, (player, args, cancelErrorMsgOnFail) => new GTANetworkAPI.HeadBlend
            {
                ShapeFirst = byte.Parse(args[0]),
                ShapeSecond = byte.Parse(args[1]),
                ShapeThird = byte.Parse(args[2]),
                SkinFirst = byte.Parse(args[3]),
                SkinSecond = byte.Parse(args[4]),
                SkinThird = byte.Parse(args[5]),
                ShapeMix = ParseFloat(args[6]),
                SkinMix = ParseFloat(args[7]),
                ThirdMix = ParseFloat(args[8]),
            }),
            [typeof(GTANetworkAPI.HeadOverlay)] = (4, (player, args, cancelErrorMsgOnFail) => new GTANetworkAPI.HeadOverlay
            {
                Index = byte.Parse(args[0]),
                Opacity = ParseFloat(args[1]),
                Color = byte.Parse(args[2]),
                SecondaryColor = byte.Parse(args[3])
            }),
            [typeof(GTANetworkAPI.Quaternion)] = (4, (player, args, cancelErrorMsgOnFail) => new GTANetworkAPI.Quaternion(ParseDouble(args[0]), ParseDouble(args[1]), ParseDouble(args[2]), ParseDouble(args[3]))),
            [typeof(GTANetworkAPI.VehiclePaint)] = (2, (player, args, cancelErrorMsgOnFail) => new GTANetworkAPI.VehiclePaint(int.Parse(args[0]), int.Parse(args[1]))),
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
