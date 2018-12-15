using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.Common_Code;
using MeasureFormula.SharedCode;


namespace CustomerFormulaCode
{
    [Formula]
    public class SimpleAssetConditionalProbabilityOutcomeAA : SimpleAssetConditionalProbabilityOutcomeAABase
    {
        public override double?[] GetLikelihoodValues(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            // If there are no outcome condition scores then there will be no outcome likelihood values.
            if (timeVariantData.Count == 0)
            {
                return null;
            }
            var firstIntervention = timeVariantData[0];

            return ProbabilityHelpers.LegacyAAOutcomeProbabilities(
            	  startFiscalYear,
            	  firstIntervention.TimePeriod.StartTime,
    		      timeInvariantData.ConditionScore_Condition_ConsqUnitOutput_B,
	              CustomerConstants.BestConditionScore,
    		      timeInvariantData.ConditionToFailureCurve);            	
        }
    }
}
