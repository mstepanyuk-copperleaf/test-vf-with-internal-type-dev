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
    public class SimpleAssetProbabilityPDFOutcome : SimpleAssetProbabilityPDFOutcomeBase
    {
        public override double?[] GetLikelihoodValues(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            return ProbabilityHelpers.PDFSubsamplingOutcomeProbabilities (
                 timeInvariantData.ConditionScore_Condition_ConsqUnitOutput_B,
                 timeInvariantData.ConditionScore_Condition_ConsqUnitOutput,
                 CustomerConstants.BestConditionScore,
                 CustomerConstants.WorstConditionScore,
                 timeInvariantData.ConditionToFailureCurve);
        }
    }
}
