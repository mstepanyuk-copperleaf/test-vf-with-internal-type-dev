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
	/// Outcome formula for avoided costs based on the hours of labour as well as direct costs expected after the investment is completed.
	/// </summary>
	[Formula]
	public class CostAvoidanceTotalConsequenceOutcome : CostAvoidanceTotalConsequenceOutcomeBase
	{
		public override double?[] GetUnits(int startFiscalYear, int months,
		                                   TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
		{
        	double applicableLabourRate = 
        		(timeInvariantData.AccountType.ValueAsInteger == CoreConstants.OMAAcctID)  ?
        		CoreConstants.OPEXLabourHour : CoreConstants.CAPEXLabourHour;

        	return InterpolatePropagate<TimeVariantInputDTO>(timeVariantData,
			                                                 startFiscalYear,
			                                                 months, (x => (x.CostsIncurredAfterThisInvestmentIsUndertaken + 
			                                                 	x.HoursIncurredIfThisInvestmentIsUndertaken * applicableLabourRate)));
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
