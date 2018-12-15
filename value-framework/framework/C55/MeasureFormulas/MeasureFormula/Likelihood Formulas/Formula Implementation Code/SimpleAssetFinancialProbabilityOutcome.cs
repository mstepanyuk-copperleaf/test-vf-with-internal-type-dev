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
    public class SimpleAssetFinancialProbabilityOutcome : SimpleAssetFinancialProbabilityOutcomeBase
    {
        public override double?[] GetLikelihoodValues(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
	        if (timeInvariantData.ConditionToFailureCurve == null) return null;
	        return LikelihoodProbability.GetLikelihoodValues(months,
	                                                         timeInvariantData.ConditionToFailureCurve,
	                                                         timeInvariantData.ConditionScore_Condition_ConsqUnitOutput,
	                                                         timeInvariantData.AssetTypeFinancialRiskExposureFactor);
        }
    }
}
