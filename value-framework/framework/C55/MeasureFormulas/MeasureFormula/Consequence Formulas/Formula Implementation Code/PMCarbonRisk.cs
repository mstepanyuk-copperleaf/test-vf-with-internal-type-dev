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
    public class PMCarbonRisk : PMCarbonRiskBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
    		double?[] privateT = timeInvariantData.Mileage_CarbonSavingsPrivateTonnesC02e_ConsqUnitOutput;
    		var convertCarbon = timeInvariantData.SystemNonTradedPriceOfCarbonPerTonnesC02e;  		
    		
    		return ArrayHelper.MultiplyArrayByTimeSeries(privateT, convertCarbon, startFiscalYear);
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
