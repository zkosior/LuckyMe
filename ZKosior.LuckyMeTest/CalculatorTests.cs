namespace ZKosior.LuckyMeTest
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ZKosior.LuckyMe;

    [TestClass]
    public class CalculatorTests
    {
        #region Public Methods and Operators

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void CharactersOtherThanDigitsAreNotSupported()
        {
            new Calculator().IsDivisibleBy13("a");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void FloatNumbersAreNotSupported()
        {
            new Calculator().IsDivisibleBy13("13.0");
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void NegativeNumbersAreNotSupported()
        {
            new Calculator().IsDivisibleBy13("-1");
        }

        [TestMethod]
        public void NumbersLargerThanUInt64AndDivisibleBy13AreTrue()
        {
            Assert.IsTrue(new Calculator().IsDivisibleBy13("13000000000000000000000000000000000000000000"));
        }

        [TestMethod]
        public void NumbersLargerThanUInt64AndNotDivisibleBy13AreFalse()
        {
            Assert.IsFalse(new Calculator().IsDivisibleBy13("13000000000000000000000000000000000000000001"));
        }

        [TestMethod]
        public void NumbersWith19OrLessDigitsAndDivisibleBy13AreTrue()
        {
            Assert.IsTrue(new Calculator().IsDivisibleBy13(13000000000000000000.ToString()));
        }

        [TestMethod]
        public void NumbersWith19OrLessDigitsAndNotDivisibleBy13AreFalse()
        {
            Assert.IsFalse(new Calculator().IsDivisibleBy13(13000000000000000001.ToString()));
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void WhiteCharactersAreNotSupported()
        {
            new Calculator().IsDivisibleBy13(" ");
        }

        #endregion
    }
}