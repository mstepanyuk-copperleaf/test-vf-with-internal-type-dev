using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class EnergySavingsConsequence : EnergySavingsConsequenceBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            return InterpolatePropagate<TimeVariantInputDTO>(timeVariantData, startFiscalYear, months,
                x => x.PowerSavings);
        }
        
        public override double?[] GetZynos(int startFiscalYear, int months, TimeInvariantInputDTO timeInvariantData, 
            IReadOnlyList<TimeVariantInputDTO> timeVariantData, double?[] unitOutput)
        {
            return null; //no conversion to Zynos
        }
    }
}
