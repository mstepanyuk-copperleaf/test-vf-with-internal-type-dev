using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    /// <summary>
    /// Null baseline used for value measure that are sometimes calculated with a baseline and sometimes without.
    /// Returns an array of 0's
    /// </summary>
    [Formula]
	public class NullBaseline : NullBaselineBase
	{
		public override double?[] GetUnits(int startFiscalYear, int months, 
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
		{
			return PopulateOutputWithValue(months, 0d);
		}
		
		public override double?[] GetZynos(int startFiscalYear, int months, TimeInvariantInputDTO timeInvariantData,
            IReadOnlyList<TimeVariantInputDTO> timeVariantData, double?[] unitOutput)
		{
			return unitOutput;
		}
	}
}
