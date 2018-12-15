using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class SimpleAssetFinancialConsequence : SimpleAssetFinancialConsequenceBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            double? consequence = null;

            if (timeInvariantData.AssetFinancialRiskConsequence != null)
            {
                consequence = timeInvariantData.AssetFinancialRiskConsequence.AvgValue;
            }
            else if (timeInvariantData.FailureSeverity != null)
            {
                consequence = timeInvariantData.FailureSeverity.AvgValue;
            }
            else
            {
                return null;
            }

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
