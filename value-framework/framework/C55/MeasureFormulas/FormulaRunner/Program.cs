using CL.FormulaHelper;

namespace FormulaRunner
{
    /// <summary>
    /// Launch the formula execution command line interface. The command line interface
    /// allows zip files exported from C55 Alternative Value page (by selecting the
    /// debug button) to be executed in the standalone solution.
    /// </summary>
    class Program
    {
        static void Main()
        {
            FormulaZipUtil.LaunchFormulaExecutionCommandLineInterface();
        }
    }
}
