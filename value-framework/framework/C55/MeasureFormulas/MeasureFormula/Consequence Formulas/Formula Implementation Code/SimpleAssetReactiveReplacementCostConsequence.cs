using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormula.Common_Code;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class SimpleAssetReactiveReplacementCostConsequence : SimpleAssetReactiveReplacementCostConsequenceBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
    		// aka Avoided Future Asset Replacement
            if (timeInvariantData.AssetReplacementCost == null)
            {
                return null;
            }
            
            var loadedReplacementCost = (double)timeInvariantData.AssetReplacementCost * 
                (1 + (timeInvariantData.AssetTypeExtraDamageFactor ?? 0));

            return PopulateOutputWithValue(months, loadedReplacementCost);
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
