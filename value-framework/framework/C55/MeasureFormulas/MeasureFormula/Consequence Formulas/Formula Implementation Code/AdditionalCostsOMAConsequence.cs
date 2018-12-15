using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.Common_Code;

namespace CustomerFormulaCode
{
    public class OperationalExpenseCalculator
    {
        private readonly double OperationalLabourRate;
        public OperationalExpenseCalculator(double operationalLabourRate)
        {
            OperationalLabourRate = operationalLabourRate;
        }
        public double AnnualAdditionalCostsFor(long AdditionalCosts, double AdditionalHours)
        {
            return AdditionalCosts + AdditionalHours * OperationalLabourRate;
        }
    }

    /// <summary>
	/// Returns a positive value for the additional O&amp;M costs incurred by completing an investment.  it is assumed that negative polarity will be applied in the value function.
	/// Annual values entered in the questionnaire are converted to a monthly equivalent as no probability is specified for this formula.
	/// </summary>
	[Formula]
	public class AdditionalCostsOMAConsequence : AdditionalCostsOMAConsequenceBase
	{
		public override double?[] GetUnits(int startFiscalYear, int months,
		                                   TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
		{
			if (timeInvariantData.AccountType.ValueAsInteger != CoreConstants.OMAAcctID)
			{
				return null;
			}

            var operationalExpenseCalculator = new OperationalExpenseCalculator(CoreConstants.OPEXLabourHour);

		    return InterpolatePropagate<TimeVariantInputDTO>(timeVariantData,
		        startFiscalYear,
		        months,
		        (x => operationalExpenseCalculator.AnnualAdditionalCostsFor(x.AdditionalCosts, x.AdditionalHours)));
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
