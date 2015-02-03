// **************************************************************************************************
// **  
// **  Yahoo! Finance Managed
// **  Written by Marius Häusler 2015
// **  It would be pleasant, if you could contact me when you are using this code.
// **  Contact: maasone@live.com
// **  Project Home: https://yahoofinance.codeplex.com/
// **  
// **************************************************************************************************
// **  
// **  Copyright @ Marius Häusler
// **  
// **  Licensed under GNU Lesser General Public License (LGPL) (Version 2.1, February 1999).
// **  
// **  License Text: https://yahoofinance.codeplex.com/license
// **  
// **  
// **************************************************************************************************
using System;
using System.Collections.Generic;
using System.Text;


namespace MaasOne.YahooFinance
{
    /// <summary>
    /// Class for managing an International Securities Identification Number.
    /// </summary>
    /// <remarks></remarks>
    public class ISIN
    {
        private char[] mChars = new char[11];
        private int mCheckDigit = -1;

        /// <summary>
        /// The complete ISIN
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [System.Xml.Serialization.XmlText()]
        public string Value { get { return new string(mChars) + this.CheckDigit.ToString(); } }

        /// <summary>
        /// The country specific part of the ISIN
        /// </summary>
        /// <value></value>
        /// <returns>The first two letters if the ISIN is valid</returns>
        /// <remarks></remarks>
        [System.Xml.Serialization.XmlIgnore()]
        public string CountryCode
        {
            get { return mChars[0].ToString() + mChars[1].ToString(); }
        }

        /// <summary>
        /// The core part of the ISIN
        /// </summary>
        /// <value></value>
        /// <returns>The ISIN without CountryCode and CheckDigit</returns>
        /// <remarks></remarks>
        [System.Xml.Serialization.XmlIgnore()]
        public string CoreCode
        {
            get { return mChars[2].ToString() + mChars[3].ToString() + mChars[4].ToString() + mChars[5].ToString() + mChars[6].ToString() + mChars[7].ToString() + mChars[8].ToString() + mChars[9].ToString() + mChars[10].ToString(); }
        }

        /// <summary>
        /// The calculated check digit of the ISIN
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Every "get" call a calculation</remarks>
        [System.Xml.Serialization.XmlIgnore()]
        public int CheckDigit
        {
            get { return mCheckDigit; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="isinWithOrWithoutCheckDigit">A valid ISIN with or without check digit</param>
        /// <remarks>If check digit exists, it will be ignored, but in this case the string must be a digit to be a valid ISIN. The check digit will be calculated seperatly.</remarks>
        public ISIN(string isinWithOrWithoutCheckDigit)
        {
            if (!IsValidFormat(isinWithOrWithoutCheckDigit))
            {
                throw new ArgumentException("The ISIN value is not valid formatted.", "isinWithOrWithoutCheckDigit");
            }
            else
            {
                for (int i = 0; i <= 10; i++)
                {
                    mChars[i] = char.ToUpper(isinWithOrWithoutCheckDigit[i]);
                }

                List<int> checkDigits = new List<int>();
                foreach (char c in mChars)
                {
                    int d = this.CharToISINDigit(c);
                    checkDigits.AddRange(this.ToSingleDigits(d));
                }
                if (checkDigits.Count > 0)
                {
                    int quersumme = 0;
                    int count = 1;
                    for (int i = checkDigits.Count - 1; i >= 0; i += -1)
                    {
                        count += 1;
                        if (count % 2 == 0)
                        {
                            checkDigits[i] *= 2;
                        }
                        int[] digits = this.ToSingleDigits(checkDigits[i]);
                        foreach (int d in digits)
                        {
                            quersumme += d;
                        }
                    }
                    mCheckDigit = (10 - (quersumme % 10)) % 10;
                }
                else
                {
                    mCheckDigit = -1;
                }
            }
        }

        /// <summary>
        /// Returns the complete ISIN value.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return this.Value;
        }

        private int[] ToSingleDigits(int i)
        {
            if (i < 10)
            {
                return new int[] { i };
            }
            else
            {
                int[] db = new int[2];
                db[0] = Convert.ToInt32(i.ToString()[0].ToString());
                db[1] = Convert.ToInt32(i.ToString()[1].ToString());
                return db;
            }
        }
        private int CharToISINDigit(char c)
        {
            if (char.IsDigit(c))
            {
                return Convert.ToInt32(c.ToString());
            }
            else
            {
                int digit = 0;
                for (ISINDigits i = ISINDigits.A; i <= ISINDigits.Z; i++)
                {
                    if (c.ToString().ToUpper() == i.ToString())
                    {
                        digit = (int)i;
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
                return digit;
            }
        }


        /// <summary>
        /// Checks a string for valid ISIN format
        /// </summary>
        /// <param name="isin">An ISIN with or without check digit</param>
        /// <returns>True if ISIN is valid formatted</returns>
        /// <remarks>If check digit exists, it will be ignored, but in this case the string must be a digit to be a valid ISIN. The check digit will be calculated seperatly.</remarks>
        public static bool IsValidFormat(string isin)
        {
            if (isin.Length == 11 | isin.Length == 12)
            {
                if (char.IsLetter(isin[0]) & char.IsLetter(isin[1]))
                {
                    for (int i = 2; i <= 10; i++)
                    {
                        if (!char.IsLetterOrDigit(isin[i]))
                            return false;
                    }
                    return !(isin.Length == 12 && !char.IsDigit(isin[11]));
                }
            }
            return false;
        }


        private enum ISINDigits
        {
            A = 10,
            B = 11,
            C = 12,
            D = 13,
            E = 14,
            F = 15,
            G = 16,
            H = 17,
            I = 18,
            J = 19,
            K = 20,
            L = 21,
            M = 22,
            N = 23,
            O = 24,
            P = 25,
            Q = 26,
            R = 27,
            S = 28,
            T = 29,
            U = 30,
            V = 31,
            W = 32,
            X = 33,
            Y = 34,
            Z = 35
        }

    }
}
