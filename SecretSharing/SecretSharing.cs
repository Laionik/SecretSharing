using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSharing
{
    class SecretSharing
    {
        private static Random rnd;
        #region test
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
            partsNumber = 4;
            p = 1523;
            t = 3;
            aTab = new int[2] { 352, 62 };
        }
        #endregion

        #region parting

        private static int GetPrimeNumber(int maxValue)
        {
            int[] primeNumbers = new int[16] { 379, 1697, 2711, 3769, 5881, 6421, 7757, 8563, 9601, 10151, 12107, 14173, 15233, 18233, 19991, 28837 };
            List<int> primeNumbersGT = primeNumbers.Where(x => x > maxValue).ToList();
            return primeNumbersGT[rnd.Next(primeNumbersGT.Count)];
        }

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
            partsNumber = rnd.Next(10, 20);
            p = GetPrimeNumber(Math.Max(secret, partsNumber));
            t = rnd.Next(5, partsNumber);
            aTab = new int[t - 1];
            for (int i = 0; i < t - 1; i++)
            {
                aTab[i] = rnd.Next(1000);
            }
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
            for (int i = 1; i <= aTab.Length; i++)
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
        public static double[] SecretParting(out int t, out int p, out int partsNumber)
        {
            // Parting
            Console.WriteLine("What's your secret number?");
            int secret = int.Parse(Console.ReadLine());
            int[] aTab;
            GenerateBasic(secret, out partsNumber, out p, out t, out aTab);
            // GenerateBasicTest(secret, out partsNumber, out p, out t, out aTab); //Test value
            double[] secretTab = SecretPartsCalculate(secret, partsNumber, p, t, aTab);
            DisplayParts(secretTab);
            return secretTab;
        }
        #endregion

        #region combaining
        /// <summary>
        /// Factor calculating
        /// </summary>
        /// <returns>Free word of factor</returns>
        public static double FactorCalculating(int index, List<int> indexTab)
        {
            double denominator = 1;
            double counter = 1;
            int i = 0;
            foreach (int indexValue in indexTab)
            {
                if (i != index)
                {
                    denominator *= (-1) * indexValue;
                    counter *= indexTab[index] - indexValue;
                }
                i++;
            }
            return denominator / counter;
        }

        /// <summary>
        /// Generate list of random indexes without duplicates
        /// </summary>
        /// <param name="t">Number of required parts</param>
        /// <param name="partsNumber">Number of parts</param>
        /// <returns>List of random indexes without duplicates</returns>
        public static List<int> GenerateRandomIndexList(int t, int partsNumber)
        {
            List<int> allNumbers = new List<int>();
            List<int> partsIndex = new List<int>();
            int maxNumber = partsNumber;

            for (int i = 1; i <= partsNumber; i++)
            {
                allNumbers.Add(i);
            }

            for (int i = 0; i < t; i++)
            {
                int rndNumber = rnd.Next(maxNumber);
                partsIndex.Add(allNumbers[rndNumber]);
                allNumbers.RemoveAt(rndNumber);
                maxNumber--;
            }
            return partsIndex;
        }

        /// <summary>
        /// Combaining alghoritm
        /// </summary>
        /// <param name="secretTab">Table of secret parts</param>
        /// <param name="t">Number of required parts</param>
        /// <returns>Secret</returns>
        public static double SecretCombaining(double[] secretTab, int t, int p, int partsNumber)
        {
            double secret = 0;
            List<int> partsIndex = GenerateRandomIndexList(t, partsNumber);
            List<double> partsTab = new List<double>();
            foreach (int number in partsIndex)
                partsTab.Add(secretTab[number - 1]);

            for (int i = 0; i < t; i++)
            {
                secret += partsTab[i] * FactorCalculating(i, partsIndex) % p;
            }

            Console.WriteLine("Secret: {0}", secret);
            return secret;
        }
        #endregion

        static void Main(string[] args)
        {
            try
            {
                rnd = new Random();
                // Parting
                int t, p, partsNumber;
                double[] secretTab = SecretParting(out t, out p, out partsNumber);

                // Combaining
                double secret = SecretCombaining(secretTab, t, p, partsNumber);
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
