using System;

namespace MeasureFormula.UITests.TestData
{
    public static class TestDataGenerator
    {
        private static readonly Random Rng = new Random();

        private const string AlphabeticChars = "abcdefghijklmnopqrstuvwxyz";
        private const string CapitalChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string NumericChars = "1234567890";

        /// <summary>
        /// A class to store methods to generate custom strings 
        /// for passwords, emails, etc
        /// </summary>
        public static string GetRandomString(int size, string charsToUse = AlphabeticChars)
        {
            var stringChars = new char[size];
            for (int i = 0; i < size; i++)
            {
                stringChars[i] = charsToUse[Rng.Next(charsToUse.Length)];
            }
            return new string(stringChars);
        }

        public static string GetRandomCapitalChars(int size, string charsToUse = CapitalChars)
        {
            return GetRandomString(size, charsToUse);
        }

        public static string GetRandomNumericChars(int size, string charsToUse = NumericChars)
        {
            return GetRandomString(size, charsToUse);
        }
    }

}
