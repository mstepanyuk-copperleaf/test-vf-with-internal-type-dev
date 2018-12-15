using System;

namespace MeasureFormula.TestHelpers
{
    public static class HelperMethods
    {
        [Obsolete]
        public static void MultiplyIntProperty<T>(T dto, string propertyName, int multiple)
        {
            var property = dto.GetType().GetProperty(propertyName);
            if (property == null)
            {
                var message = "Could not get " + propertyName + " property for type: " + dto.GetType().FullName;
                throw new InvalidOperationException(message);
            }

            var newValue = (int)property.GetValue(dto) * multiple;

            property.SetValue(dto, newValue);
        }

        [Obsolete]
        public static void MultiplyDoubleProperty<T>(T dto, string propertyName, double multiple)
        {
            var property = dto.GetType().GetProperty(propertyName);
            if (property == null)
            {
                string message = "Could not get " + propertyName + " property for type: " + dto.GetType().FullName;
                throw new InvalidOperationException(message);
            }

            var newValue = (double)property.GetValue(dto) * multiple;

            property.SetValue(dto, newValue);
        }
    }
}
