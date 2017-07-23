// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Calculator.cs" company="ZKosior">
//   Copyright (C) Zbigniew Kosior. All rights reserved.
// </copyright>
// <summary>
//   Provides advanced calculation features.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ZKosior.LuckyMe
{
    using System;
    using System.Globalization;
    using System.IO;

    /// <summary>
    ///     Provides advanced calculation features.
    /// </summary>
    public class Calculator
    {
        #region Constants

        /// <summary>
        ///     The digits to read.
        /// </summary>
        /// <remarks>
        ///     Unsigned Integer 64 can store 20 digit decimal numbers.
        ///     First two digits of Unsigned Integer 64 maximal value are '18',
        ///     so not all 20 digit numbers can be represented in Unsigned Integer 64.
        ///     During calculations we prefix next part of verified number with modulo from previous calculation.
        ///     If we decide to divide by numbers 18 or less, then possible modulo outcomes are less then 18
        ///     and we can use all remaining 18 digits maximizing the number of digits verified in one step.
        ///     If we would like to divide by numbers higher than 18, we should use only 17 digits.
        /// </remarks>
        private const int DigitsToRead = 18;

        /// <summary>
        ///     The divisor.
        /// </summary>
        private const int Divisor = 13;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Determines whether the specified number is divisible by 13.
        /// </summary>
        /// <param name="number">
        /// String containing the number to be verified.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified number is divisible by 13; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// When number can not be parsed.
        /// </exception>
        /// <remarks>
        /// Numbers for verification can be very large, System.String type is used.
        /// </remarks>
        public virtual bool IsDivisibleBy13(string number)
        {
            // if number can be verified in one step, then we can use one more digit
            if (number.Length <= DigitsToRead + 1)
            {
                return (Convert.ToUInt64(number) % Divisor) == 0;
            }

            ulong modulo = 0;
            for (int i = 0; i < number.Length; i += DigitsToRead)
            {
                int charactersToRead = Math.Min(number.Length - i, DigitsToRead);
                modulo =
                    Convert.ToUInt64(
                        string.Format(
                            "{0}{1}", 
                            modulo != 0 ? modulo.ToString(CultureInfo.InvariantCulture) : string.Empty, 
                            number.Substring(i, charactersToRead))) % Divisor;

                // if not reading CHAR_AMMOUNT_TO_READ then it's at the end of line. we don't need to verify it
            }

            return modulo == 0;
        }

        /// <summary>
        /// Verifies numbers coming from input stream data.
        /// </summary>
        /// <param name="inputStream">
        /// The input stream.
        /// </param>
        /// <param name="outputStream">
        /// The output stream.
        /// </param>
        public virtual void VerifyData(Stream inputStream, Stream outputStream)
        {
            // If this would be part of a library I would recommend to verify streams
            var reader = new StreamReader(inputStream);
            var writer = new StreamWriter(outputStream);
            int lineNumber = 0;
            while (!reader.EndOfStream)
            {
                string nextNumber = reader.ReadLine();
                lineNumber++;
                if (string.IsNullOrWhiteSpace(nextNumber))
                {
                    continue;
                }

                try
                {
                    writer.Write("{0}\n", this.IsDivisibleBy13(nextNumber) ? "Yes" : "No");
                }
                catch (FormatException e)
                {
                    throw new ApplicationException(string.Format("Please verify line number: {0}", lineNumber), e);
                }
                catch (OverflowException e)
                {
                    throw new ApplicationException(
                        string.Format("Please verify line number: {0}. Numbers can not be negative.", lineNumber), e);
                }
            }

            writer.Flush();
        }

        #endregion
    }
}