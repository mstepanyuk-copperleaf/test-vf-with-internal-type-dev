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
    public class ConditionFromHoursOutcome : ConditionFromHoursOutcomeBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
    		if (timeInvariantData.AssetOperatingHours == null || timeInvariantData.AssetOpHrsDate == null ||
                timeInvariantData.AssetExpectedAnnualOperatingHours == null)
    		{
    			return null;
    		}
			
    		if (timeInvariantData.AssetImpact == false)
    		{
    			return null; // Asset not impacted by alternative
    		}

            //get offset from alternative milestone
            var milestoneOffset = CommonHelperFunctions.GetAlternativeMilestoneOffset(startFiscalYear,
                timeInvariantData.AlternativeMilestones,
                timeInvariantData.SystemAlternativeMilestoneISDCode);
            
            var impactOffset =
                CommonHelperFunctions.GetImpactOffset(milestoneOffset, timeInvariantData.InvestmentSpendByAccountType);
            if (impactOffset == null) return null;
            
            //ensure endOfOffsetMonths is non-negative
            var endOfSpendOffsetMonths = Math.Max(0, impactOffset.Value);
			var opHoursOutcome = new double?[months];

			opHoursOutcome[endOfSpendOffsetMonths] = 0;
            for (var i = endOfSpendOffsetMonths + 1; i < months; i++)
			{
			    opHoursOutcome[i] = opHoursOutcome[i - 1] + timeInvariantData.AssetExpectedAnnualOperatingHours / 
                    CommonConstants.MonthsPerYear;                			        
			}
			
            return CommonHelperFunctions.ComputeConditionFromEoh(opHoursOutcome, 
                timeInvariantData.AssetConditionDecayCurve, startFiscalYear, months);
        }
        
        public override double?[] GetZynos(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData,
            IReadOnlyList<TimeVariantInputDTO> timeVariantData,
            double?[] unitOutput)
        {
            return null;  //Condition not expressed in Zynos
        }
    }
}
