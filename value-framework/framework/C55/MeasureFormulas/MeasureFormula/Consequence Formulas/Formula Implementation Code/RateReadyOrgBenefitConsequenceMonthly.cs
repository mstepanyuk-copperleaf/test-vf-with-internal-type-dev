using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormula.Common_Code;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.SharedCode;

namespace CustomerFormulaCode
{
    [Formula]
    public class RateReadyOrgBenefitConsequenceMonthly : RateReadyOrgBenefitConsequenceMonthlyBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            // RR01 is either 0 or 1
            const double RateValueInPoints = 50;
            return InterpolatePropagate<TimeVariantInputDTO>(
                timeVariantData,
                startFiscalYear, 
                months, 
                x => (RateValueInPoints * x.RRO1.Value) / CommonConstants.MonthsPerYear);
        }
        
        public override double?[] GetZynos(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData,
            IReadOnlyList<TimeVariantInputDTO> timeVariantData,
            double?[] unitOutput)
        {
            return unitOutput;
        }
    }
}
