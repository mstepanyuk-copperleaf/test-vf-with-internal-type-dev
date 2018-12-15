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
    public class CMTotalMilesCost : CMTotalMilesCostBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
    		double?[] companyMiles = timeInvariantData.Mileage_TotalMileageCompany_LikelihoodUnitOutput;
    		
    		double companyCost = timeInvariantData.SystemCompanyMileageCostPoundsPerMile ?? 0;
    		
    		var totalCMileageCost = ArrayHelper.MultiplyStreamOfValuesByConstant(companyMiles, companyCost);
    		
    		return totalCMileageCost;
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
