using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class ConditionBaselineConsequence : ConditionBaselineConsequenceBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            return MonthlyConditionScores<TimeVariantInputDTO>(
                startFiscalYear,
                months,
                timeInvariantData.AssetInServiceDate,
                timeVariantData,
                timeInvariantData.AssetConditionDecayCurve,
                x => x.ConditionScore);
        }

        public override double?[] GetZynos(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData,
            IReadOnlyList<TimeVariantInputDTO> timeVariantData,
            double?[] unitOutput)
        {
            //condition score has no valid zynos conversion
            return null;
        }
    }
}
