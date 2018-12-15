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
    /// Total Investment costs in dollars.
    /// No likelihood formula expected to be used.
    /// </summary>
    [Formula]
    public class TotalCost : TotalCostBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months, 
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            return GetSpendForAllAccountTypes(months, timeInvariantData.InvestmentSpendByAccountType);
        }
        
        public override double?[] GetZynos(int startFiscalYear, int months, TimeInvariantInputDTO timeInvariantData,
            IReadOnlyList<TimeVariantInputDTO> timeVariantData, double?[] unitOutput)
        {
            return ConvertUnitsToZynos(unitOutput, CustomerConstants.DollarToZynoConversionFactor);
        }
    }
}
