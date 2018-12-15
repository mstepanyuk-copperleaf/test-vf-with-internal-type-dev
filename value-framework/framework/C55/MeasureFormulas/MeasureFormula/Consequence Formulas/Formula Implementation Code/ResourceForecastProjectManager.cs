using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.Common_Code;

namespace CustomerFormulaCode
{
	/// <summary>
	/// Returns a value in resource units for the resource code specified in CustomerConstants.cs
	/// This value is not converted to Zynos as it is expected that the dollar value will be captured through other forecast value measures.
	/// Intended primarily for use as a constraint or reporting metric.
	/// </summary>
	[Formula]
	public class ResourceForecastProjectManager : ResourceForecastProjectManagerBase
	{
		public override double?[] GetUnits(int startFiscalYear, int months,
		                                   TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
		{
			return ResourceSupply.GetSpendForResourceType(months, timeInvariantData.InvestmentSpendByResource, CoreConstants.PROJ_MGR);
		}
		
		public override double?[] GetZynos(int startFiscalYear, int months,
		                                   TimeInvariantInputDTO timeInvariantData,
		                                   IReadOnlyList<TimeVariantInputDTO> timeVariantData,
		                                   double?[] unitOutput)
		{
			return null;
		}
	}
}
