using NUnit.Framework;
using Rekrutacja.Helpers.ProstyKalkulatorStaticHelper;
using System;

namespace Rekrutacja.Tests.Helpers
{
    [TestFixture]
    public class ProstyKalkulatorStaticHelperTests
    {
        [TestCase(2, 3, '+', 5)]
        [TestCase(5, 2, '-', 3)]
        [TestCase(4, 3, '*', 12)]
        [TestCase(10, 2, '/', 5)]
        public void WykonajOperacje_ValidOperations_ReturnsExpectedResult(int a, int b, char operacja, int expected)
        {
            var result = ProstyKalkulatorStaticHelper.WykonajOperacje(a, b, operacja);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void WykonajOperacje_DivideByZero_ThrowsDivideByZeroException()
        {
            Assert.Throws<DivideByZeroException>(() =>
                ProstyKalkulatorStaticHelper.WykonajOperacje(5, 0, '/'));
        }

        [Test]
        public void WykonajOperacje_InvalidOperation_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
                ProstyKalkulatorStaticHelper.WykonajOperacje(1, 2, '%'));
        }
    }
}