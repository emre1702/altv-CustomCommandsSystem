using System;
using System.Collections.Generic;
using System.Globalization;

namespace CustomCommandSystem.Common.Datas
{
    public static class DefaultConverters
    {
        public static Dictionary<Type, (int ArgsLength, Func<ArraySegment<string>, object> Converter)> Data { get; } = new Dictionary<Type, (int ArgsLength, Func<ArraySegment<string>, object> Converter)>
        {
            [typeof(bool)] = (1, args => ParseBool(args[0])),
            [typeof(byte)] = (1, args => byte.Parse(args[0])),
            [typeof(sbyte)] = (1, args => sbyte.Parse(args[0])),
            [typeof(char)] = (1, args => char.Parse(args[0])),
            [typeof(decimal)] = (1, args => decimal.Parse(args[0].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture)),
            [typeof(double)] = (1, args => ParseDouble(args[0])),
            [typeof(float)] = (1, args => ParseFloat(args[0].Replace(',', '.'))),
            [typeof(int)] = (1, args => int.Parse(args[0])),
            [typeof(uint)] = (1, args => uint.Parse(args[0])),
            [typeof(long)] = (1, args => long.Parse(args[0])),
            [typeof(ulong)] = (1, args => ulong.Parse(args[0])),
            [typeof(short)] = (1, args => short.Parse(args[0])),
            [typeof(ushort)] = (1, args => ushort.Parse(args[0])),
            [typeof(string)] = (1, args => args[0]),

            [typeof(GTANetworkAPI.Player)] = (1, args => GTANetworkAPI.NAPI.Player.GetPlayerFromName(args[0])),
            [typeof(GTANetworkAPI.Vector3)] = (3, args => new GTANetworkAPI.Vector3(ParseDouble(args[0]), ParseDouble(args[1]), ParseDouble(args[2]))),
            [typeof(GTANetworkAPI.Color)] = (3, args => new GTANetworkAPI.Color(int.Parse(args[0]), int.Parse(args[1]), int.Parse(args[2]))),
            [typeof(GTANetworkAPI.ComponentVariation)] = (3, args => new GTANetworkAPI.ComponentVariation(int.Parse(args[0]), int.Parse(args[1]), int.Parse(args[2]))),
            [typeof(GTANetworkAPI.Decoration)] = (2, args => new GTANetworkAPI.Decoration { Collection = uint.Parse(args[0]), Overlay = uint.Parse(args[1]) }),
            [typeof(GTANetworkAPI.HeadBlend)] = (9, args => new GTANetworkAPI.HeadBlend
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
            [typeof(GTANetworkAPI.HeadOverlay)] = (4, args => new GTANetworkAPI.HeadOverlay
            {
                Index = byte.Parse(args[0]),
                Opacity = ParseFloat(args[1]),
                Color = byte.Parse(args[2]),
                SecondaryColor = byte.Parse(args[3])
            }),
            [typeof(GTANetworkAPI.Quaternion)] = (4, args => new GTANetworkAPI.Quaternion(ParseDouble(args[0]), ParseDouble(args[1]), ParseDouble(args[2]), ParseDouble(args[3]))),
            [typeof(GTANetworkAPI.VehiclePaint)] = (2, args => new GTANetworkAPI.VehiclePaint(int.Parse(args[0]), int.Parse(args[1]))),
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
