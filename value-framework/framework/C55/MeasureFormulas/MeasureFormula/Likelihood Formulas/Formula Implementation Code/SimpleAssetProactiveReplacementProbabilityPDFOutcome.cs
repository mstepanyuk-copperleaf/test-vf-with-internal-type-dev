using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.Common_Code;

namespace CustomerFormulaCode
{
    [Formula]
    public class SimpleAssetProactiveReplacementProbabilityPDFOutcome : SimpleAssetProactiveReplacementProbabilityPDFOutcomeBase
    {
        public override double?[] GetLikelihoodValues(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
    		// TODO: document or make a new helper function 
            return ProbabilityEventDoesNotOccurPriorToImpact(
                timeInvariantData.ConditionScore_Condition_ConsqUnitOutput_B,
                timeInvariantData.ConditionToFailureCurve,
                timeInvariantData.ConditionScore_Condition_ConsqUnitOutput,
                CustomerConstants.BestConditionScore,
                CustomerConstants.WorstConditionScore);
        }
    }
}
