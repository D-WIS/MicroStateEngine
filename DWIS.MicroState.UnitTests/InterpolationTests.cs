using DWIS.API.DTO;
using DWIS.MicroState.Model;
using OSDC.DotnetLibraries.Drilling.DrillingProperties;

namespace DWIS.MicroState.UnitTests
{
    public class InterpolationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>> values = new Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>>();
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            double[] alphas = new double[] { 2, 1 };
            double[] betas = new double[] { 5, 0 };
            int[] incrs = new int[] { 4, 0 };
            DateTime[] starts = new DateTime[] { DateTime.Now, DateTime.Now + TimeSpan.FromSeconds(10) };
            TimeSpan[] steps = new TimeSpan[] { TimeSpan.FromSeconds(0.8), TimeSpan.FromSeconds(1.4) };
            for (int k = 0; k < n; k++)
            {
                CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>> data = new CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>(l);
                DWISNodeID nodeID = new DWISNodeID() { NameSpaceIndex = 0, ID = k.ToString() };
                values.Add(nodeID, data);
                for (int i = 0; i < l; i++)
                {
                    double x = 0;
                    for (int j = 0; j < amplitudes.Length; j++)
                    {
                        x += amplitudes[j] * Math.Sin((i + incrs[k]) * tStep * 2 * Math.PI * (j + 1) / period);
                    }
                    GaussianDrillingProperty prop = new GaussianDrillingProperty();
                    prop.Mean = alphas[k] * x + betas[k];
                    prop.StandardDeviation = 0.01;
                    data.Add(new Tuple<DateTime, GaussianDrillingProperty>(starts[k] + i*steps[k], prop));
                }
            }
            DateTime minTimeStamp, maxTimeStamp;
            FuseAndCalibrateSignals.GetMinMaxTimeStamps<GaussianDrillingProperty>(values, out minTimeStamp, out maxTimeStamp);
            Assert.AreEqual(starts[1], minTimeStamp);
            Assert.AreEqual(starts[0] + (l - 1) * steps[0], maxTimeStamp);
        }

        [Test]
        public void Test2()
        {
            Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>> values = new Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>>();
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            double[] alphas = new double[] { 1, 1 };
            double[] betas = new double[] { 0, 0 };
            int[] incrs = new int[] { 0, 0 };
            DateTime[] starts = new DateTime[] { DateTime.Now, DateTime.Now };
            TimeSpan[] steps = new TimeSpan[] { TimeSpan.FromSeconds(0.8), TimeSpan.FromSeconds(0.8) };
            List<DWISNodeID> nodes = new List<DWISNodeID>();
            for (int k = 0; k < n; k++)
            {
                CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>> data = new CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>(l);
                DWISNodeID nodeID = new DWISNodeID() { NameSpaceIndex = 0, ID = k.ToString() };
                nodes.Add(nodeID);
                values.Add(nodeID, data);
                for (int i = 0; i < l; i++)
                {
                    double x = 0;
                    for (int j = 0; j < amplitudes.Length; j++)
                    {
                        x += amplitudes[j] * Math.Sin((i + incrs[k]) * tStep * 2 * Math.PI * (j + 1) / period);
                    }
                    GaussianDrillingProperty prop = new GaussianDrillingProperty();
                    prop.Mean = alphas[k] * x + betas[k];
                    prop.StandardDeviation = 0.01;
                    data.Add(new Tuple<DateTime, GaussianDrillingProperty>(starts[k] + i * steps[k], prop));
                }
            }
            DateTime minTimeStamp, maxTimeStamp;
            FuseAndCalibrateSignals.GetMinMaxTimeStamps<GaussianDrillingProperty>(values, out minTimeStamp, out maxTimeStamp);
            TimeSpan timeStep = TimeSpan.FromSeconds(1);
            Dictionary<DWISNodeID, double[]> reinterpolated = new Dictionary<DWISNodeID, double[]>();
            FuseAndCalibrateSignals.Reinterpolate(values, minTimeStamp, maxTimeStamp, timeStep, reinterpolated);
            Assert.AreEqual(reinterpolated[nodes[0]].Length, reinterpolated[nodes[1]].Length);
            for (int i = 0; i < reinterpolated[nodes[0]].Length; i++)
            {
                Assert.AreEqual(reinterpolated[nodes[0]][i], reinterpolated[nodes[1]][i], 1e-4);
            }
        }

        [Test]
        public void Test3()
        {
            Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>>> values = new Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>>>();
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            double[] alphas = new double[] { 1, 1 };
            double[] betas = new double[] { 0, 0 };
            int[] incrs = new int[] { 0, 0 };
            DateTime[] starts = new DateTime[] { DateTime.Now, DateTime.Now };
            TimeSpan[] steps = new TimeSpan[] { TimeSpan.FromSeconds(0.8), TimeSpan.FromSeconds(0.8) };
            List<DWISNodeID> nodes = new List<DWISNodeID>();
            for (int k = 0; k < n; k++)
            {
                CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>> data = new CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>>(l);
                DWISNodeID nodeID = new DWISNodeID() { NameSpaceIndex = 0, ID = k.ToString() };
                nodes.Add(nodeID);
                values.Add(nodeID, data);
                for (int i = 0; i < l; i++)
                {
                    double x = 0;
                    for (int j = 0; j < amplitudes.Length; j++)
                    {
                        x += amplitudes[j] * Math.Sin((i + incrs[k]) * tStep * 2 * Math.PI * (j + 1) / period);
                    }
                    ScalarDrillingProperty prop = new ScalarDrillingProperty();
                    prop.ScalarValue = alphas[k] * x + betas[k];
                    data.Add(new Tuple<DateTime, ScalarDrillingProperty>(starts[k] + i * steps[k], prop));
                }
            }
            DateTime minTimeStamp, maxTimeStamp;
            FuseAndCalibrateSignals.GetMinMaxTimeStamps<ScalarDrillingProperty>(values, out minTimeStamp, out maxTimeStamp);
            TimeSpan timeStep = TimeSpan.FromSeconds(1);
            Dictionary<DWISNodeID, double[]> reinterpolated = new Dictionary<DWISNodeID, double[]>();
            FuseAndCalibrateSignals.Reinterpolate(values, minTimeStamp, maxTimeStamp, timeStep, reinterpolated);
            Assert.AreEqual(reinterpolated[nodes[0]].Length, reinterpolated[nodes[1]].Length);
            for (int i = 0; i < reinterpolated[nodes[0]].Length; i++)
            {
                Assert.AreEqual(reinterpolated[nodes[0]][i], reinterpolated[nodes[1]][i], 1e-4);
            }
        }

        [Test]
        public void Test4()
        {
            Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>>> values = new Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>>>();
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            double[] alphas = new double[] { 1, 1 };
            double[] betas = new double[] { 0, 0 };
            int[] incrs = new int[] { 0, 0 };
            DateTime[] starts = new DateTime[] { DateTime.Now, DateTime.Now };
            TimeSpan[] steps = new TimeSpan[] { TimeSpan.FromSeconds(0.8), TimeSpan.FromSeconds(0.8) };
            List<DWISNodeID> nodes = new List<DWISNodeID>();
            for (int k = 0; k < n; k++)
            {
                CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>> data = new CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>>(l);
                DWISNodeID nodeID = new DWISNodeID() { NameSpaceIndex = 0, ID = k.ToString() };
                nodes.Add(nodeID);
                values.Add(nodeID, data);
                for (int i = 0; i < l; i++)
                {
                    double x = 0;
                    for (int j = 0; j < amplitudes.Length; j++)
                    {
                        x += amplitudes[j] * Math.Sin((i + incrs[k]) * tStep * 2 * Math.PI * (j + 1) / period);
                    }
                    ScalarDrillingProperty prop = new ScalarDrillingProperty();
                    prop.ScalarValue = alphas[k] * x + betas[k];
                    data.Add(new Tuple<DateTime, ScalarDrillingProperty>(starts[k] + i * steps[k], prop));
                }
            }
            ScalarDrillingProperty? propResult = FuseAndCalibrateSignals.Interpolate(values[nodes[0]], starts[0], TimeSpan.Zero, 1.0, 0.0);
            Assert.NotNull(propResult);
            Assert.AreEqual(values[nodes[0]].GetItems().ElementAt<Tuple<DateTime, ScalarDrillingProperty>>(0).Item2.ScalarValue.Value, propResult.ScalarValue, 1e-4);
        }
        [Test]
        public void Test5()
        {
            Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>> values = new Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>>();
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            double[] alphas = new double[] { 1, 1 };
            double[] betas = new double[] { 0, 0 };
            int[] incrs = new int[] { 0, 0 };
            DateTime[] starts = new DateTime[] { DateTime.Now, DateTime.Now };
            TimeSpan[] steps = new TimeSpan[] { TimeSpan.FromSeconds(0.8), TimeSpan.FromSeconds(0.8) };
            List<DWISNodeID> nodes = new List<DWISNodeID>();
            for (int k = 0; k < n; k++)
            {
                CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>> data = new CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>(l);
                DWISNodeID nodeID = new DWISNodeID() { NameSpaceIndex = 0, ID = k.ToString() };
                nodes.Add(nodeID);
                values.Add(nodeID, data);
                for (int i = 0; i < l; i++)
                {
                    double x = 0;
                    for (int j = 0; j < amplitudes.Length; j++)
                    {
                        x += amplitudes[j] * Math.Sin((i + incrs[k]) * tStep * 2 * Math.PI * (j + 1) / period);
                    }
                    GaussianDrillingProperty prop = new GaussianDrillingProperty();
                    prop.Mean = alphas[k] * x + betas[k];
                    prop.StandardDeviation = 0.01;
                    data.Add(new Tuple<DateTime, GaussianDrillingProperty>(starts[k] + i * steps[k], prop));
                }
            }
            GaussianDrillingProperty? propResult = FuseAndCalibrateSignals.Interpolate(values[nodes[0]], starts[0], TimeSpan.Zero, 1.0, 0.0);
            Assert.NotNull(propResult);
            Assert.AreEqual(values[nodes[0]].GetItems().ElementAt<Tuple<DateTime, GaussianDrillingProperty>>(0).Item2.Mean.Value, propResult.Mean, 1e-4);
            Assert.AreEqual(values[nodes[0]].GetItems().ElementAt<Tuple<DateTime, GaussianDrillingProperty>>(0).Item2.StandardDeviation.Value, propResult.StandardDeviation, 1e-4);
        }
        [Test]
        public void Test6()
        {
            Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, BernoulliDrillingProperty>>> values = new Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, BernoulliDrillingProperty>>>();
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            double[] alphas = new double[] { 1, 1 };
            double[] betas = new double[] { 0, 0 };
            int[] incrs = new int[] { 0, 0 };
            DateTime[] starts = new DateTime[] { DateTime.Now, DateTime.Now };
            TimeSpan[] steps = new TimeSpan[] { TimeSpan.FromSeconds(0.8), TimeSpan.FromSeconds(0.8) };
            List<DWISNodeID> nodes = new List<DWISNodeID>();
            for (int k = 0; k < n; k++)
            {
                CircularBuffer<Tuple<DateTime, BernoulliDrillingProperty>> data = new CircularBuffer<Tuple<DateTime, BernoulliDrillingProperty>>(l);
                DWISNodeID nodeID = new DWISNodeID() { NameSpaceIndex = 0, ID = k.ToString() };
                nodes.Add(nodeID);
                values.Add(nodeID, data);
                for (int i = 0; i < l; i++)
                {
                    double x = 0;
                    for (int j = 0; j < amplitudes.Length; j++)
                    {
                        x += amplitudes[j] * Math.Sin((i + incrs[k]) * tStep * 2 * Math.PI * (j + 1) / period);
                    }
                    BernoulliDrillingProperty prop = new BernoulliDrillingProperty();
                    prop.Probability = alphas[k] * x + betas[k];
                    data.Add(new Tuple<DateTime, BernoulliDrillingProperty>(starts[k] + i * steps[k], prop));
                }
            }
            BernoulliDrillingProperty? propResult = FuseAndCalibrateSignals.Interpolate(values[nodes[0]], starts[0], TimeSpan.Zero);
            Assert.NotNull(propResult);
            Assert.AreEqual(values[nodes[0]].GetItems().ElementAt<Tuple<DateTime, BernoulliDrillingProperty>>(0).Item2.Probability.Value, propResult.Probability, 1e-4);
        }

        [Test]
        public void Test7()
        {
            Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>> values = new Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>>();
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            double[] alphas = new double[] { 1, 1 };
            double[] betas = new double[] { 0, 0 };
            int[] incrs = new int[] { 0, 0 };
            DateTime[] starts = new DateTime[] { DateTime.Now, DateTime.Now + TimeSpan.FromSeconds(5)};
            TimeSpan[] steps = new TimeSpan[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1) };
            List<DWISNodeID> nodes = new List<DWISNodeID>();
            for (int k = 0; k < n; k++)
            {
                CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>> data = new CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>(l);
                DWISNodeID nodeID = new DWISNodeID() { NameSpaceIndex = 0, ID = k.ToString() };
                nodes.Add(nodeID);
                values.Add(nodeID, data);
                for (int i = 0; i < l; i++)
                {
                    double x = 0;
                    for (int j = 0; j < amplitudes.Length; j++)
                    {
                        x += amplitudes[j] * Math.Sin((i + incrs[k]) * tStep * 2 * Math.PI * (j + 1) / period);
                    }
                    GaussianDrillingProperty prop = new GaussianDrillingProperty();
                    prop.Mean = alphas[k] * x + betas[k];
                    prop.StandardDeviation = 0.01;
                    data.Add(new Tuple<DateTime, GaussianDrillingProperty>(starts[k] + i * steps[k], prop));
                }
            }
            DateTime minTimeStamp, maxTimeStamp;
            FuseAndCalibrateSignals.GetMinMaxTimeStamps<GaussianDrillingProperty>(values, out minTimeStamp, out maxTimeStamp);
            TimeSpan timeStep = TimeSpan.FromSeconds(1);
            Dictionary<DWISNodeID, double[]> reinterpolated = new Dictionary<DWISNodeID, double[]>();
            FuseAndCalibrateSignals.Reinterpolate(values, minTimeStamp, maxTimeStamp, timeStep, reinterpolated);
            Assert.AreEqual(reinterpolated[nodes[0]].Length, reinterpolated[nodes[1]].Length);
            for (int i = 0; i < reinterpolated[nodes[0]].Length-5; i++)
            {
                Assert.AreEqual(reinterpolated[nodes[0]][i], reinterpolated[nodes[1]][i+5], 1e-4);
            }
        }

        [Test]
        public void Test8()
        {
            Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>> values = new Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>>();
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            double[] alphas = new double[] { 1, 1 };
            double[] betas = new double[] { 0, 0 };
            int[] incrs = new int[] { 5, 0 };
            DateTime[] starts = new DateTime[] { DateTime.Now, DateTime.Now-TimeSpan.FromSeconds(5) };
            TimeSpan[] steps = new TimeSpan[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1) };
            List<DWISNodeID> nodes = new List<DWISNodeID>();
            for (int k = 0; k < n; k++)
            {
                CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>> data = new CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>(l);
                DWISNodeID nodeID = new DWISNodeID() { NameSpaceIndex = 0, ID = k.ToString() };
                nodes.Add(nodeID);
                values.Add(nodeID, data);
                for (int i = 0; i < l; i++)
                {
                    double x = 0;
                    for (int j = 0; j < amplitudes.Length; j++)
                    {
                        x += amplitudes[j] * Math.Sin((i + incrs[k]) * tStep * 2 * Math.PI * (j + 1) / period);
                    }
                    GaussianDrillingProperty prop = new GaussianDrillingProperty();
                    prop.Mean = alphas[k] * x + betas[k];
                    prop.StandardDeviation = 0.01;
                    data.Add(new Tuple<DateTime, GaussianDrillingProperty>(starts[k] + i * steps[k], prop));
                }
            }
            DateTime minTimeStamp, maxTimeStamp;
            FuseAndCalibrateSignals.GetMinMaxTimeStamps<GaussianDrillingProperty>(values, out minTimeStamp, out maxTimeStamp);
            TimeSpan timeStep = TimeSpan.FromSeconds(1);
            Dictionary<DWISNodeID, double[]> reinterpolated = new Dictionary<DWISNodeID, double[]>();
            FuseAndCalibrateSignals.Reinterpolate(values, minTimeStamp, maxTimeStamp, timeStep, reinterpolated);
            Assert.AreEqual(reinterpolated[nodes[0]].Length, reinterpolated[nodes[1]].Length);
            for (int i = 0; i < reinterpolated[nodes[0]].Length; i++)
            {
                Assert.AreEqual(reinterpolated[nodes[0]][i], reinterpolated[nodes[1]][i], 1e-4);
            }
        }
    }
}
