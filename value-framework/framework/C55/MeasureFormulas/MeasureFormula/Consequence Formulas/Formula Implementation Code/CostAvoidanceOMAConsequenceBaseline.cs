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
	public class CostAvoidanceOMAConsequenceBaseline : CostAvoidanceOMAConsequenceBaselineBase
	{
		public override double?[] GetUnits(int startFiscalYear, int months,
		                                   TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
		{
            if (timeInvariantData.AccountType == null 
	           || timeInvariantData.AccountType.ValueAsInteger != CoreConstants.OMAAcctID
	           || timeInvariantData.SystemCostAvoidanceFactor == null
	           || timeInvariantData.SystemOMAScalingFactor == null)
            {
                return null;
            }

            return InterpolatePropagate<TimeVariantInputDTO>(timeVariantData, startFiscalYear, months,
                x => (x.CostsIncurredIfThisInvestmentIsNotUndertaken 
                      + x.HoursIncurredIfThisInvestmentIsNotUndertaken * CoreConstants.OPEXLabourHour)
                   * timeInvariantData.SystemCostAvoidanceFactor * timeInvariantData.SystemOMAScalingFactor);

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
