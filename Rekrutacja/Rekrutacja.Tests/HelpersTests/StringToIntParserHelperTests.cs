using NUnit.Framework;
using Rekrutacja.Helpers.StringToIntParserHelper;
using System;

namespace Rekrutacja.Tests.Helpers
{
    [TestFixture]
    public class StringToIntParserHelperTests
    {
        /// <summary>
        /// Prawidłowe liczby całkowite dodatnie w formie stringa wraz z przypadkami specyficznymi jak "0007"
        /// </summary>
        [TestCase("0", 0)]
        [TestCase("5", 5)]
        [TestCase("123", 123)]
        [TestCase("5564", 5564)]
        [TestCase("0007", 7)]
        public void ParsePositiveNumberFromStringToInt_ValidInput_ReturnsExpectedInt(string input, int expected)
        {
            var result = StringToIntParserHelper.ParsePositiveNumberFromStringToInt(input);
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// NULL zaiast obiektu string, oczekiwany wyjątek
        /// </summary>
        [TestCase("")]
        [TestCase(null)]
        public void ParsePositiveNumberFromStringToInt_EmptyOrNull_ThrowsException(string input)
        {
            Assert.Throws<Exception>(() => StringToIntParserHelper.ParsePositiveNumberFromStringToInt(input));
        }

        /// <summary>
        /// Nieprawidłowe znaki (niebędące cyfra - tzn. poza kodem ASCII 48-57) w stringu, oczekiwany wyjątek
        /// </summary>
        [TestCase("12a3")]
        [TestCase(" 123")]
        [TestCase("1.23")]
        [TestCase("-123")]
        [TestCase("12 3")]
        public void ParsePositiveNumberFromStringToInt_InvalidCharacters_ThrowsException(string input)
        {
            Assert.Throws<Exception>(() => StringToIntParserHelper.ParsePositiveNumberFromStringToInt(input));
        }
    }
}