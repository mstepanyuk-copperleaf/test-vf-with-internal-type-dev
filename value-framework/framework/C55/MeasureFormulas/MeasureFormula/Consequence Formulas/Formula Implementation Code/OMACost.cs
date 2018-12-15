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
    /// Investment O&M costs in dollars.
    /// No likelihood formula expected to be used.
    /// Typically applied as a constraint.
    /// Uses the capital account code (OMAAccount) specified in CustomerConstants.cs
    /// </summary>
    [Formula]
	public class OMACost : OMACostBase
	{
		public override double?[] GetUnits(int startFiscalYear, int months, TimeInvariantInputDTO timeInvariantData, 
            IReadOnlyList<TimeVariantInputDTO> timeVariantData)
		{
			return GetSpendForAccountType(months, timeInvariantData.InvestmentSpendByAccountType,CustomerConstants.OMAAccount);
		}
		
		public override double?[] GetZynos(int startFiscalYear, int months, TimeInvariantInputDTO timeInvariantData,
            IReadOnlyList<TimeVariantInputDTO> timeVariantData, double?[] unitOutput)
		{
            return ConvertUnitsToZynos(unitOutput, CustomerConstants.DollarToZynoConversionFactor);
		}
	}
}
