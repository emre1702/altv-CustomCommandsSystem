﻿using CustomCommandsSystem.Common.Delegates;
using CustomCommandsSystem.Common.Models;
using AltV.Net.Elements.Entities;
using System;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CustomCommandsSystem.Common.Interfaces.Services
{
    internal interface ICommandArgumentsConverter
    {
        event EmptyDelegate? ConverterChanged;

        ValueTask<(object? ConvertedValue, int AmountArgsUsed, bool? AllowNull)> Convert(IPlayer player, UserInputData userInputData, int atIndex, Type toType, CancelEventArgs errorMessageCancel);
        void SetAsyncConverter(Type forType, int argumentsLength, AsyncConverterDelegate asyncConverter, bool? allowNull = false);
        void SetConverter(Type forType, int argumentsLength, ConverterDelegate converter, bool? allowNull = false);
        internal int? GetTypeArgumentsLength(Type type); 
    }
}