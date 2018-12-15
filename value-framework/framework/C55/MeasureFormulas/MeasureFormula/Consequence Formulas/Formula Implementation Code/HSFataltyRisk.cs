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
    public class HSFataltyRisk : HSFataltyRiskBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            return null;
            // TODO - add measure
            
//            double?[] Public = timeInvariantData.HealthAndSafety_NumberOfMemberOfPublicFatalities_LikelihoodUnitOutput;
//    		double PublicCost = timeInvariantData.SystemCostOfMemberOfPublicFatality ?? 0;
    		
//    		double?[] Operative = timeInvariantData.HealthAndSafety_NumberOfOperativeFatalities_LikelihoodUnitOutput;
//    		double OperativeCost = timeInvariantData.SystemCostOfOpertiveFatality ?? 0;
    		
//    		var Array1 = ArrayHelper.MultiplyStreamOfValuesByConstant(Public, PublicCost);
//    		var Array2 = ArrayHelper.MultiplyStreamOfValuesByConstant(Operative, OperativeCost);
    			
//    		return ArrayHelper.SumArrays(new []{ Array1, Array2});
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
