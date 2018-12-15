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
    public class CMCarbonT : CMCarbonTBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
    		double?[] companyMileage= timeInvariantData.Mileage_TotalMileageCompany_LikelihoodUnitOutput;
    		double carbonConversion = timeInvariantData.SystemCarbonConversionFactorPerMile ?? 0;
    		
    		return ArrayHelper.MultiplyStreamOfValuesByConstant(companyMileage, carbonConversion/CoreConstants.TonnesToKgs); 		
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