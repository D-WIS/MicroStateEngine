using DWIS.API.DTO;
using DWIS.MicroState.Model;
using Microsoft.CodeAnalysis;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DWIS.MicroState.UnitTests
{
    public class CalibrateSignalsTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            int l = 600;
            double[] data = new double[l];
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            for (int i = 0; i < l; i++)
            {
                double x = 0;
                for (int j = 0; j < amplitudes.Length; j++)
                {
                    x += amplitudes[j] * Math.Sin(i * tStep * 2 * Math.PI * (j + 1) / period);
                }
                data[i] = x;
            }
            List<double> guesses = new List<double>();
            guesses.Add(1);
            guesses.Add(0);
            guesses.Add(0);
            double calibrationTimeWindowFactor = 0.5;
            int m = (int)(calibrationTimeWindowFactor * l); // width of window
            int k_min = l / 2 - m / 2; // first index in the window if not delays

            double chiSquare = FuseAndCalibrateSignals.ChiSquare(guesses.ToArray(), data, 1, l, m, k_min);
            Assert.AreEqual(0, chiSquare, 1e-6);
        }
        [Test]
        public void Test2()
        {
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            int[] incrs = new int[] { 0, 0 };
            double[] data = new double[n*l];
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < l; i++)
                {
                    double x = 0;
                    for (int j = 0; j < amplitudes.Length; j++)
                    {
                        x += amplitudes[j] * Math.Sin((i + incrs[k]) * tStep * 2 * Math.PI * (j + 1) / period);
                    }
                    data[k * l + i] = x;
                }
            }
            List<double> guesses = new List<double>();
            guesses.Add(1);
            guesses.Add(0);
            guesses.Add(0);

            double calibrationTimeWindowFactor = 0.5;
            int m = (int)(calibrationTimeWindowFactor * l); // width of window
            int k_min = l / 2 - m / 2; // first index in the window if not delays
            double[] guess = guesses.ToArray();
            double chiSquare = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            Assert.AreEqual(0, chiSquare, 1e-6);
            double[] results = new double[(n-1) * 3];
            FuseAndCalibrateSignals.ChiSquarePartialDerivatives(guess, data, results, n, l, m, k_min);
            double epsilon = 1e-6;
            guess[0] += epsilon;
            double chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[0] -= 2*epsilon;
            double chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[0] += epsilon;
            double res0 = (chiSquare2 - chiSquare1) / (2 * epsilon);
            guess[1] += epsilon;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[1] -= 2 * epsilon;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[1] += epsilon;
            double res1 = (chiSquare2 - chiSquare1) / (2 * epsilon);
            guess[1] -= epsilon;
            guess[2] += 1;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[2] -= 2;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[2] += 1;
            double res2 = (chiSquare2 - chiSquare1) / 2;
            guess[2] -= 1;
            Assert.AreEqual(res0, results[0], 1e-6);
            Assert.AreEqual(res1, results[1], 1e-6);
            Assert.AreEqual(res2, results[2], 1e-6);
        }

        [Test]
        public void Test3()
        {
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            int[] incrs = new int[] { 0, 1 };
            double[] data = new double[n * l];
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < l; i++)
                {
                    double x = 0;
                    for (int j = 0; j < amplitudes.Length; j++)
                    {
                        x += amplitudes[j] * Math.Sin((i + incrs[k]) * tStep * 2 * Math.PI * (j + 1) / period);
                    }
                    data[k * l + i] = x;
                }
            }
            List<double> guesses = new List<double>();
            guesses.Add(1);
            guesses.Add(0);
            guesses.Add(0);

            double calibrationTimeWindowFactor = 0.5;
            int m = (int)(calibrationTimeWindowFactor * l); // width of window
            int k_min = l / 2 - m / 2; // first index in the window if not delays
            double[] guess = guesses.ToArray();
            double chiSquare = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            Assert.AreEqual(37.760131895004157, chiSquare, 1e-6);
            double[] results = new double[(n - 1) * 3];
            FuseAndCalibrateSignals.ChiSquarePartialDerivatives(guesses.ToArray(), data, results, n, l, m, k_min);
            double epsilon = 1e-6;
            guess[0] += epsilon;
            double chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[0] -= 2 * epsilon;
            double chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[0] += epsilon;
            double res0 = (chiSquare2 - chiSquare1) / (2 * epsilon);
            guess[1] += epsilon;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[1] -= 2 * epsilon;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[1] += epsilon;
            double res1 = (chiSquare2 - chiSquare1) / (2 * epsilon);
            guess[1] -= epsilon;
            guess[2] += 1;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[2] -= 2;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[2] += 1;
            double res2 = (chiSquare2 - chiSquare1) / 2;
            guess[2] -= 1;
            Assert.AreEqual(res0, results[0], 1e-6);
            Assert.AreEqual(res1, results[1], 1e-6);
            Assert.AreEqual(res2, results[2], 1e-6);
        }
        [Test]
        public void Test4()
        {
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            int[] incrs = new int[] { 0, -1 };
            double[] data = new double[n * l];
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < l; i++)
                {
                    double x = 0;
                    for (int j = 0; j < amplitudes.Length; j++)
                    {
                        x += amplitudes[j] * Math.Sin((i + incrs[k]) * tStep * 2 * Math.PI * (j + 1) / period);
                    }
                    data[k * l + i] = x;
                }
            }
            List<double> guesses = new List<double>();
            guesses.Add(1);
            guesses.Add(0);
            guesses.Add(0);

            double calibrationTimeWindowFactor = 0.5;
            int m = (int)(calibrationTimeWindowFactor * l); // width of window
            int k_min = l / 2 - m / 2; // first index in the window if not delays
            double[] guess = guesses.ToArray();
            double chiSquare = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            Assert.AreEqual(37.76013189500415, chiSquare, 1e-6);
            double[] results = new double[(n - 1) * 3];
            FuseAndCalibrateSignals.ChiSquarePartialDerivatives(guess, data, results, n, l, m, k_min);
            double epsilon = 1e-6;
            guess[0] += epsilon;
            double chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[0] -= 2 * epsilon;
            double chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[0] += epsilon;
            double res0 = (chiSquare2 - chiSquare1) / (2 * epsilon);
            guess[1] += epsilon;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[1] -= 2 * epsilon;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[1] += epsilon;
            double res1 = (chiSquare2 - chiSquare1) / (2 * epsilon);
            guess[1] -= epsilon;
            guess[2] += 1;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[2] -= 2;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[2] += 1;
            double res2 = (chiSquare2 - chiSquare1) / 2;
            guess[2] -= 1;
            Assert.AreEqual(res0, results[0], 1e-6);
            Assert.AreEqual(res1, results[1], 1e-6);
            Assert.AreEqual(res2, results[2], 1e-6);
        }
        [Test]
        public void Test5()
        {
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            int[] incrs = new int[] { 0, 5 };
            double[] data = new double[n * l];
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < l; i++)
                {
                    double x = 0;
                    for (int j = 0; j < amplitudes.Length; j++)
                    {
                        x += amplitudes[j] * Math.Sin((i + incrs[k]) * tStep * 2 * Math.PI * (j + 1) / period);
                    }
                    data[k * l + i] = x;
                }
            }
            List<double> guesses = new List<double>();
            guesses.Add(1);
            guesses.Add(0);
            guesses.Add(0);

            double calibrationTimeWindowFactor = 0.5;
            int m = (int)(calibrationTimeWindowFactor * l); // width of window
            int k_min = l / 2 - m / 2; // first index in the window if not delays
            double[] guess = guesses.ToArray();
            double chiSquare = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            Assert.AreEqual(728.15585582390293, chiSquare, 1e-6);
            double[] results = new double[(n - 1) * 3];
            FuseAndCalibrateSignals.ChiSquarePartialDerivatives(guess, data, results, n, l, m, k_min);
            double epsilon = 1e-6;
            guess[0] += epsilon;
            double chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[0] -= 2 * epsilon;
            double chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[0] += epsilon;
            double res0 = (chiSquare2 - chiSquare1) / (2 * epsilon);
            guess[1] += epsilon;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[1] -= 2 * epsilon;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[1] += epsilon;
            double res1 = (chiSquare2 - chiSquare1) / (2 * epsilon);
            guess[1] -= epsilon;
            guess[2] += 1;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[2] -= 2;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[2] += 1;
            double res2 = (chiSquare2 - chiSquare1) / 2;
            guess[2] -= 1;
            Assert.AreEqual(res0, results[0], 1e-6);
            Assert.AreEqual(res1, results[1], 1e-6);
            Assert.AreEqual(res2, results[2], 1e-6);
        }

        [Test]
        public void Test6()
        {
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            double[] alphas = new double[] { 2, 1 };
            double[] betas = new double[] { 0, 0 };
            int[] incrs = new int[] { 0, 0 };
            double[] data = new double[n * l];
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < l; i++)
                {
                    double x = 0;
                    for (int j = 0; j < amplitudes.Length; j++)
                    {
                        x += amplitudes[j] * Math.Sin((i + incrs[k]) * tStep * 2 * Math.PI * (j + 1) / period);
                    }
                    data[k * l + i] = alphas[k]*x + betas[k];
                }
            }
            double calibrationTimeWindowFactor = 0.5;
            int m = (int)(calibrationTimeWindowFactor * l); // width of window
            int k_min = l / 2 - m / 2; // first index in the window if not delays

            List<double> guesses = new List<double>();
            guesses.Add(alphas[0]);
            guesses.Add(betas[0]);
            guesses.Add(incrs[0]);

            double[] guess = guesses.ToArray();
            double chiSquare = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            Assert.AreEqual(0, chiSquare, 1e-6);
            double[] results = new double[(n - 1) * 3];
            FuseAndCalibrateSignals.ChiSquarePartialDerivatives(guess, data, results, n, l, m, k_min);
            double epsilon = 1e-6;
            guess[0] += epsilon;
            double chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[0] -= 2 * epsilon;
            double chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[0] += epsilon;
            double res0 = (chiSquare2 - chiSquare1) / (2 * epsilon);
            guess[1] += epsilon;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[1] -= 2 * epsilon;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[1] += epsilon;
            double res1 = (chiSquare2 - chiSquare1) / (2 * epsilon);
            guess[1] -= epsilon;
            guess[2] += 1;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[2] -= 2;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[2] += 1;
            double res2 = (chiSquare2 - chiSquare1) / 2;
            guess[2] -= 1;
            Assert.AreEqual(res0, results[0], 1e-6);
            Assert.AreEqual(res1, results[1], 1e-6);
            Assert.AreEqual(res2, results[2], 1e-6);

            guesses = new List<double>();
            guesses.Add(1);
            guesses.Add(0);
            guesses.Add(0);

            guess = guesses.ToArray();
            chiSquare = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            Assert.AreEqual(563.9999999999992, chiSquare, 1e-6);
            results = new double[(n - 1) * 3];
            FuseAndCalibrateSignals.ChiSquarePartialDerivatives(guess, data, results, n, l, m, k_min);
            epsilon = 1e-6;
            guess[0] += epsilon;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[0] -= 2 * epsilon;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[0] += epsilon;
            res0 = (chiSquare2 - chiSquare1) / (2 * epsilon);
            guess[1] += epsilon;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[1] -= 2 * epsilon;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[1] += epsilon;
            res1 = (chiSquare2 - chiSquare1) / (2 * epsilon);
            guess[1] -= epsilon;
            guess[2] += 1;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[2] -= 2;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[2] += 1;
            res2 = (chiSquare2 - chiSquare1) / 2;
            guess[2] -= 1;
            Assert.AreEqual(res0, results[0], 1e-6);
            Assert.AreEqual(res1, results[1], 1e-6);
            Assert.AreEqual(res2, results[2], 1e-6);
        }

        [Test]
        public void Test7()
        {
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            double[] alphas = new double[] { 2, 1 };
            double[] betas = new double[] { 5, 0 };
            int[] incrs = new int[] { 0, 0 };
            double[] data = new double[n * l];
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < l; i++)
                {
                    double x = 0;
                    for (int j = 0; j < amplitudes.Length; j++)
                    {
                        x += amplitudes[j] * Math.Sin((i + incrs[k]) * tStep * 2 * Math.PI * (j + 1) / period);
                    }
                    data[k * l + i] = alphas[k] * x + betas[k];
                }
            }
            double calibrationTimeWindowFactor = 0.5;
            int m = (int)(calibrationTimeWindowFactor * l); // width of window
            int k_min = l / 2 - m / 2; // first index in the window if not delays

            List<double> guesses = new List<double>();
            guesses.Add(alphas[0]);
            guesses.Add(betas[0]);
            guesses.Add(incrs[0]);

            double[] guess = guesses.ToArray();
            double chiSquare = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            Assert.AreEqual(0, chiSquare, 1e-6);
            double[] results = new double[(n - 1) * 3];
            FuseAndCalibrateSignals.ChiSquarePartialDerivatives(guess, data, results, n, l, m, k_min);
            double epsilon = 1e-6;
            guess[0] += epsilon;
            double chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[0] -= 2 * epsilon;
            double chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[0] += epsilon;
            double res0 = (chiSquare2 - chiSquare1) / (2 * epsilon);
            guess[1] += epsilon;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[1] -= 2 * epsilon;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[1] += epsilon;
            double res1 = (chiSquare2 - chiSquare1) / (2 * epsilon);
            guess[1] -= epsilon;
            guess[2] += 1;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[2] -= 2;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[2] += 1;
            double res2 = (chiSquare2 - chiSquare1) / 2;
            guess[2] -= 1;
            Assert.AreEqual(res0, results[0], 1e-6);
            Assert.AreEqual(res1, results[1], 1e-6);
            Assert.AreEqual(res2, results[2], 1e-6);
            guesses = new List<double>();
            guesses.Add(1);
            guesses.Add(0);
            guesses.Add(0);
            guess = guesses.ToArray();
            chiSquare = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            Assert.AreEqual(8063.9999999999973, chiSquare, 1e-6);
            results = new double[(n - 1) * 3];
            FuseAndCalibrateSignals.ChiSquarePartialDerivatives(guess, data, results, n, l, m, k_min);
            epsilon = 1e-6;
            guess[0] += epsilon;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[0] -= 2 * epsilon;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[0] += epsilon;
            res0 = (chiSquare2 - chiSquare1) / (2 * epsilon);
            guess[1] += epsilon;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[1] -= 2 * epsilon;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[1] += epsilon;
            res1 = (chiSquare2 - chiSquare1) / (2 * epsilon);
            guess[1] -= epsilon;
            guess[2] += 1;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[2] -= 2;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[2] += 1;
            res2 = (chiSquare2 - chiSquare1) / 2;
            guess[2] -= 1;
            Assert.AreEqual(res0, results[0], 1e-5);
            Assert.AreEqual(res1, results[1], 1e-5);
            Assert.AreEqual(res2, results[2], 1e-5);
        }

        [Test]
        public void Test8()
        {
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            double[] alphas = new double[] { 2, 1 };
            double[] betas = new double[] { 5, 0 };
            int[] incrs = new int[] { 4, 0 };
            double[] data = new double[n * l];
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < l; i++)
                {
                    double x = 0;
                    for (int j = 0; j < amplitudes.Length; j++)
                    {
                        x += amplitudes[j] * Math.Sin((i + incrs[k]) * tStep * 2 * Math.PI * (j + 1) / period);
                    }
                    data[k * l + i] = alphas[k] * x + betas[k];
                }
            }
            double calibrationTimeWindowFactor = 0.5;
            int m = (int)(calibrationTimeWindowFactor * l); // width of window
            int k_min = l / 2 - m / 2; // first index in the window if not delays

            List<double> guesses = new List<double>();
            guesses.Add(alphas[0]);
            guesses.Add(betas[0]);
            guesses.Add(incrs[0]);

            double[] guess = guesses.ToArray();
            double chiSquare = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            Assert.AreEqual(0, chiSquare, 1e-6);
            double[] results = new double[(n - 1) * 3];
            FuseAndCalibrateSignals.ChiSquarePartialDerivatives(guess, data, results, n, l, m, k_min);
            double epsilon = 1e-6;
            guess[0] += epsilon;
            double chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[0] -= 2 * epsilon;
            double chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[0] += epsilon;
            double res0 = (chiSquare2 - chiSquare1) / (2 * epsilon);
            guess[1] += epsilon;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[1] -= 2 * epsilon;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[1] += epsilon;
            double res1 = (chiSquare2 - chiSquare1) / (2 * epsilon);
            guess[1] -= epsilon;
            guess[2] += 1;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[2] -= 2;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[2] += 1;
            double res2 = (chiSquare2 - chiSquare1) / 2;
            guess[2] -= 1;
            Assert.AreEqual(res0, results[0], 1e-6);
            Assert.AreEqual(res1, results[1], 1e-6);
            Assert.AreEqual(res2, results[2], 1e-6);

            guesses = new List<double>();
            guesses.Add(1);
            guesses.Add(0);
            guesses.Add(0);

            guess = guesses.ToArray();
            chiSquare = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            Assert.AreEqual(9091.16255943506, chiSquare, 1e-6);
            results = new double[(n - 1) * 3];
            FuseAndCalibrateSignals.ChiSquarePartialDerivatives(guess, data, results, n, l, m, k_min);
            epsilon = 1e-6;
            guess[0] += epsilon;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[0] -= 2 * epsilon;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[0] += epsilon;
            res0 = (chiSquare2 - chiSquare1) / (2 * epsilon);
            guess[1] += epsilon;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[1] -= 2 * epsilon;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[1] += epsilon;
            res1 = (chiSquare2 - chiSquare1) / (2 * epsilon);
            guess[1] -= epsilon;
            guess[2] += 1;
            chiSquare2 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[2] -= 2;
            chiSquare1 = FuseAndCalibrateSignals.ChiSquare(guess, data, n, l, m, k_min);
            guess[2] += 1;
            res2 = (chiSquare2 - chiSquare1) / 2;
            guess[2] -= 1;
            Assert.AreEqual(res0, results[0], 1e-5);
            Assert.AreEqual(res1, results[1], 1e-5);
            Assert.AreEqual(res2, results[2], 1e-5);
        }


        [Test]
        public void Test9()
        {
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            double[] data1 = new double[600];
            double alpha = 1.0;
            double beta = 0.0;
            int incr = 0;
            for (int i = 0; i < data1.Length; i++)
            {
                double x = 0;
                for (int j = 0; j < amplitudes.Length; j++)
                {
                    x += amplitudes[j] * Math.Sin((i+incr) * tStep * 2 * Math.PI * (j + 1) / period);
                }
                data1[i] = alpha * x + beta;
            }
            double[] data2 = new double[600];
            for (int i = 0; i < data2.Length; i++)
            {
                double x = 0;
                for (int j = 0; j < amplitudes.Length; j++)
                {
                    x += amplitudes[j] * Math.Sin(i * tStep * 2 * Math.PI * (j + 1) / period);
                }
                data2[i] = x;
            }
            Dictionary<API.DTO.DWISNodeID, double[]>  reinterpolatedValues = new Dictionary<API.DTO.DWISNodeID, double[]>();
            reinterpolatedValues.Add(new DWISNodeID() { NameSpaceIndex = 0, ID = "1" }, data1);
            reinterpolatedValues.Add(new DWISNodeID() { NameSpaceIndex = 0, ID = "2" }, data2);

            Dictionary<string, CalibrationParameters> calibrations = new Dictionary<string, CalibrationParameters>();
            foreach (var kvp in reinterpolatedValues) 
            {
                calibrations.Add(kvp.Key.ToString(), new CalibrationParameters() { Scaling = 1.0, Bias = 0.0, Delay = TimeSpan.Zero });
            }
            FuseAndCalibrateSignals.Optimize(reinterpolatedValues, calibrations, TimeSpan.FromSeconds(1));
            foreach (var kvp in calibrations)
            {
                Assert.AreEqual(1, kvp.Value.Scaling, 1e-6);
                Assert.AreEqual(0, kvp.Value.Bias, 1e-6);
                Assert.AreEqual(0, kvp.Value.Delay.TotalSeconds, 1e-6);
            }
        }

        [Test]
        public void Test10()
        {
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            double[] data1 = new double[600];
            double alpha = 2.0;
            double beta = 5.0;
            int incr = 4;
            for (int i = 0; i < data1.Length; i++)
            {
                double x = 0;
                for (int j = 0; j < amplitudes.Length; j++)
                {
                    x += amplitudes[j] * Math.Sin((i + incr) * tStep * 2 * Math.PI * (j + 1) / period);
                }
                data1[i] = alpha * x + beta;
            }

            double[] data2 = new double[600];
            for (int i = 0; i < data2.Length; i++)
            {
                double x = 0;
                for (int j = 0; j < amplitudes.Length; j++)
                {
                    x += amplitudes[j] * Math.Sin(i * tStep * 2 * Math.PI * (j + 1) / period);
                }
                data2[i] = x;
            }
            DWISNodeID nodeID1 = new DWISNodeID() { NameSpaceIndex = 0, ID = "1" };
            DWISNodeID nodeID2 = new DWISNodeID() { NameSpaceIndex = 0, ID = "2" };
            Dictionary<API.DTO.DWISNodeID, double[]> reinterpolatedValues = new Dictionary<API.DTO.DWISNodeID, double[]>();
            reinterpolatedValues.Add(nodeID1, data1);
            reinterpolatedValues.Add(nodeID2, data2);
            TimeSpan timeStep = TimeSpan.FromSeconds(1);
            Dictionary<string, CalibrationParameters> calibrations = new Dictionary<string, CalibrationParameters>();
            foreach (var kvp in reinterpolatedValues)
            {
                calibrations.Add(kvp.Key.ToString(), new CalibrationParameters() { Scaling = 1.0, Bias = 0.0, Delay = TimeSpan.Zero });
            }
            calibrations[nodeID1.ToString()].Scaling = alpha;
            calibrations[nodeID1.ToString()].Bias = beta;
            calibrations[nodeID1.ToString()].Delay = incr * timeStep;
            FuseAndCalibrateSignals.Optimize(reinterpolatedValues, calibrations, timeStep);

            Assert.AreEqual(alpha, calibrations[nodeID1.ToString()].Scaling, 1e-6);
            Assert.AreEqual(beta, calibrations[nodeID1.ToString()].Bias, 1e-6);
            Assert.AreEqual(incr * timeStep.TotalSeconds, calibrations[nodeID1.ToString()].Delay.TotalSeconds, 1e-6);
            Assert.AreEqual(1, calibrations[nodeID2.ToString()].Scaling, 1e-6);
            Assert.AreEqual(0, calibrations[nodeID2.ToString()].Bias, 1e-6);
            Assert.AreEqual(0, calibrations[nodeID2.ToString()].Delay.TotalSeconds, 1e-6);

            calibrations = new Dictionary<string, CalibrationParameters>();
            foreach (var kvp in reinterpolatedValues)
            {
                calibrations.Add(kvp.Key.ToString(), new CalibrationParameters() { Scaling = 1.0, Bias = 0.0, Delay = TimeSpan.Zero });
            }
            double factor = 0.98;
            calibrations[nodeID1.ToString()].Scaling = alpha* factor;
            calibrations[nodeID1.ToString()].Bias = beta* factor;
            calibrations[nodeID1.ToString()].Delay = (int)(incr* factor) * timeStep;
            FuseAndCalibrateSignals.Optimize(reinterpolatedValues, calibrations, timeStep);

            Assert.AreEqual(alpha, calibrations[nodeID1.ToString()].Scaling, 1e-3);
            Assert.AreEqual(beta, calibrations[nodeID1.ToString()].Bias, 1e-3);
            Assert.AreEqual(incr * timeStep.TotalSeconds, calibrations[nodeID1.ToString()].Delay.TotalSeconds, 0.5);
            Assert.AreEqual(1, calibrations[nodeID2.ToString()].Scaling, 1e-6);
            Assert.AreEqual(0, calibrations[nodeID2.ToString()].Bias, 1e-6);
            Assert.AreEqual(0, calibrations[nodeID2.ToString()].Delay.TotalSeconds, 1e-6);

            factor = 0.90;
            calibrations[nodeID1.ToString()].Scaling = alpha * factor;
            calibrations[nodeID1.ToString()].Bias = beta * factor;
            calibrations[nodeID1.ToString()].Delay = (int)(incr * factor) * timeStep;
            FuseAndCalibrateSignals.Optimize(reinterpolatedValues, calibrations, timeStep);

            Assert.AreEqual(alpha, calibrations[nodeID1.ToString()].Scaling, 1e-3);
            Assert.AreEqual(beta, calibrations[nodeID1.ToString()].Bias, 1e-3);
            Assert.AreEqual(incr * timeStep.TotalSeconds, calibrations[nodeID1.ToString()].Delay.TotalSeconds, 0.5);
            Assert.AreEqual(1, calibrations[nodeID2.ToString()].Scaling, 1e-6);
            Assert.AreEqual(0, calibrations[nodeID2.ToString()].Bias, 1e-6);
            Assert.AreEqual(0, calibrations[nodeID2.ToString()].Delay.TotalSeconds, 1e-6);

            factor = 0.50;
            calibrations[nodeID1.ToString()].Scaling = alpha * factor;
            calibrations[nodeID1.ToString()].Bias = beta * factor;
            calibrations[nodeID1.ToString()].Delay = (int)(incr * factor) * timeStep;
            FuseAndCalibrateSignals.Optimize(reinterpolatedValues, calibrations, timeStep);

            Assert.AreEqual(alpha, calibrations[nodeID1.ToString()].Scaling, 1e-3);
            Assert.AreEqual(beta, calibrations[nodeID1.ToString()].Bias, 1e-3);
            Assert.AreEqual(incr * timeStep.TotalSeconds, calibrations[nodeID1.ToString()].Delay.TotalSeconds, 0.5);
            Assert.AreEqual(1, calibrations[nodeID2.ToString()].Scaling, 1e-6);
            Assert.AreEqual(0, calibrations[nodeID2.ToString()].Bias, 1e-6);
            Assert.AreEqual(0, calibrations[nodeID2.ToString()].Delay.TotalSeconds, 1e-6);

            factor = 0.20;
            calibrations[nodeID1.ToString()].Scaling = alpha * factor;
            calibrations[nodeID1.ToString()].Bias = beta * factor;
            calibrations[nodeID1.ToString()].Delay = (int)(incr * factor) * timeStep;
            FuseAndCalibrateSignals.Optimize(reinterpolatedValues, calibrations, timeStep);

            Assert.AreEqual(alpha, calibrations[nodeID1.ToString()].Scaling, 1e-3);
            Assert.AreEqual(beta, calibrations[nodeID1.ToString()].Bias, 1e-3);
            Assert.AreEqual(incr * timeStep.TotalSeconds, calibrations[nodeID1.ToString()].Delay.TotalSeconds, 0.5);
            Assert.AreEqual(1, calibrations[nodeID2.ToString()].Scaling, 1e-6);
            Assert.AreEqual(0, calibrations[nodeID2.ToString()].Bias, 1e-6);
            Assert.AreEqual(0, calibrations[nodeID2.ToString()].Delay.TotalSeconds, 1e-6);
        }
    }
}