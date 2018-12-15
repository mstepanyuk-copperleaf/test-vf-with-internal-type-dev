using NUnit.Framework;

namespace MeasureFormula.UITests.TestAttributes
{
    public class CategorySmoke : CategoryAttribute
    {
        public CategorySmoke() : base("Smoke") { }
    }
}
