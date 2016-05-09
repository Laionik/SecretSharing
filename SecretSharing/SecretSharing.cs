using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSharing
{
    class SecretSharing
    {
        /// <summary>
        /// Generate start values
        /// </summary>
        /// <param name="secret">Secret</param>
        /// <param name="partsNumber">Number of parts</param>
        /// <param name="p">Value of modulo</param>
        /// <param name="t">Parts required to recover a secret</param>
        /// <param name="aTab">Table of random values</param>
        private static void GenerateBasic(int secret, out int partsNumber, out int p, out int t, out int[] aTab)
        {
            Random rnd = new Random();
            partsNumber = rnd.Next(10, 20);
            p = rnd.Next(secret > partsNumber ? secret : partsNumber, secret > partsNumber ? secret * 10 : partsNumber * 10);
            t = rnd.Next(5, partsNumber);
            aTab = new int[t - 1];
            for (int i = 0; i < t - 1; i++)
            {
                aTab[i] = rnd.Next(100);
            }
        }

        /// <summary>
        /// Generate test start values
        /// </summary>
        /// <param name="secret">Secret</param>
        /// <param name="partsNumber">Number of parts</param>
        /// <param name="p">Value of modulo</param>
        /// <param name="t">Parts required to recover a secret</param>
        /// <param name="aTab">Table of random values</param>
        private static void GenerateBasicTest(int secret, out int partsNumber, out int p, out int t, out int[] aTab)
        {
            Random rnd = new Random();
            partsNumber = 4;
            p = 1523;
            t = 3;
            aTab = new int[2] { 352, 62 };
        }

        /// <summary>
        /// Sum of a elements multiplayed by x
        /// </summary>
        /// <param name="aTab">Table of random values</param>
        /// <param name="x">X</param>
        /// <returns>Sum</returns>
        public static double Sum(int[] aTab, int x)
        {
            double sum = 0;
            for(int i = 1; i <= aTab.Length; i ++)
                sum += aTab[i - 1] * Math.Pow(x, i);
            return sum;
        }

        /// <summary>
        /// Calculate parts
        /// </summary>
        /// <param name="secret">Secret</param>
        /// <param name="partsNumber">Number of parts</param>
        /// <param name="p">Value of modulo</param>
        /// <param name="t">Parts required to recover a secret</param>
        /// <param name="aTab">Table of random values</param>
        /// <returns>Table of parts</returns>
        public static double[] SecretPartsCalculate(int secret, int partsNumber, int p, int t, int[] aTab)
        {
            double[] secretTab = new double[partsNumber];
            for (int i = 1; i <= partsNumber; i++)
            {
                secretTab[i - 1] = (secret + Sum(aTab, i)) % p;
            }
            return secretTab;
        }

        /// <summary>
        /// Display values of secret parts
        /// </summary>
        /// <param name="secretTab">Table of secret parts</param>
        public static void DisplayParts(double[] secretTab)
        {
            for (int i = 1; i <= secretTab.Length; i++)
            {
                Console.WriteLine("({0}, {1})", i, secretTab[i - 1]);
            }
        }


        static void Main(string[] args)
        {
            try
            {
                //Console.WriteLine("What's your secret number?");
                //int secret = int.Parse(Console.ReadLine());
                int secret = 954;// int.Parse(Console.ReadLine());
                int partsNumber, p, t;
                int[] aTab;
                //GenerateBasic(secret, out partsNumber, out p, out t, out aTab);
                GenerateBasicTest(secret, out partsNumber, out p, out t, out aTab);
                double[] secretTab = SecretPartsCalculate(secret, partsNumber, p, t, aTab);
                DisplayParts(secretTab);

            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}
