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
    public class HSMinorInjuryRisk : HSMinorInjuryRiskBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
	        return null;
	        
	        // TODO - add measure
//    		double?[] Public = timeInvariantData.HealthAndSafety_NumberOfMemberOfPublicMinorInjuries_LikelihoodUnitOutput;
//    		double PublicCost = timeInvariantData.SystemCostOfMemberOfPublicMinorInjury ?? 0;
    		
//    		double?[] Operative = timeInvariantData.HealthAndSafety_NumberOfOperativeMinorInjuries_LikelihoodUnitOutput;
//    		double OperativeCost = timeInvariantData.SystemCostOfOperativeMinorInjury ?? 0;
    		
//    		var Array1 = ArrayHelper.MultiplyStreamOfValuesByConstant(Public, PublicCost);
//    		var Array2 = ArrayHelper.MultiplyStreamOfValuesByConstant(Operative, OperativeCost);
    			
//    		return ArrayHelper.SumArrays(new [] {Array1, Array2});
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
