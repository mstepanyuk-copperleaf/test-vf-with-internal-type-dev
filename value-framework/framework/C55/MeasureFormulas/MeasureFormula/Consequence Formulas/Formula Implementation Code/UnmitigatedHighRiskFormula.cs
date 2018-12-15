using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormula.SharedCode;
using MeasureFormula.Common_Code;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class UnmitigatedHighRiskFormula : UnmitigatedHighRiskFormulaBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            var results = new double?[months];

            //calculate the outcome risk value
            var monthlyRiskValues = InterpolatePropagate<TimeVariantInputDTO>(timeVariantData,
                startFiscalYear, months, x => x.RiskConsequence.AvgValue * x.RiskProbability.AvgValue / CommonConstants.MonthsPerYear);

            //find the high risk level dto
            var highestRiskLevel = timeInvariantData.RiskMatrixRiskLevel.Dict.Where(x => x.Value.Code == CustomerConstants.RiskLevelHigh)
                .Select(r => r.Value).FirstOrDefault();

            //need both a valid risk level and risk values
            if (highestRiskLevel == null || monthlyRiskValues == null)
            {
                return null;
            }

            // grab the monthly risk level threshold because we'll be looking at monthly values
            var monthlyHighestRiskLevelThreshold = highestRiskLevel.MinValue / CommonConstants.MonthsPerYear;
            
            for (var i = 0; i < months; i++)
            {
                //if the risk value falls in the red zone in the risk matrix for the current month, set a 1
                if (monthlyRiskValues[i] >= monthlyHighestRiskLevelThreshold)
                {
                    results[i] = 1;
                }
            }

            return results;
        }
        
        public override double?[] GetZynos(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData,
            IReadOnlyList<TimeVariantInputDTO> timeVariantData,
            double?[] unitOutput)
        {
            // no valid zynos conversion for this value measure
            return null;
        }
    }
}
