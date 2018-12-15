using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.Common_Code;

namespace CustomerFormulaCode
{
	/// <summary>
	/// Customer Contribution costs in dollars.
	/// No likelihood formula expected to be used.
	/// Typically applied as a constraint.
	/// Uses the customer contribution account code (CustomerContributionAccount) specified in CustomerConstants.cs
	/// </summary>
	[Formula]
	public class CustomerContributionConsequenceMonthly : CustomerContributionConsequenceMonthlyBase
	{
		public override double?[] GetUnits(int startFiscalYear, int months,
		                                   TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
		{
			var values = GetSpendForAccountType(months, timeInvariantData.InvestmentSpendByAccountType,CoreConstants.CustomerContributionAccount);

			return values;
		}
		
		public override double?[] GetZynos(int startFiscalYear, int months,
		                                   TimeInvariantInputDTO timeInvariantData,
		                                   IReadOnlyList<TimeVariantInputDTO> timeVariantData,
		                                   double?[] unitOutput)
		{
			return null;
		}
	}
}
