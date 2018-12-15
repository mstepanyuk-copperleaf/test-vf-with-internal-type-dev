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
    public class HSRiskGeneralTotal : HSRiskGeneralTotalBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            return null;
            // TODO - add measure
            
//    		double?[] Fatalities = timeInvariantData.HealthAndSafety_HealthAndSafetyRiskFatalities_ConsqUnitOutput;
//    		double?[] MajorInjuries = timeInvariantData.HealthAndSafety_HealthAndSafetyRiskMajorInjuries_ConsqUnitOutput;
//    		double?[] MinorInjuries = timeInvariantData.HealthAndSafety_HealthAndSafetyRiskMinorInjuries_ConsqUnitOutput;
//        
//    		return ArrayHelper.SumArrays(new [] {Fatalities, MajorInjuries, MinorInjuries});
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
