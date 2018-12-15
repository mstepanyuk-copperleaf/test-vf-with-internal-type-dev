using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.Common_Code;
using MeasureFormula.SharedCode;

namespace CustomerFormulaCode
{
	[Formula]
	public class ProductWorkplaceBenBenefitConsequence : ProductWorkplaceBenBenefitConsequenceBase
	{
		public override double?[] GetUnits(int startFiscalYear, int months,
		                                   TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
		{
			return InterpolatePropagate<TimeVariantInputDTO>(timeVariantData,
				startFiscalYear,
				months,
				x => (x.NumberOfCandidatesAttracted
				     * HelperFunctions.GetCustomFieldValue(x.WorkplaceImpactOnAttractiveness)
				     * CoreConstants.ValuePerCandidateAttracted)
				     + (x.NumberOfEmployeesAffected
				     * HelperFunctions.GetCustomFieldValue(x.WorkplaceImpactOnProductivity)
				     * CoreConstants.EmployeeCostPerYear)
				     + (x.NumberOfEmployeesAtRiskOfLeaving
				     * HelperFunctions.GetCustomFieldValue(x.WorkplaceImpactOnProductivity)
				     * CoreConstants.EmployeeCostToReplace
				     * CoreConstants.ProbabilityOfEmployeesLeaving));
		}

		public override double?[] GetZynos(int startFiscalYear, int months, TimeInvariantInputDTO timeInvariantData,
		                                   IReadOnlyList<TimeVariantInputDTO> timeVariantData, double?[] unitOutput)
		{
            return ConvertUnitsToZynos(unitOutput, CustomerConstants.DollarToZynoConversionFactor);
		}
	}
}
