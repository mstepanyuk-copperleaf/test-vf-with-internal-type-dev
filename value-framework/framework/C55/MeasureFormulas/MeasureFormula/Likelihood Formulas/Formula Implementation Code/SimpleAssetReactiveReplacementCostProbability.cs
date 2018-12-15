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
    public class SimpleAssetReactiveReplacementCostProbability : SimpleAssetReactiveReplacementCostProbabilityBase
    {
        public override double?[] GetLikelihoodValues(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            // aka Avoided Future Asset Replacement
            return MonthlyBaselineProbabilitiesUntilImpact(
                timeInvariantData.ConditionScore_Condition_ConsqUnitOutput_B,
                timeInvariantData.ConditionToFailureCurve,
                timeInvariantData.ConditionScore_Condition_ConsqUnitOutput,
                CustomerConstants.BestConditionScore,
                CustomerConstants.WorstConditionScore
            );
        }
    }
}
