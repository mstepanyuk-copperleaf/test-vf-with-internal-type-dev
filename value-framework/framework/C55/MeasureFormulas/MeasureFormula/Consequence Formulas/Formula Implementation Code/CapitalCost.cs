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
    /// Investment Capital costs in dollars.
    /// No likelihood formula expected to be used.
    /// Typically applied as a constraint.
    /// Uses the capital account code (CAPEXAccount) specified in CustomerConstants.cs
    /// </summary>
    [Formula]
	public class CapitalCost : CapitalCostBase
	{
		public override double?[] GetUnits(int startFiscalYear, int months, 
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
		{
			return GetSpendForAccountType(months, timeInvariantData.InvestmentSpendByAccountType, 
                CustomerConstants.CAPEXAccount );
		}
		
		public override double?[] GetZynos(int startFiscalYear, int months, TimeInvariantInputDTO timeInvariantData, 
            IReadOnlyList<TimeVariantInputDTO> timeVariantData, double?[] unitOutput)
		{
            return ConvertUnitsToZynos(unitOutput, CustomerConstants.DollarToZynoConversionFactor);
		}
	}
}

