// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="ZKosior">
//   Copyright (C) Zbigniew Kosior. All rights reserved.
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ZKosior.LuckyMe
{
    /// <summary>
    ///     The program.
    /// </summary>
    internal class Program
    {
        #region Methods

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        private static void Main(string[] args)
        {
            // IoC could be used, but this is a small appliaction
            new ApplicationRunner(new Calculator()).Run(args);
        }

        #endregion
    }
}