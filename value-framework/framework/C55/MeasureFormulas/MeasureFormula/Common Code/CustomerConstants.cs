using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasureFormula.Common_Code
{
	public static class CustomerConstants
	{
		/// <summary>
		/// Conversion factors for switching between dollars and zynos
		/// </summary>
		public const double ZynoToDollarConversionFactor = 1000d;
		public const double DollarToZynoConversionFactor = 1d / ZynoToDollarConversionFactor;

		/// <summary>
		/// Condition score range -match these to the condition range configured using C55 system parameters
		/// </summary>
		public const int WorstConditionScore = 0;
		public const int BestConditionScore = 10;

		/// <summary>
		/// Account Types
		/// </summary>
		public const string CAPEXAccount = "Capital";
		public const string OMAAccount = "OMA";

		/// <summary>
		/// Constants brought over for TVA value models
		/// </summary>
		public const string RiskLevelHigh = "High";
	}
}