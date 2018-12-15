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
    public class ConditionFromHoursBaseline : ConditionFromHoursBaselineBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
    		if (timeInvariantData.AssetOperatingHours == null || timeInvariantData.AssetOpHrsDate == null || 
                timeInvariantData.AssetExpectedAnnualOperatingHours == null)
    		{
    			return null;
    		}

    		var opHours = new double?[months];
    		var operatinghoursOffset = ConvertDateTimeToOffset(timeInvariantData.AssetOpHrsDate ?? DateTime.Now, startFiscalYear);
            
            if (operatinghoursOffset >= months)
            {
                return null;
            }

            var monthlyExpectedOperatingHours = timeInvariantData.AssetExpectedAnnualOperatingHours /
                                                CommonConstants.MonthsPerYear;
            double? initialOperatingHours = timeInvariantData.AssetOperatingHours;

            //if operating hours offset is in the past, we need to calculate how many hours the asset has been in
            //operating until index 0 of month array
            if (operatinghoursOffset < 0)
            {
                var numMonthsOfOffset = operatinghoursOffset * -1d;
                initialOperatingHours += numMonthsOfOffset * monthlyExpectedOperatingHours;
                operatinghoursOffset = 0; //set offset to 0 so we can continue on with the rest of the calculation
            }

            opHours[operatinghoursOffset] = initialOperatingHours;
			for (var i = operatinghoursOffset + 1; i < months; i++)
			{
			    opHours[i] = opHours[i - 1] + monthlyExpectedOperatingHours;                
			}
			
		    return CommonHelperFunctions.ComputeConditionFromEoh(opHours, timeInvariantData.AssetConditionDecayCurve, startFiscalYear, months);
        }
        
        public override double?[] GetZynos(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData,
            IReadOnlyList<TimeVariantInputDTO> timeVariantData,
            double?[] unitOutput)
        {
            return null;  //Condition Score not expressed in Zynos
        }
    }
}
