using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.Common_Code;
using MeasureFormula.SharedCode;

namespace CustomerFormulaCode
{
	/// <summary>
	/// Public Perception Benefit Questionnaire formula calculates the consequence of an investment that improves
	/// community and stakeholder percetion of the company.
	/// This formula uses constants specified in CustomerConstants.cs
	/// Returns Annual Consequence values.  Expected to be used in conjunction with a likelihood formula that returns monthly values
	/// </summary>
	[Formula]
	public class PublicPerceptionConsequence : PublicPerceptionConsequenceBase
	{
		public override double?[] GetUnits(int startFiscalYear, int months,
		                                   TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
		{
			return InterpolatePropagate<TimeVariantInputDTO>(timeVariantData,
			                                                 startFiscalYear,
			                                                 months, (x => x.PercentageOfCustomersImpacted*CoreConstants.PublicPerceptionValueForAllCustomers/
			                                                          (CommonConstants.PercentPerProbabilityOne)));
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
