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
	/// <summary>
	/// Project Execution Risk questionnaire formula calculates the risk of executing an investment (negative value) based on it's cost and various key success factors.
	/// Returns Annual Consequence values.  Expected to be used in conjunction with a likelihood formula that returns monthly values
	/// </summary>
	[Formula]
	public class ProjectExecutionRiskConsequenceMonthly : ProjectExecutionRiskConsequenceMonthlyBase
	{
		public override double?[] GetUnits(int startFiscalYear, int months,
		                                   TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
		{
			var spendValues = GetSpendForAccountType(months, timeInvariantData.InvestmentSpendByAccountType, CustomerConstants.CAPEXAccount);
			
			if (spendValues == null) return null;
			
			if (timeInvariantData.HowWellDefinedWillProjectScopeBe == null
			    && timeInvariantData.LiningUpRightImplementationPartners == null
			    && timeInvariantData.TeamsRightInternalSkillset == null
			    && timeInvariantData.SkillsToManageTheProject == null
			    && timeInvariantData.TypeOfContract == null
			    && timeInvariantData.ProvenImplementationMethodology == null
			    && timeInvariantData.AreInitialEffortAndScheduleRealistic == null
			    && timeInvariantData.VendorIndustryExperience == null
			    && timeInvariantData.VendorTechnicalExperience == null) return null;
			
			double?[] result = new double?[months];
			for (int offset = 0; offset < months; offset++)
			{
				if (spendValues[offset] != null)
				{
					//Planning (Min = 3.3%; Max = 8.3%)
					double? planningFactor = (HelperFunctions.GetCustomFieldValue(timeInvariantData.HowWellDefinedWillProjectScopeBe) ?? 0)
					                         + (HelperFunctions.GetCustomFieldValue(timeInvariantData.LiningUpRightImplementationPartners) ?? 0)
					                         + (HelperFunctions.GetCustomFieldValue(timeInvariantData.TeamsRightInternalSkillset) ?? 0);
						
					//Project Management (Min = 3.4%; Max = 8.4%)
					double? projectManagementFactor = (HelperFunctions.GetCustomFieldValue(timeInvariantData.SkillsToManageTheProject) ?? 0)
					                                  + (HelperFunctions.GetCustomFieldValue(timeInvariantData.TypeOfContract) ?? 0)
					                                  + (HelperFunctions.GetCustomFieldValue(timeInvariantData.ProvenImplementationMethodology) ?? 0)
					                                  + (HelperFunctions.GetCustomFieldValue(timeInvariantData.AreInitialEffortAndScheduleRealistic) ?? 0);
					
					//Vendor Technical Capability (Min = 3.3%; Max = 8.3%)
					double? vendorFactor = (HelperFunctions.GetCustomFieldValue(timeInvariantData.VendorIndustryExperience) ?? 0)
					                       + (HelperFunctions.GetCustomFieldValue(timeInvariantData.VendorTechnicalExperience) ?? 0);
					
					//Total = 25%
					result[offset] = spendValues[offset] * (planningFactor + projectManagementFactor + vendorFactor);
				}
			}
			return result;
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
