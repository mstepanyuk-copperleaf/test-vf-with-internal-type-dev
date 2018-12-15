using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CL.FormulaHelper;
using CL.FormulaHelper.Attributes;
using CL.FormulaHelper.DTOs;
using MeasureFormula.Common_Code;

namespace CustomerFormulaCode
{
    /**
     * This class exists so that we can test if Parsitron is able to parse and pickup internal output types.
     */
    [Formula]
    public class DummyInternalBase : FormulaInternalBase
    {

        [DataContract]
        public class DummyContract
        {
            [PromptInput("BenefitProbability")]
            [DataMember]
            public System.Int32 BenefitProbability  { get; private set; }

            [CoreFieldInput(FormulaCoreFieldInputType.TimePeriod)]
            [DataMember]
            public CL.FormulaHelper.DTOs.TimePeriodDTO dummyTimePeriodInput  { get; private set; }

            [MeasureInput("ConditionScore", "Condition", MeasureOutputType.ConsqUnitOutput, false)]
            [DataMember]
            public System.Double?[] dummyMeasureInput  { get; private set; }

        }

        public override Type[] GetKnownTypes()
        {
            return new[] {this.GetType()};
        }
    }
}
