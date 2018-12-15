using System;
using System.Collections.Generic;
using AutoFixture;

namespace MeasureFormula.TestHelpers
{
    public enum Parameter
    {
        SystemCustomerTairConsequence,
        AccountType,
        TimePeriod
    }

    public static class ParameterBuilder
    {
        private static readonly Dictionary<Parameter, FormulaParameter> ParameterDictionary;

        static ParameterBuilder()
        {
            ParameterDictionary = new Dictionary<Parameter, FormulaParameter>();

            AddToDictionary(Parameter.SystemCustomerTairConsequence, "p_SystemCustomerTAIRConsequence");
            AddToDictionary(Parameter.AccountType, "p_AccountType");
            AddToDictionary(Parameter.TimePeriod, "p_TimePeriod");

        }

        private static void AddToDictionary(Parameter parameterCode, string parameterName)
        {
            ParameterDictionary.Add(parameterCode, new FormulaParameter
            {
                ParameterName = parameterName,
                Value = null
            });
        }

        public static void SetConstructorParameter<T>(Fixture fixture, Parameter parameterCode, T parameterValue)
        {
            FormulaParameter parameter;

            if (ParameterDictionary.TryGetValue(parameterCode, out parameter))
            {
                DataPrep.SetConstructorParameter(fixture, parameter.ParameterName, parameterValue);
            }
        }

        public static void SetConstructorParameterWithDefaultValue(Fixture fixture, Parameter parameterCode)
        {
            FormulaParameter parameter;

            if (!ParameterDictionary.TryGetValue(parameterCode, out parameter) || parameter.Value == null) return;

            //need to convert the object to the right run-time type in order for AutoFixture to correctly set the
            //constructor parameter
            var valueWithActualType = Convert.ChangeType(parameter.Value, parameter.Value.GetType());
            DataPrep.SetConstructorParameter(fixture, parameter.ParameterName, valueWithActualType);
        }
    }

    public class FormulaParameter
    {
        public string ParameterName { get; set; }
        public object Value { get; set; }
    }
}
