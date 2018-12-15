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
    public class ResValAssetValue : ResValAssetValueBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
    		double InitialValue = 0;    		
    		double BookAge = 0;
    		
    		double?[] AgeEffective = timeInvariantData.ResidualValue_AssetEffectiveAge_ConsqUnitOutput;
    		
    		InitialValue = timeInvariantData.AssetValue;
    		BookAge = timeInvariantData.AssetBookAge;
    		
    		var InitialValueArray = PopulateOutputWithValue(months, InitialValue/CommonConstants.MonthsPerYear);
    		
    		var multiplier = -1.0 * InitialValue/BookAge;
    		
    		var InitialValueReduction = ArrayHelper.MultiplyStreamOfValuesByConstant(AgeEffective, multiplier);
    		
    		double?[] ResidualValue = new double?[months];
    		
    		for (int i = 0; i < months; i++)
		    {
    			if (AgeEffective[i] == null){
					ResidualValue[i] = null;
    			} else
			    {
    				ResidualValue[i] = InitialValueArray[i] + InitialValueReduction[i];   
    				if (ResidualValue[i] < 0)
				    {
    					ResidualValue[i] = 0;
    				}
    			}
    		}
			return ResidualValue;
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
