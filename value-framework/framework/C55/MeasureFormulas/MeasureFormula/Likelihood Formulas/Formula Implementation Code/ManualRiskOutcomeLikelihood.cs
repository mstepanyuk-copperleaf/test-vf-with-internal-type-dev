using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class ManualRiskOutcomeLikelihood : ManualRiskOutcomeLikelihoodBase
    {
        public override double?[] GetLikelihoodValues(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            // MJ: copied from Zeus v15, AutomaticMitigationProbabilityFormula

            var result = new double?[months];

            // return 0 values after the end of spend
            FillWithValueAfterEndOfSpend(months, timeInvariantData.InvestmentSpendByAccountType, ref result, 0);

            return result;

        }
    }
}
