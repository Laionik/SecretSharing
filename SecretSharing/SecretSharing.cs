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

        /// <summary>
        /// Parting alghoritm
        /// </summary>
        /// <returns>Table of secret parts</returns>
        public static double[] SecretParting(out int t, out int p)
        {
            // Parting
            //Console.WriteLine("What's your secret number?");
            //int secret = int.Parse(Console.ReadLine());
            int secret = 954;
            int partsNumber;
            int[] aTab;
            //GenerateBasic(secret, out partsNumber, out p, out t, out aTab);
            GenerateBasicTest(secret, out partsNumber, out p, out t, out aTab);
            double[] secretTab = SecretPartsCalculate(secret, partsNumber, p, t, aTab);
            DisplayParts(secretTab);
            return secretTab;
        }
        /// <summary>
        /// Factor calculating
        /// </summary>
        /// <returns>Free word of factor</returns>
        public static double FactorCalculating(int index, double[] partsTab)
        {
            //tu ma być X nie Y
            double denominator = index == 0 ? partsTab[1] : partsTab[0];
            double counter = index == 0 ? partsTab[0] - partsTab[1] : partsTab[index] - partsTab[0];
            for (int i = index == 0 ? 1 : 0; i < partsTab.Length; i++)
            {
                if (i != index)
                {
                    denominator *= partsTab[i];
                    counter *= partsTab[index] - partsTab[i];
                }
            }
            double test = denominator / counter;
            return denominator / counter;
        }

        /// <summary>
        /// Combaining alghoritm
        /// </summary>
        /// <param name="secretTab">Table of secret parts</param>
        /// <param name="t">Number of required parts</param>
        /// <returns>Secret</returns>
        public static double SecretCombaining(double[] secretTab, int t, int p)
        {
            double secret = 0;
            double[] partsTab = secretTab.Take(t).ToArray();
            for(int i = 0; i < t; i++)
            {
                secret += FactorCalculating(i, partsTab) % p;
            }

            Console.WriteLine("Secret: {0}", secret);
            return secret;
        }


        static void Main(string[] args)
        {
            try
            {
                // Parting
                int t, p;
                double[] secretTab = SecretParting(out t, out p);

                // Combaining
                double secret = SecretCombaining(secretTab, t, p);
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
