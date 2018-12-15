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
	/// Baseline formula for avoided costs based on the hours of labour as well as direct costs expected before the investment is completed.
	/// </summary>
	[Formula]
	public class CostAvoidanceTotalConsequenceBaseline : CostAvoidanceTotalConsequenceBaselineBase
	{
		public override double?[] GetUnits(int startFiscalYear, int months,
		                                   TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
		{
        	double applicableLabourRate = 
        		(timeInvariantData.AccountType.ValueAsInteger == CoreConstants.OMAAcctID)  ?
        		CoreConstants.OPEXLabourHour : CoreConstants.CAPEXLabourHour;

        	return InterpolatePropagate<TimeVariantInputDTO>(timeVariantData,
			                                                 startFiscalYear,
			                                                 months, (x => (x.CostsIncurredIfThisInvestmentIsNotUndertaken + x.HoursIncurredIfThisInvestmentIsNotUndertaken
			                                                                                                * applicableLabourRate)));
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
