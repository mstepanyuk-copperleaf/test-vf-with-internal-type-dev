using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.SharedCode;

namespace CustomerFormulaCode
{
    [Formula]
    public class ProjectProbabilityFormula : ProjectProbabilityFormulaBase
    {
        public override double?[] GetLikelihoodValues(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            double projProbability = timeInvariantData.BenefitProbabilityNTV/CommonConstants.MonthsPerYear/CommonConstants.PercentPerProbabilityOne;
                      
            return PopulateOutputWithValue(months, projProbability);
        }
    }
}
