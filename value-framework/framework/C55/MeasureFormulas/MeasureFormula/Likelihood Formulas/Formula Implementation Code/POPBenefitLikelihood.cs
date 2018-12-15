using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormula.SharedCode;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class POPBenefitLikelihood : POPBenefitLikelihoodBase
    {
        public override double?[] GetLikelihoodValues(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            return InterpolatePropagate<TimeVariantInputDTO>(timeVariantData, startFiscalYear, months,
                x => x.PopBenefitProbability / CommonConstants.PercentPerProbabilityOne / CommonConstants.MonthsPerYear);
        }
    }
}
