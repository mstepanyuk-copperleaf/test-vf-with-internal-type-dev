using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class SimpleAssetEnvironmentalConsequence : SimpleAssetEnvironmentalConsequenceBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            double? envConsequence = null;

            if (timeInvariantData.AssetEnvironmentalRiskConsequence != null)
            {
                envConsequence = timeInvariantData.AssetEnvironmentalRiskConsequence.AvgValue;
            }
            else if (timeInvariantData.FailureSeverity != null)
            {
                envConsequence = timeInvariantData.FailureSeverity.AvgValue;
            }
            else
            {
                return null;
            }
    		return PopulateOutputWithValue(months, envConsequence);
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
