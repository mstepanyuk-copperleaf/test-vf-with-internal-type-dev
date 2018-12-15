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
    public class CMCostsAvoided : CMCostsAvoidedBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            return null;
            
            // TODO: bring in new measure
            
/*
            double?[] companyMResidual = timeInvariantData.Mileage_CostOfTotalMileageCompanyPounds_ConsqUnitOutput;
    		double?[] companyMBaseline = timeInvariantData.Mileage_CostOfTotalMileageCompanyPounds_ConsqUnitOutput_B;
        
    		var residualMileageCost = ArrayHelper.MultiplyStreamOfValuesByConstant(companyMResidual, -1.0); // -1 multiplied to outcome to get Baseline - Outcome
    		
    		return ArrayHelper.SumArrays(new []{residualMileageCost, companyMBaseline});		
*/
            
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
