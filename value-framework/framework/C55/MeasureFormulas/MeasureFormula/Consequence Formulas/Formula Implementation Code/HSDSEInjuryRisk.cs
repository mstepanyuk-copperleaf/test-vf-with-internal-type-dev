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
    public class HSDSEInjuryRisk : HSDSEInjuryRiskBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            return null;
            //TDO add measure
//            double?[] NoDSEInjuries = timeInvariantData.HealthAndSafety_NumberOfDSEInjuries_LikelihoodUnitOutput;
//    		double CostDSE = timeInvariantData.SystemDSEInjuryCost ?? 0;
    		
//    		return ArrayHelper.MultiplyStreamOfValuesByConstant(NoDSEInjuries, CostDSE);
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
