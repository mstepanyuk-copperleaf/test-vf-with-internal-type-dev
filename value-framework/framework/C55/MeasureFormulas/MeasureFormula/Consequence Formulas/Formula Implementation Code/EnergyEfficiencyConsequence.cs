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
	public class EnergyEfficiencyConsequence : EnergyEfficiencyConsequenceBase
	{
		public override double?[] GetUnits(int startFiscalYear, int months,
		                                   TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
		{
		    var energySavingsPerMWh = (timeInvariantData.SystemEnergySavingsValueDollarsPerMWh ?? 0) +
		                        (timeInvariantData.SystemCO2perMWh ?? 0) * (timeInvariantData.SystemGHGValue ?? 0);
            
		    return InterpolatePropagate<TimeVariantInputDTO>(timeVariantData, startFiscalYear, months,
		        x => x.PowerSavings * energySavingsPerMWh);
        }
		
		public override double?[] GetZynos(int startFiscalYear, int months, TimeInvariantInputDTO timeInvariantData, 
            IReadOnlyList<TimeVariantInputDTO> timeVariantData, double?[] unitOutput)
		{
			return ConvertUnitsToZynos(unitOutput, CustomerConstants.DollarToZynoConversionFactor);
		}
	}
}
