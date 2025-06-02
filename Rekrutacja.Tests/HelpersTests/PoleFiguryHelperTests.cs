using NUnit.Framework;
using Rekrutacja.Helpers.PoleFiguryHelper;
using Rekrutacja.Enums;
using System;

namespace Rekrutacja.Tests.Helpers
{
    [TestFixture]
    public class PoleFiguryHelperTests
    {
        [TestCase(4, 0, FiguraEnum.Kwadrat, 16)] // 4*4
        [TestCase(3, 5, FiguraEnum.Prostokat, 15)] // 3*5
        [TestCase(6, 3, FiguraEnum.Trojkat, 1)] // (6/3)/2 = 1, zaokr¹glone
        [TestCase(2, 0, FiguraEnum.Kolo, 13)] // PI*2^2 = 12.566..., zaokr¹glone do 13
        public void ObliczPoleFigury_ValidFigures_ReturnsExpectedResult(int a, int b, FiguraEnum figura, int expected)
        {
            var result = PoleFiguryHelper.ObliczPoleFigury(a, b, figura);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ObliczPoleFigury_InvalidFigure_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
                PoleFiguryHelper.ObliczPoleFigury(1, 2, (FiguraEnum)999));
        }
    }
}