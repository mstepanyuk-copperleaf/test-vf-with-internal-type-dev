using System;
using System.Collections.Generic;
using CL.FormulaHelper.DTOs;

namespace MeasureFormula.TestHelpers
{
    public class PointEqualityComparer : IEqualityComparer<CurvePointDTO>
    {
        public const double DoubleComparisonTolerance = 0.0001;
        private readonly double PointTolerance;
        public PointEqualityComparer(double pointTolerance)
        {
            PointTolerance = pointTolerance;
        }
        public bool Equals(CurvePointDTO lhs, CurvePointDTO rhs)
        {
            if (lhs == null && rhs == null) return true;

            if (lhs == null || rhs == null) return false;
            return (Math.Abs(lhs.X - rhs.X) < DoubleComparisonTolerance && Math.Abs(lhs.Y - rhs.Y) < PointTolerance);
        }

        public int GetHashCode(CurvePointDTO obj)
        {
            throw new NotImplementedException();
        }
    }
}