using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    /// <summary>
    /// Consequence calculated in risk units based on the risk matrix.
    /// Driven by questionnaire input.
    /// Returns annual consequence values.  Intended to be used in conjunction with a monthly probability value.
    /// </summary>
    [Formula]
	public class RiskConsequenceManual : RiskConsequenceManualBase
	{
		public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
		{
		    return InterpolatePropagate<TimeVariantInputDTO>(timeVariantData, startFiscalYear, months, 
                x => x.ManualRiskConsequence.AvgValue);
		}
		
		public override double?[] GetZynos(int startFiscalYear, int months, TimeInvariantInputDTO timeInvariantData,
            IReadOnlyList<TimeVariantInputDTO> timeVariantData, double?[] unitOutput)
		{
			return unitOutput;
		}
	}
}
