using System;
using System.Reflection;
using AutoFixture.Kernel;
using CL.FormulaHelper.DTOs;

namespace MeasureFormula.TestHelpers
{
    public class TimePeriodExtractor : ISpecimenBuilder
    {
        private int Index;
        private readonly TimePeriodDTO[] TimePeriodDto;

        public TimePeriodExtractor(TimePeriodDTO[] timePeriodDto)
        {
            if (timePeriodDto != null)
            {
                TimePeriodDto = timePeriodDto;
                Index = 0;
            }
            else throw new ArgumentNullException("timePeriodDto");
        }

        public object Create(object request, ISpecimenContext context)
        {
            var parameterInfo = request as ParameterInfo;
            if (parameterInfo == null)
            {
                return new NoSpecimen();
            }

            if (parameterInfo.ParameterType != typeof(TimePeriodDTO)
                || parameterInfo.Name != "p_TimePeriod")
            {
                return new NoSpecimen();
            }

            if (Index >= TimePeriodDto.Length) throw new Exception("The number of elements in TimePeriodDTO must be the "
                                                                   + "same as the number of periods generated for "
                                                                   + "TimeVariantDTO");
            return TimePeriodDto[Index++];
        }
    }
}