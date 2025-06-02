using NUnit.Framework;
using Rekrutacja.Helpers.ProstyKalkulatorStaticHelper;
using System;

namespace Rekrutacja.Tests.Helpers
{
    [TestFixture]
    public class ProstyKalkulatorStaticHelperTests
    {
        /// <summary>
        /// Przypadki prawidłowe dla każdej z operacji
        /// </summary>
        [TestCase(2, 3, '+', 5)] // 2 + 3 = 5
        [TestCase(5, 2, '-', 3)] // 5 - 2 = 3
        [TestCase(4, 3, '*', 12)] // 4 * 3 = 12
        [TestCase(10, 2, '/', 5)] // 10 / 2 = 5
        [TestCase(10, 3, '/', 3)] // 10 / 3 = 3,3333..., wynik zaokrąglony do 3, ponieważ jest to najbliższa liczba całkowita
        public void WykonajOperacje_ValidOperations_ReturnsExpectedResult(int a, int b, char operacja, int expected)
        {
            var result = ProstyKalkulatorStaticHelper.WykonajOperacje(a, b, operacja);
            Assert.AreEqual(expected, result);
        }
        
        /// <summary>
        /// Dzielenie przez zero, oczekiwany wyjątek
        /// </summary>
        [Test]
        public void WykonajOperacje_DivideByZero_ThrowsDivideByZeroException()
        {
            Assert.Throws<DivideByZeroException>(() =>
                ProstyKalkulatorStaticHelper.WykonajOperacje(5, 0, '/'));
        }


        /// <summary>
        /// Nieprawidłowa operacja (znak inny niż +, -, *, /), oczekiwany wyjątek
        /// </summary>
        [Test]
        public void WykonajOperacje_InvalidOperation_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
                ProstyKalkulatorStaticHelper.WykonajOperacje(1, 2, '%'));
        }
    }
}