using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.Common_Code;

namespace CustomerFormulaCode
{
    [Formula]
    public class EmployeeProductivityBenefitConsequence : EmployeeProductivityBenefitConsequenceBase
    {
	    public override double?[] GetUnits(int startFiscalYear, int months,
	                                       TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
	    {
		    return InterpolatePropagate<TimeVariantInputDTO>(timeVariantData, startFiscalYear, months, 
			    x => CalculateMonthlyConsequence(timeInvariantData, x));
	    }

	    private static double? CalculateMonthlyConsequence(TimeInvariantInputDTO timeInvariantData, TimeVariantInputDTO timeVariantData)
	    {
		    var fieldEmployeeCostsSaved = timeVariantData.NoFieldEmployees
		                                       * timeVariantData.FieldHoursSaved
		                                       * timeInvariantData.SystemFieldEmployeeCostPerHour;
		    var officeEmployeeCostsSaved = timeVariantData.NoOfficeEmployees
		                                  * timeVariantData.OfficeHoursSaved
		                                  * timeInvariantData.SystemOfficeEmployeeCostPerHour;
		    var managerCostsSaved = timeVariantData.NoManagerEmployees
		                                          * timeVariantData.ManagerHoursSaved
		                                          * timeInvariantData.SystemManagerCostPerHour;
		    var fieldEmployeeAdditionalCosts = timeVariantData.NoFieldEmployees
		                                            * timeVariantData.FieldHoursAdditional
		                                            * timeInvariantData.SystemFieldEmployeeCostPerHour;
		    var officeEmployeeAdditionalCosts = timeVariantData.NoOfficeEmployees
		                                       * timeVariantData.OfficeHoursAdditional
		                                       * timeInvariantData.SystemOfficeEmployeeCostPerHour;
		    var managerAdditionalCosts = timeVariantData.NoManagerEmployees
		                                               * timeVariantData.ManagerHoursAdditional
		                                               * timeInvariantData.SystemManagerCostPerHour;
		    return (fieldEmployeeCostsSaved + officeEmployeeCostsSaved + managerCostsSaved) 
		           * timeInvariantData.SystemProbabilityOfRepurposing
		           - (fieldEmployeeAdditionalCosts + officeEmployeeAdditionalCosts + managerAdditionalCosts);
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
