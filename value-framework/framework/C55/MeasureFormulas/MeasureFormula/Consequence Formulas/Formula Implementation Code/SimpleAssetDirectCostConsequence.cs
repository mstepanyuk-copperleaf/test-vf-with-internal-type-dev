using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class SimpleAssetDirectCostConsequence : SimpleAssetDirectCostConsequenceBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            if (timeInvariantData.AssetReplacementCost == null)
            {
                return null;
            }
            
    		// TODO - System Burden
            var replacementCost = LoadedReplacementCost(
                timeInvariantData.AssetReplacementCost,
                1.0, // timeInvariantData.SystemBurdenFactor ?? 1.0,
                timeInvariantData.AssetTypeCostVariationFactor);

            if (timeInvariantData.AssetTypeExtraDamageFactor.HasValue)
            {
                replacementCost *= (1.0 + timeInvariantData.AssetTypeExtraDamageFactor.Value);
            }

            return PopulateOutputWithValue(months, replacementCost);
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
