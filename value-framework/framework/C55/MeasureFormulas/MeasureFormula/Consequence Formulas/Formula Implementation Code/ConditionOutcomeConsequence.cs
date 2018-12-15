using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;
using CL.FormulaHelper.DTOs;
using MeasureFormula.Common_Code;

namespace CustomerFormulaCode
{
    [Formula]
    public class ConditionOutcomeConsequence : ConditionOutcomeConsequenceBase
    {
    	public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            if (!timeInvariantData.AssetImpactedByAlternative)
            {
                return null;
            }

            return MonthlyConditionScores<TimeVariantInputDTO>(
                startFiscalYear,
                months, 
                timeInvariantData.AssetInServiceDate,
                timeVariantData,
                timeInvariantData.AssetConditionDecayCurve,
                x => x.OutcomeConditionScore);

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
