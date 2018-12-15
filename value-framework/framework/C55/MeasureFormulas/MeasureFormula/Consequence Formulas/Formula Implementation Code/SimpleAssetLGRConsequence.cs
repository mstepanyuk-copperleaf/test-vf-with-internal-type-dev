using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormula.Common_Code;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class SimpleAssetLGRConsequence : SimpleAssetLGRConsequenceBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            if ( !(timeInvariantData.AssetContributesToLostGeneration  ?? false) )
            {
                return null;
            }

            var dollarConsequence = GenerationRiskConsequenceInCurrencyUnits(
                        startFiscalYear,
                        months,
                        timeInvariantData.AssetGenerationGroup,
                        timeInvariantData.AssetIsSpareAvailable,
                        timeInvariantData.AssetTypeDowntimeWeeksWithSpare,
                        timeInvariantData.AssetTypeDowntimeWeeksWithoutSpare,
                        timeInvariantData.StrategyAlternative);
            return dollarConsequence;
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
