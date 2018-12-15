namespace MeasureFormula.Common_Code
{
    public class CoreConstants
    {
        /// <summary>
        /// Labour costs per hour used in multiple questionnaires
        /// </summary>
        public const double CAPEXLabourHour = 110d;

        public const double OPEXLabourHour = 110d;

        /// <summary>
        /// Customer Service Benefit Constants
        /// </summary>
        public const double AgentTimeCostPerHour = 110d;

        public const double SavingsPerLowEffortResolution = 20d;
        public const double SavingsPerSelfServiceResoultion = 20d;

        /// <summary>
        /// Environmental Benefits Constants
        /// </summary>
        public const double RenewableEnergyValuePerMW = 200d;

        /// <summary>
        /// Constants Used in Working Conditions Benefit
        /// </summary>
        public const double ValuePerCandidateAttracted = 10000d;

        public const double EmployeeCostPerYear = 100000d;
        public const double EmployeeCostToReplace = 200000d;
        public const double ProbabilityOfEmployeesLeaving = 0.1d;

        /// <summary>
        /// The value of making a minor brand image improvement for all customers
        /// Used in the public perception benefit questionnaire
        /// </summary>
        public const double PublicPerceptionValueForAllCustomers = 500000d;

        /// <summary>
        /// Constants Used in Employee Wellness benefit
        /// </summary>
        // This is the factor for the whole organization, so it will differ depending on the size of the organization.
        public const double EmployeeWellnessFactor = 500000d;

        public const string CustomerContributionAccount = "CIAC";

        /// <summary>
        /// Resource Codes
        /// </summary>
        public const string PROJ_MGR = "12410";

        public const string	ENG_DESIGN = "14000";

        /// <summary>
        /// Account Type questionnaire Dropdown Values
        /// </summary>
        public const int CapitalAcctID = 1;

        public const int OMAAcctID = 2;
        public const double MajorConsequenceValueUnits = 55000;  // Risk units

        /// <summary>
        /// NGN tonnes to kg's
        /// </summary>
        public const double TonnesToKgs = 1000d;

        public const int AssetStartDateMonthOffset = 4;

        /// <summary>
        /// Constanst brought over for MH value models
        /// </summary>
        public const double CustomerServiceCoefficient = 50000d;

        public const double CostToHireOrRetainEmployee = 100000d;
    }
    
    public enum FinancialBenefitType
    {
        CostAvoidance = 1,
        CostSavings = 2,
        RevenueIncrease = 3
    }
}