using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.Common_Code;
using MeasureFormula.SharedCode;

namespace CustomerFormulaCode
{
    [Formula]
    public class EmissionLevelsConsequence : EmissionLevelsConsequenceBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
                                           TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            var co2Prices = timeInvariantData.SystemCO2CreditsValueDollarsPerMWhTimeSeries;
            var tonsCO2Avoided = InterpolatePropagate<TimeVariantInputDTO>(timeVariantData,
                                                                          startFiscalYear,
                                                                          months, (x => x.CO2ReductionTV));
            if (co2Prices == null || tonsCO2Avoided == null)
            {
                return null;
            }

            var result = CommonHelperFunctions.scaleValues(startFiscalYear, months, tonsCO2Avoided, co2Prices);
            return result;
        }
        
        public override double?[] GetZynos(int startFiscalYear, int months,
                                           TimeInvariantInputDTO timeInvariantData,
                                           IReadOnlyList<TimeVariantInputDTO> timeVariantData,
                                           double?[] unitOutput)
        {
            return ConvertUnitsToZynos(unitOutput, CustomerConstants.DollarToZynoConversionFactor);
        }
    }
}
