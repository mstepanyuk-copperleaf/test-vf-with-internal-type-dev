using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using MeasureFormula.Common_Code;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.SharedCode;

namespace CustomerFormulaCode
{
    [Formula]
    public class SimpleAssetReplacementCostLegacyMonthly : SimpleAssetReplacementCostLegacyMonthlyBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
    		// TODO: System Burden Factor
            if (timeInvariantData.AssetTypeAnnualSpendProfileCurve == null || timeInvariantData.AssetReplacementCost == null) return null;

            //Find the number of year using spend curve 
            int numYearsOfSpend = -1;

            foreach (var pt in timeInvariantData.AssetTypeAnnualSpendProfileCurve.Points)
            {
                numYearsOfSpend = Math.Max(numYearsOfSpend, (int)pt.X);
            }

            if (numYearsOfSpend <= 0) return null;

            //calculate replacement cost to be used below
            var replacementCost = LoadedReplacementCost(
                timeInvariantData.AssetReplacementCost,
                1.0, // timeInvariantData.SystemBurdenFactor ?? 1.0,
                timeInvariantData.AssetTypeCostVariationFactor);
            
            double?[] ret = new double?[months];

            if (timeInvariantData.ConditionScore_Condition_ConsqUnitOutput == null) return null;
            
            //Find where the condition is 10 and that is the impact date
            var assetReplacementOffsets = timeInvariantData.ConditionScore_Condition_ConsqUnitOutput.Select((val, index) => val >= (double) CustomerConstants.BestConditionScore? index: (int?) null).Where( x => x != null).ToList();

            foreach (var assetReplacementOffset in assetReplacementOffsets)
            {
                foreach (var dataPoint in timeInvariantData.AssetTypeAnnualSpendProfileCurve.Points)
                {
                    var spendYear = dataPoint.X;
                    var proportion = dataPoint.Y;
                    var monthlySpend = proportion * replacementCost / CommonConstants.MonthsPerYearInt;
                    
                    // The assumption is that the last year of spend occurs in the 12 months following the impact.  
                    //if (assetReplacementOffset == null) continue;
                    var startMonthForYearlySpend =  (assetReplacementOffset ?? 0) - (numYearsOfSpend - spendYear) * CommonConstants.MonthsPerYearInt;
                    var lastMonthForYearlySpend = Math.Min(months, startMonthForYearlySpend + CommonConstants.MonthsPerYearInt);
                    for (int j = (int) Math.Max(0, startMonthForYearlySpend); j < lastMonthForYearlySpend; j++)
                    {
                        ret[j] = monthlySpend + (ret[j] ?? 0d);
                    }
                }
            }
            return ret;
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
