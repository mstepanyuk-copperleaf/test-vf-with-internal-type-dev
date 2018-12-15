using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.SharedCode;

namespace CustomerFormulaCode
{
    [Formula]
    public class BenefitLikelihood : BenefitLikelihoodBase
    {
        public override double?[] GetLikelihoodValues(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            return InterpolatePropagate<TimeVariantInputDTO>(timeVariantData,
                startFiscalYear,
                months,
                x => x.BenefitProbability / CommonConstants.PercentPerProbabilityOne / CommonConstants.MonthsPerYear);
        }
    }
}
