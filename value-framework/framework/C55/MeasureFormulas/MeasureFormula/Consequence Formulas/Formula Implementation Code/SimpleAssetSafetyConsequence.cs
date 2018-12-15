using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class SimpleAssetSafetyConsequence : SimpleAssetSafetyConsequenceBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            if (timeInvariantData.AssetSafetyRiskConsequence == null
                && timeInvariantData.FailureSeverity == null) return null;

            double? consequence = timeInvariantData.AssetSafetyRiskConsequence != null
                                      ? timeInvariantData.AssetSafetyRiskConsequence.AvgValue
                                      : timeInvariantData.FailureSeverity.AvgValue;

      		return PopulateOutputWithValue(months, consequence);
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
