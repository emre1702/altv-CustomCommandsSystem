﻿using AltV.Net.Elements.Entities;
using CustomCommandsSystem.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CustomCommandsSystem.Common.Models
{
    internal class CommandMethodData
    {
        public MethodInfo Method { get; }
        public Delegate FastInvokeHandler { get; }
        public bool IsPlayerRequired { get; }
        public bool IsCommandInfoRequired { get; }
        public int? RemainingParamsToStringIndex { get; set; }
        public int Priority { get; }
        public int UserParametersStartIndex => (IsPlayerRequired ? 1 : 0) + (IsCommandInfoRequired ? 1 : 0);
        public List<CommandParameterData> UserParameters { get; set; } = new List<CommandParameterData>();
        public string[] Aliases { get; }
        public CustomCommandRequirementBaseAttribute[] RequirementCheckers { get; }

        public int MinUserArgsAmount { get; private set; } = 0;
        public int MaxUserArgsAmount { get; private set; } = int.MaxValue;
        public int? ExactUserArgsAmount { get; private set; }
        public object? Instance { get; internal set; }

        private readonly Type _playerType = typeof(IPlayer);
        private readonly Type _customCommandInfoType = typeof(CustomCommandInfo);

        public CommandMethodData(MethodInfo method, Delegate fastInvokeHandler, int priority)
        {
            Method = method;
            FastInvokeHandler = fastInvokeHandler;
            Priority = priority;
            IsPlayerRequired = GetIsPlayerRequired(method);
            IsCommandInfoRequired = GetIsCommandInfoRequired(method);

            Aliases = method.GetCustomAttributes<CustomCommandAliasAttribute>().SelectMany(a => a.Aliases).ToArray();
            RequirementCheckers = method.GetCustomAttributes().Union(method.DeclaringType!.GetCustomAttributes()).OfType<CustomCommandRequirementBaseAttribute>().ToArray();
        }

        internal void CompleteInitialization()
        {
            MinUserArgsAmount = UserParameters.Sum(GetAmountUserArgsNeeded);
            MaxUserArgsAmount = GetAmountUserArgsMaxPossible();
            ExactUserArgsAmount = GetExactAmounterUserArgsRequired();
        }

        private int GetAmountUserArgsNeeded(CommandParameterData parameterData)
        {
            if (parameterData.HasDefaultValue) return 0;
            if (parameterData.IsRemainingText) return 1;

            return parameterData.UserInputLength;
        }

        private int GetAmountUserArgsMaxPossible()
        {
            if (UserParameters.Any(u => u.IsRemainingText)) return int.MaxValue;
            return UserParameters.Sum(u => u.UserInputLength);
        }

        private int? GetExactAmounterUserArgsRequired()
        {
            if (UserParameters.Any(u => u.DefaultValue is { } || u.IsRemainingText)) return null;
            return UserParameters.Sum(u => u.UserInputLength);
        }

        private bool GetIsPlayerRequired(MethodInfo method)
        {
            var parameters = method.GetParameters();
            return parameters.Length >= 1 && parameters[0].ParameterType.IsAssignableTo(_playerType);
        }

        private bool GetIsCommandInfoRequired(MethodInfo method)
        {
            var parameters = method.GetParameters();
            return parameters.Length >= 1 && parameters[0].ParameterType.IsAssignableTo(_customCommandInfoType)
                || parameters.Length >= 2 && parameters[0].ParameterType.IsAssignableTo(_playerType) && parameters[1].ParameterType.IsAssignableTo(_customCommandInfoType);
        }
    }
}
