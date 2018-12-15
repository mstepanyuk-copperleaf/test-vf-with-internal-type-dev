using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.Common_Code;

namespace CustomerFormulaCode
{
    [Formula]
    public class ProductWorkplaceBenBenefitConsequenceFormula : ProductWorkplaceBenBenefitConsequenceFormulaBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            return InterpolatePropagate<TimeVariantInputDTO>(timeVariantData,
                startFiscalYear, 
                months, x => (x.NoEmployeesAtRiskOfLeaving * 
                              CoreConstants.CostToHireOrRetainEmployee 
                              +
                              x.NoEmployeesAffected * 
                              CoreConstants.CostToHireOrRetainEmployee * 
                              x.WorkplaceImpactOnProductivity.Value) / 1000);
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
