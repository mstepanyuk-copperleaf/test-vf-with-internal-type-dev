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
    public class CMCarbonRisk : CMCarbonRiskBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            double?[] CompanyT= timeInvariantData.Mileage_CarbonSavingsCompanyTonnesC02e_ConsqUnitOutput;
    		var ConvertCarbon = timeInvariantData.SystemNonTradedPriceOfCarbonPerTonnesC02e;
    		
    		return ArrayHelper.MultiplyArrayByTimeSeries(CompanyT, ConvertCarbon, startFiscalYear);
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
