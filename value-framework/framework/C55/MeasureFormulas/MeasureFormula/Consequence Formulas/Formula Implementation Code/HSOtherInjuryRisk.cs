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
    public class HSOtherInjuryRisk : HSOtherInjuryRiskBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            return null;
            // TODO - add measure

//            double?[] NoOtherInjuries = timeInvariantData.HealthAndSafety_NumberOfOtherInjuries_LikelihoodUnitOutput;
//    		double CostOther = timeInvariantData.SystemOtherInjuryCost ?? 0;
//    		
//    		return ArrayHelper.MultiplyStreamOfValuesByConstant(NoOtherInjuries, CostOther);
    		
        }
        
        public override double?[] GetZynos(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData,
            IReadOnlyList<TimeVariantInputDTO> timeVariantData,
            double?[] unitOutput)
        {
            return unitOutput;
        }
    }
}
