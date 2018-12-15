using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.Common_Code;
using MeasureFormula.SharedCode;

namespace CustomerFormulaCode
{
	[Formula]
	public class EnvironmentalBenefitConsequence : EnvironmentalBenefitConsequenceBase
	{
		public override double?[] GetUnits(int startFiscalYear, int months,
		                                   TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
		{
			var ghgValue = timeInvariantData.SystemGHGValue ?? 0d;
			var energySavingsDollarsPerMwh = timeInvariantData.SystemEnergySavingsValueDollarsPerMWh ?? 0d;

			return InterpolatePropagate<TimeVariantInputDTO>(timeVariantData, startFiscalYear, months,
				x => x.CO2Reduction * ghgValue
				     + x.SF6Reduction * ghgValue * CommonConstants.TonnesCO2PerKgSF6
				     + x.EnergySavings * energySavingsDollarsPerMwh);
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
