using System.Configuration;

namespace MeasureFormula.UITests.TestData
{
    public static class LoginData
    {
        public static readonly Account CopperleafAccount = new Account(
            ConfigurationManager.AppSettings["copperleaf_username"],
            ConfigurationManager.AppSettings["copperleaf_password"]);
    }
}
