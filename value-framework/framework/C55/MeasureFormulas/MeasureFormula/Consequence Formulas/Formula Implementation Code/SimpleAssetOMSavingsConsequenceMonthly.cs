using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.Common_Code;
using MeasureFormula.SharedCode;

namespace CustomerFormulaCode
{
    [Formula]
    public class SimpleAssetOMSavingsConsequenceMonthly : SimpleAssetOMSavingsConsequenceMonthlyBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
			var annualCost = new double?[months];

			if (timeInvariantData.ConditionScore_Condition_ConsqUnitOutput == null
			    || timeInvariantData.AssetTypeOMAAnnualCostMediumConditionUpperBound == null
			    || timeInvariantData.AssetTypeOMAAnnualCostHighConditionUpperBound == null
			    || timeInvariantData.AssetTypeOMAAnnualCostLow == null
			    || timeInvariantData.AssetTypeOMAAnnualCostMedium == null
			    || timeInvariantData.AssetTypeOMAAnnualCostHigh == null)
			{
				return null;
			}
		
			for (var i = 0; i < months; i++)
			{
				var condition = timeInvariantData.ConditionScore_Condition_ConsqUnitOutput[i];
				if (condition == null)
				{
					annualCost[i] = null;
				}
				else if (condition >= timeInvariantData.AssetTypeOMAAnnualCostMediumConditionUpperBound)
				{
   			        // Note that assets with Low maintenance costs are those which are in good shape. i.e. they have
                    // condition scores greater than the upper bound of Medium-OMA-cost assets.   			        
					annualCost[i] = timeInvariantData.AssetTypeOMAAnnualCostLow;
				}
				else if (condition >= timeInvariantData.AssetTypeOMAAnnualCostHighConditionUpperBound
					   && condition < timeInvariantData.AssetTypeOMAAnnualCostMediumConditionUpperBound)
				{
					annualCost[i] = timeInvariantData.AssetTypeOMAAnnualCostMedium;
				}
				else
				{
					annualCost[i] = timeInvariantData.AssetTypeOMAAnnualCostHigh;
				}
			}
			     
			// Divide by 12 because this is a monthly formula
            return HelperFunctions.ScaleValues (months, annualCost, 1d / CommonConstants.MonthsPerYear);
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
