using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.Common_Code;

namespace CustomerFormulaCode
{
    [Formula]
    public class SimpleAssetEnvironmentalProbabilityOutcome : SimpleAssetEnvironmentalProbabilityOutcomeBase
    {
        public override double?[] GetLikelihoodValues(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
	        if (timeInvariantData.ConditionToFailureCurve == null) return null;
	        return LikelihoodProbability.GetLikelihoodValues(months,
	                                                         timeInvariantData.ConditionToFailureCurve,
	                                                         timeInvariantData.ConditionScore_Condition_ConsqUnitOutput,
	                                                         timeInvariantData.AssetTypeEnvironmentalRiskExposureFactor);
        }
	}
}
