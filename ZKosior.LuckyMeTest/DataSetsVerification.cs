namespace ZKosior.LuckyMeTest
{
    using System;
    using System.IO;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rhino.Mocks;

    using ZKosior.LuckyMe;

    [TestClass]
    public class DataSetsVerification
    {
        #region Public Methods and Operators

        [TestMethod]
        public void CanHndleMultipleLines()
        {
            var mocks = new MockRepository();
            var calculator = mocks.PartialMock<Calculator>();
            var readerStream = new MemoryStream(Encoding.ASCII.GetBytes("10\n25\n39\n"));
            var result = new byte[10];
            var writerStream = new MemoryStream(result);
            using (mocks.Record())
            {
                Expect.Call(calculator.IsDivisibleBy13("10")).Return(false);
                Expect.Call(calculator.IsDivisibleBy13("25")).Return(false);
                Expect.Call(calculator.IsDivisibleBy13("39")).Return(true);
            }

            using (mocks.Playback())
            {
                calculator.VerifyData(readerStream, writerStream);
            }

            Assert.AreEqual("No\nNo\nYes\n", Encoding.ASCII.GetString(result));
        }

        [TestMethod]
        public void HandlesAndRethrowsExceptionWhenNegativeNumber()
        {
            var mocks = new MockRepository();
            var calculator = mocks.PartialMock<Calculator>();
            var readerStream = new MemoryStream(Encoding.ASCII.GetBytes("-1"));
            var result = new byte[3];
            var writerStream = new MemoryStream(result);
            using (mocks.Record())
            {
                Expect.Call(calculator.IsDivisibleBy13(null)).Throw(new OverflowException("exception"));
            }

            try
            {
                calculator.VerifyData(readerStream, writerStream);
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ApplicationException));
                Assert.AreEqual("Please verify line number: 1. Numbers can not be negative.", e.Message);
            }
        }

        [TestMethod]
        public void IgnoresEmptyLinesAtInTheMiddle()
        {
            var mocks = new MockRepository();
            var calculator = mocks.PartialMock<Calculator>();
            var readerStream = new MemoryStream(Encoding.ASCII.GetBytes("10\n\n25\n"));
            var result = new byte[6];
            var writerStream = new MemoryStream(result);
            using (mocks.Record())
            {
                Expect.Call(calculator.IsDivisibleBy13("10")).Return(false);
                Expect.Call(calculator.IsDivisibleBy13("25")).Return(false);
            }

            using (mocks.Playback())
            {
                calculator.VerifyData(readerStream, writerStream);
            }

            Assert.AreEqual("No\nNo\n", Encoding.ASCII.GetString(result));
        }

        [TestMethod]
        public void IgnoresEmptyLinesAtTheEnd()
        {
            var mocks = new MockRepository();
            var calculator = mocks.PartialMock<Calculator>();
            var readerStream = new MemoryStream(Encoding.ASCII.GetBytes("10\n25\n\n"));
            var result = new byte[6];
            var writerStream = new MemoryStream(result);
            using (mocks.Record())
            {
                Expect.Call(calculator.IsDivisibleBy13("10")).Return(false);
                Expect.Call(calculator.IsDivisibleBy13("25")).Return(false);
            }

            using (mocks.Playback())
            {
                calculator.VerifyData(readerStream, writerStream);
            }

            Assert.AreEqual("No\nNo\n", Encoding.ASCII.GetString(result));
        }

        [TestMethod]
        public void ReadsDataAndWritesResult()
        {
            var mocks = new MockRepository();
            var calculator = mocks.PartialMock<Calculator>();
            var readerStream = new MemoryStream(new[] { Convert.ToByte('1') });
            var result = new byte[3];
            var writerStream = new MemoryStream(result);
            using (mocks.Record())
            {
                Expect.Call(calculator.IsDivisibleBy13("1")).Return(false);
            }

            using (mocks.Playback())
            {
                calculator.VerifyData(readerStream, writerStream);
            }

            Assert.AreEqual("No\n", Encoding.ASCII.GetString(result));
        }

        [TestMethod]
        public void WhenInvalidCharacters_ThrowsExceptionGivesNumberOfFirstInvalidLine()
        {
            var mocks = new MockRepository();
            var calculator = mocks.PartialMock<Calculator>();
            var readerStream = new MemoryStream(Encoding.ASCII.GetBytes("a"));
            var result = new byte[3];
            var writerStream = new MemoryStream(result);
            using (mocks.Record())
            {
                Expect.Call(calculator.IsDivisibleBy13(null)).Throw(new FormatException("exception"));
            }

            try
            {
                calculator.VerifyData(readerStream, writerStream);
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ApplicationException));
                Assert.AreEqual("Please verify line number: 1", e.Message);
            }
        }

        [TestMethod]
        public void WhenThrowingExceptionAfterEmptyLine_GivesLineNumberOfTheFirstInvalidLine()
        {
            var mocks = new MockRepository();
            var calculator = mocks.PartialMock<Calculator>();
            var readerStream = new MemoryStream(Encoding.ASCII.GetBytes("10\n\na"));
            var result = new byte[3];
            var writerStream = new MemoryStream(result);
            using (mocks.Record())
            {
                Expect.Call(calculator.IsDivisibleBy13(null)).Throw(new FormatException("exception"));
            }

            try
            {
                calculator.VerifyData(readerStream, writerStream);
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ApplicationException));
                Assert.AreEqual("Please verify line number: 3", e.Message);
            }
        }

        #endregion
    }
}