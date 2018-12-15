using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.Common_Code;

namespace CustomerFormulaCode
{
	[Formula]
	public class RiskLikelihoodAutomaticOutcome : RiskLikelihoodAutomaticOutcomeBase
	{
		public override double?[] GetLikelihoodValues(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
		{
			var result = new double?[months];

			// return 0 values after the end of spend
			FillWithValueAfterEndOfSpend(months, timeInvariantData.InvestmentSpendByAccountType, ref result, 0);

			return result;
		}
	}
}
