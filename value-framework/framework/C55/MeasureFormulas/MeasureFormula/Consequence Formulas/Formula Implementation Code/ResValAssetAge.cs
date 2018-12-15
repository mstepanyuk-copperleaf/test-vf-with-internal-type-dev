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
    public class ResValAssetAge : ResValAssetAgeBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
    		double? AssetInServiceYear = 0;     		  		
    		    		
    		DateTime alternativeStartDate = timeInvariantData.AlternativeStartDate;
    		
    		if (timeInvariantData.AssetEffectiveAge == null)
		    {
    			AssetInServiceYear = timeInvariantData.AlternativeStartDate.Month < CoreConstants.AssetStartDateMonthOffset ? timeInvariantData.AlternativeStartDate.Year - 1: timeInvariantData.AlternativeStartDate.Year;
    		} else
		    {
    			AssetInServiceYear = timeInvariantData.AssetEffectiveAge.Value.Year;
    		}
    		
    		double?[] AssetAge = new double?[months];

    		for (int i = 0; i < months; i++)
		    {
			    if (startFiscalYear + i / CommonConstants.MonthsPerYear < AssetInServiceYear + 1)
			    {
				    AssetAge[i] = null;
			    } else
			    {
				    AssetAge[i] = (Math.Floor(i / CommonConstants.MonthsPerYear) - (AssetInServiceYear + 1 - startFiscalYear))
				                  / CommonConstants.MonthsPerYear;
			    }
		    }

			return AssetAge;
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
