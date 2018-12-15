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
	/// Cost savings formula indicating the amount saved based on the hours of labour as well as direct costs avoided by the investment.
	/// This formula is expected to be used in conjunction with a null baseline formula.  A -1 multipler is included in the calculation so that a negative
	/// cost savings result for baseline-outcome will result in a positive value contribution.
	/// </summary>
	[Formula]
	public class CostSavingsTotalConsequence : CostSavingsTotalConsequenceBase
	{
		public override double?[] GetUnits(int startFiscalYear, int months,
		                                   TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
		{
        	double applicableLabourRate = 
        		(timeInvariantData.AccountType.ValueAsInteger == CoreConstants.OMAAcctID)  ?
        		CoreConstants.OPEXLabourHour : CoreConstants.CAPEXLabourHour;

        	return InterpolatePropagate<TimeVariantInputDTO>(timeVariantData,
			                                                 startFiscalYear,
			                                                 months, (x => (-1d * (x.CostsSaved + x.HoursSaved
			                                                                                                    * applicableLabourRate))));
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
