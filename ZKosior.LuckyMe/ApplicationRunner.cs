// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationRunner.cs" company="ZKosior">
//   Copyright (C) Zbigniew Kosior. All rights reserved.
// </copyright>
// <summary>
//   The application runner.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ZKosior.LuckyMe
{
    using System;
    using System.IO;

    /// <summary>
    ///     The application runner.
    /// </summary>
    public class ApplicationRunner
    {
        #region Fields

        /// <summary>
        ///     This field is here only to show that build script runs StyleCop rules verification.
        /// </summary>
        private int unusedField;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRunner"/> class.
        /// </summary>
        /// <param name="calculator">
        /// The calculator.
        /// </param>
        public ApplicationRunner(Calculator calculator)
        {
            // property injection would be better (i.e. MEF), but here it's only for testability purposes
            // TypeMock could also be used making it easier to test
            this.Calculator = calculator;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the calculator.
        /// </summary>
        private Calculator Calculator { get; set; }

        /// <summary>
        ///     Gets or sets the input file.
        /// </summary>
        private FileInfo InputFile { get; set; }

        /// <summary>
        ///     Gets or sets the output file.
        /// </summary>
        private FileInfo OutputFile { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The create file info.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <returns>
        /// The <see cref="FileInfo"/>.
        /// </returns>
        public virtual FileInfo CreateFileInfo(string fileName)
        {
            return new FileInfo(fileName);
        }

        /// <summary>
        ///     The get input stream.
        /// </summary>
        /// <returns>
        ///     The <see cref="Stream" />.
        /// </returns>
        public virtual Stream GetInputStream()
        {
            return this.InputFile.OpenRead();
        }

        /// <summary>
        ///     The get output stream.
        /// </summary>
        /// <returns>
        ///     The <see cref="Stream" />.
        /// </returns>
        public virtual Stream GetOutputStream()
        {
            return this.OutputFile.OpenWrite();
        }

        /// <summary>
        /// Entry point for application logic.
        /// </summary>
        /// <param name="args">
        /// Command line arguments.
        /// </param>
        public void Run(string[] args)
        {
            try
            {
                if (this.ValidateParameters(args))
                {
                    this.Calculator.VerifyData(this.GetInputStream(), this.GetOutputStream());
                    this.WriteToConsole("Calculation finished");
                }
            }
            catch (Exception exception)
            {
                // not all exception can be caught here and not all should be, 
                // but right here I just want to gracefully exit application
                this.WriteToConsole(exception.Message);
            }
        }

        /// <summary>
        /// The write to console.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        public virtual void WriteToConsole(string text)
        {
            Console.WriteLine(text);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The validate parameters.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool ValidateParameters(string[] args)
        {
            if (args.Length < 1)
            {
                this.WriteToConsole("Wrong number of params specified");
                this.WriteToConsole("Exemplary usage:");
                this.WriteToConsole(@"LuckyMe Data\Test01_input.txt");
                this.WriteToConsole(@"LuckyMe Data\Test01_input.txt Data\Test01_output.txt");
                return false;
            }

            this.InputFile = this.CreateFileInfo(args[0]);
            this.OutputFile = this.CreateFileInfo(args.Length > 1 ? args[1] : args[0].Replace(".txt", "_output.txt"));
            if (!this.InputFile.Exists)
            {
                // There still might be some problems with access rights to files, but i didn't want to go too deep
                this.WriteToConsole("Input file does not exist");
                return false;
            }

            return true;
        }

        #endregion
    }
}