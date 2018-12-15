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
    public class PMTotalMilesCost : PMTotalMilesCostBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
    		double?[] privateMiles = timeInvariantData.Mileage_TotalMileagePrivate_LikelihoodUnitOutput;
    		double privateCost = timeInvariantData.SystemPrivateMileageCostPoundsPerMile ?? 0;
    		
    		var totalPMileageCost = ArrayHelper.MultiplyStreamOfValuesByConstant(privateMiles, privateCost);
    		
    		return totalPMileageCost;
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
