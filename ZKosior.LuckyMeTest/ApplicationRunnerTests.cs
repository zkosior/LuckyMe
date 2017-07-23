namespace ZKosior.LuckyMeTest
{
    using System;
    using System.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rhino.Mocks;

    using ZKosior.LuckyMe;

    [TestClass]
    public class ApplicationRunnerTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void ValidatesCorrectInputAndOutputParametersAndVerifiesData()
        {
            var mocks = new MockRepository();
            var calculator = mocks.PartialMock<Calculator>();
            var applicationRunner = mocks.PartialMock<ApplicationRunner>(calculator);
            var fileInfo = mocks.StrictMock<FileInfo>();
            using (mocks.Record())
            {
                Expect.Call(applicationRunner.CreateFileInfo("inputfile.txt")).Return(fileInfo);
                Expect.Call(applicationRunner.CreateFileInfo("outputfile.txt")).Return(fileInfo);
                Expect.Call(fileInfo.Exists).Return(true);
                Expect.Call(applicationRunner.GetInputStream()).Return(null);
                Expect.Call(applicationRunner.GetOutputStream()).Return(null);
                calculator.VerifyData(null, null);
                LastCall.IgnoreArguments();
                applicationRunner.WriteToConsole("Calculation finished");
            }

            using (mocks.Playback())
            {
                applicationRunner.Run(new[] { "inputfile.txt", "outputfile.txt" });
            }
        }

        [TestMethod]
        public void ValidatesCorrectInputParameterAndVerifiesData()
        {
            var mocks = new MockRepository();
            var calculator = mocks.StrictMock<Calculator>();
            var fileInfo = mocks.StrictMock<FileInfo>();
            var applicationRunner = mocks.StrictMock<ApplicationRunner>(calculator);
            using (mocks.Record())
            {
                Expect.Call(applicationRunner.CreateFileInfo("inputfile.txt")).Return(fileInfo);
                Expect.Call(applicationRunner.CreateFileInfo("inputfile_output.txt")).Return(fileInfo);
                Expect.Call(fileInfo.Exists).Return(true);
                Expect.Call(applicationRunner.GetInputStream()).Return(null);
                Expect.Call(applicationRunner.GetOutputStream()).Return(null);
                calculator.VerifyData(null, null);
                LastCall.IgnoreArguments();
                applicationRunner.WriteToConsole("Calculation finished");
            }

            using (mocks.Playback())
            {
                applicationRunner.Run(new[] { "inputfile.txt" });
            }
        }

        [TestMethod]
        public void WhenExceptionOccures_WritesMessageToConsole()
        {
            var mocks = new MockRepository();
            var calculator = mocks.StrictMock<Calculator>();
            var applicationRunner = mocks.StrictMock<ApplicationRunner>(calculator);
            using (mocks.Record())
            {
                applicationRunner.WriteToConsole(null);
                LastCall.IgnoreArguments();
                LastCall.Throw(new Exception("Exception Message"));
                applicationRunner.WriteToConsole("Exception Message");
            }

            using (mocks.Playback())
            {
                applicationRunner.Run(new string[] { });
            }
        }

        [TestMethod]
        public void WhenIncorrectNumberOfParameters_ReturnsFalseAndWritesOutputToConsole()
        {
            var mocks = new MockRepository();
            var calculator = mocks.StrictMock<Calculator>();
            var applicationRunner = mocks.StrictMock<ApplicationRunner>(calculator);
            using (mocks.Record())
            {
                applicationRunner.WriteToConsole("Wrong number of params specified");
                applicationRunner.WriteToConsole("Exemplary usage:");
                applicationRunner.WriteToConsole(@"LuckyMe Data\Test01_input.txt");
                applicationRunner.WriteToConsole(@"LuckyMe Data\Test01_input.txt Data\Test01_output.txt");
            }

            using (mocks.Playback())
            {
                applicationRunner.Run(new string[] { });
            }
        }

        [TestMethod]
        public void WhenInputFileDoesNotExist_ReturnsFalseAndWritesOutputToConsole()
        {
            var mocks = new MockRepository();
            var calculator = mocks.StrictMock<Calculator>();
            var fileInfo = mocks.StrictMock<FileInfo>();
            var applicationRunner = mocks.StrictMock<ApplicationRunner>(calculator);
            using (mocks.Record())
            {
                Expect.Call(applicationRunner.CreateFileInfo("inputfile.txt")).Return(fileInfo);
                Expect.Call(applicationRunner.CreateFileInfo("inputfile_output.txt")).Return(fileInfo);
                Expect.Call(fileInfo.Exists).Return(false); // file does not exist
                applicationRunner.WriteToConsole("Input file does not exist");
            }

            using (mocks.Playback())
            {
                applicationRunner.Run(new[] { "inputfile.txt" });
            }
        }

        #endregion
    }
}