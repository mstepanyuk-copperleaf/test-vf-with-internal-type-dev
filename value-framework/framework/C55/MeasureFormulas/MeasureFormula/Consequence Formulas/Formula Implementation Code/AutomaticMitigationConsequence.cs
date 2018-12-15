using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class AutomaticMitigationConsequence : AutomaticMitigationConsequenceBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            var result = new double?[months];

            // return 0 values after the end of spend
            FillWithValueAfterEndOfSpend(months, timeInvariantData.InvestmentSpendByAccountType, ref result, 0);

            return result;
        }
        
        public override double?[] GetZynos(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData,
            IReadOnlyList<TimeVariantInputDTO> timeVariantData,
            double?[] unitOutput)
        {
    	    // MJ Copied from Zeus V15

    	    return unitOutput;
        }
    }
}
