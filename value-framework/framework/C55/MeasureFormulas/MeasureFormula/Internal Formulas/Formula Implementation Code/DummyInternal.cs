using System;
using System.Collections.Generic;
using CL.FormulaHelper;
using CL.FormulaHelper.Attributes;
using MeasureFormula.Common_Code;

namespace CustomerFormulaCode
{
    /**
     * This class exists so that we can test if Parsitron is able to parse and pickup internal output types.
     */
    [Formula]
    public class DummyInternal : DummyInternalBase {}

}
