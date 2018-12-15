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
	public class AvoidedFutureAssetReplacementConsequence : AvoidedFutureAssetReplacementConsequenceBase
	{
		public override double?[] GetUnits(int startFiscalYear, int months,
		                                   TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
		{
			if (timeInvariantData.InvestmentSpendByAccountType == null)
			{
				return null;
			}
			var investmentCost = GetSpendForAllAccountTypes(months, timeInvariantData.InvestmentSpendByAccountType);
			double totalCost = 0;
			for (var i = 0; i < months; i++)
			{
				totalCost += (investmentCost[i] ?? 0);
			}
			
			var result = new double?[months];
		    //get offset from alternative milestone
            var milestoneOffset = CommonHelperFunctions.GetAlternativeMilestoneOffset(startFiscalYear,
		        timeInvariantData.AlternativeMilestones,
		        timeInvariantData.SystemAlternativeMilestoneISDCode);

		    //negative value for avoided cost on outcome.
            var outcomeValue = -1d * timeInvariantData.SystemReactiveReplacementCostFactor * totalCost;

            if (milestoneOffset.HasValue)
		    {
		        for (var i = Math.Max(0, milestoneOffset.Value); i < months; i++)
		        {
		            result[i] = outcomeValue;
		        }
            }
            else {
                FillWithValueAfterEndOfSpend (months, timeInvariantData.InvestmentSpendByAccountType, ref result, outcomeValue); 
            }

			return result;
		}
		
		public override double?[] GetZynos(int startFiscalYear, int months,
		                                   TimeInvariantInputDTO timeInvariantData,
		                                   IReadOnlyList<TimeVariantInputDTO> timeVariantData,
		                                   double?[] unitOutput)
		{
			return ConvertUnitsToZynos(unitOutput, CustomerConstants.DollarToZynoConversionFactor);
		}
	}
}
