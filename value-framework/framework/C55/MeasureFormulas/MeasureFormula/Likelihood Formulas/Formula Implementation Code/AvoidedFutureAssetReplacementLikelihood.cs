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
    public class AvoidedFutureAssetReplacementLikelihood : AvoidedFutureAssetReplacementLikelihoodBase
    {
        public override double?[] GetLikelihoodValues(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
			if (timeInvariantData.InvestmentSpendByAccountType == null)
			{
				return null;
			}

			var annualProbabilities = new double?[months];

            //get offset from alternative milestone
            var milestoneOffset = CommonHelperFunctions.GetAlternativeMilestoneOffset(startFiscalYear,
                timeInvariantData.AlternativeMilestones,
                timeInvariantData.SystemAlternativeMilestoneISDCode);
            
            //if milestone offset exists, use that. Otherwise use end of spend offset
            var firstMonth = CommonHelperFunctions.GetImpactOffset(milestoneOffset, timeInvariantData.InvestmentSpendByAccountType) ?? 0;
		    var endMonth = Math.Min(months, firstMonth + CommonConstants.MonthsPerYearInt * timeInvariantData.YearsToCertainReplacement);

            for (var i = Math.Max(0, firstMonth); i < endMonth; i++)
			{
				annualProbabilities[i] = 1d / timeInvariantData.YearsToCertainReplacement;
			}

			return HelperFunctions.GetMonthlyProbability(annualProbabilities);
        }
    }
}
