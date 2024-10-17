using DWIS.API.DTO;
using DWIS.MicroState.Model;
using OSDC.DotnetLibraries.Drilling.DrillingProperties;

namespace DWIS.MicroState.UnitTests
{
    public class ManageCalibrationTests
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
            double[] alphas = new double[] { 1, 1 };
            double[] betas = new double[] { 0, 0 };
            int[] incrs = new int[] { 0, 0 };
            DateTime[] starts = new DateTime[] { DateTime.Now, DateTime.Now };
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
            Dictionary<DWISNodeID, CalibrationParameters> calibrations = new Dictionary<DWISNodeID, CalibrationParameters>();
    
            FuseAndCalibrateSignals.ManageCalibrations(values, calibrations);
            Assert.IsTrue(calibrations.ContainsKey(nodes[0]));
            Assert.IsTrue(calibrations.ContainsKey(nodes[1]));
            Assert.AreEqual(alphas[0], calibrations[nodes[0]].Scaling, 1e-4);
            Assert.AreEqual(betas[0], calibrations[nodes[0]].Bias, 1e-4);
            Assert.AreEqual(incrs[0], calibrations[nodes[0]].Delay.TotalSeconds, 0.5);
            Assert.AreEqual(alphas[1], calibrations[nodes[1]].Scaling, 1e-4);
            Assert.AreEqual(betas[1], calibrations[nodes[1]].Bias, 1e-4);
            Assert.AreEqual(incrs[1], calibrations[nodes[1]].Delay.TotalSeconds, 0.5);
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
            double[] alphas = new double[] { 2, 1 };
            double[] betas = new double[] { 0, 0 };
            int[] incrs = new int[] { 0, 0 };
            DateTime[] starts = new DateTime[] { DateTime.Now, DateTime.Now };
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
            Dictionary<DWISNodeID, CalibrationParameters> calibrations = new Dictionary<DWISNodeID, CalibrationParameters>();

            FuseAndCalibrateSignals.ManageCalibrations(values, calibrations);
            Assert.IsTrue(calibrations.ContainsKey(nodes[0]));
            Assert.IsTrue(calibrations.ContainsKey(nodes[1]));
            Assert.AreEqual(alphas[0], calibrations[nodes[0]].Scaling, 1e-4);
            Assert.AreEqual(betas[0], calibrations[nodes[0]].Bias, 1e-4);
            Assert.AreEqual(incrs[0], calibrations[nodes[0]].Delay.TotalSeconds, 0.5);
            Assert.AreEqual(alphas[1], calibrations[nodes[1]].Scaling, 1e-4);
            Assert.AreEqual(betas[1], calibrations[nodes[1]].Bias, 1e-4);
            Assert.AreEqual(incrs[1], calibrations[nodes[1]].Delay.TotalSeconds, 0.5);
        }

        [Test]
        public void Test3()
        {
            Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>> values = new Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>>();
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            double[] alphas = new double[] { 2, 1 };
            double[] betas = new double[] { 5, 0 };
            int[] incrs = new int[] { 0, 0 };
            DateTime[] starts = new DateTime[] { DateTime.Now, DateTime.Now };
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
            Dictionary<DWISNodeID, CalibrationParameters> calibrations = new Dictionary<DWISNodeID, CalibrationParameters>();

            FuseAndCalibrateSignals.ManageCalibrations(values, calibrations);
            Assert.IsTrue(calibrations.ContainsKey(nodes[0]));
            Assert.IsTrue(calibrations.ContainsKey(nodes[1]));
            Assert.AreEqual(alphas[0], calibrations[nodes[0]].Scaling, 1e-4);
            Assert.AreEqual(betas[0], calibrations[nodes[0]].Bias, 1e-4);
            Assert.AreEqual(incrs[0], calibrations[nodes[0]].Delay.TotalSeconds, 0.75);
            Assert.AreEqual(alphas[1], calibrations[nodes[1]].Scaling, 1e-4);
            Assert.AreEqual(betas[1], calibrations[nodes[1]].Bias, 1e-4);
            Assert.AreEqual(incrs[1], calibrations[nodes[1]].Delay.TotalSeconds, 0.75);
        }
        [Test]
        public void Test4()
        {
            Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>> values = new Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>>();
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            double[] alphas = new double[] { 2, 1 };
            double[] betas = new double[] { 5, 0 };
            int[] incrs = new int[] { 3, 0 };
            DateTime[] starts = new DateTime[] { DateTime.Now, DateTime.Now - TimeSpan.FromSeconds(3) };
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
            Dictionary<DWISNodeID, CalibrationParameters> calibrations = new Dictionary<DWISNodeID, CalibrationParameters>();

            FuseAndCalibrateSignals.ManageCalibrations(values, calibrations);
            Assert.IsTrue(calibrations.ContainsKey(nodes[0]));
            Assert.IsTrue(calibrations.ContainsKey(nodes[1]));
            Assert.AreEqual(alphas[0], calibrations[nodes[0]].Scaling, 1e-4);
            Assert.AreEqual(betas[0], calibrations[nodes[0]].Bias, 1e-4);
            Assert.AreEqual(0.0, calibrations[nodes[0]].Delay.TotalSeconds, 0.5);
            Assert.AreEqual(alphas[1], calibrations[nodes[1]].Scaling, 1e-4);
            Assert.AreEqual(betas[1], calibrations[nodes[1]].Bias, 1e-4);
            Assert.AreEqual(incrs[1], calibrations[nodes[1]].Delay.TotalSeconds, 0.5);
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
            double[] alphas = new double[] { 2, 1 };
            double[] betas = new double[] { 5, 0 };
            int[] incrs = new int[] { 1, 0 };
            DateTime[] starts = new DateTime[] { DateTime.Now, DateTime.Now};
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
            Dictionary<DWISNodeID, CalibrationParameters> calibrations = new Dictionary<DWISNodeID, CalibrationParameters>();

            FuseAndCalibrateSignals.ManageCalibrations(values, calibrations);
            Assert.IsTrue(calibrations.ContainsKey(nodes[0]));
            Assert.IsTrue(calibrations.ContainsKey(nodes[1]));
            Assert.AreEqual(alphas[0], calibrations[nodes[0]].Scaling, 0.2);
            Assert.AreEqual(betas[0], calibrations[nodes[0]].Bias, 0.2);
            Assert.AreEqual(1.0, calibrations[nodes[0]].Delay.TotalSeconds, 0.5);
            Assert.AreEqual(alphas[1], calibrations[nodes[1]].Scaling, 1e-4);
            Assert.AreEqual(betas[1], calibrations[nodes[1]].Bias, 1e-4);
            Assert.AreEqual(incrs[1], calibrations[nodes[1]].Delay.TotalSeconds, 0.5);
        }
        [Test]
        public void Test6()
        {
            Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>> values = new Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>>();
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            double[] alphas = new double[] { 2, 1 };
            double[] betas = new double[] { 5, 0 };
            int[] incrs = new int[] { 5, 0 };
            DateTime[] starts = new DateTime[] { DateTime.Now, DateTime.Now };
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
            Dictionary<DWISNodeID, CalibrationParameters> calibrations = new Dictionary<DWISNodeID, CalibrationParameters>();

            FuseAndCalibrateSignals.ManageCalibrations(values, calibrations);
            Assert.IsTrue(calibrations.ContainsKey(nodes[0]));
            Assert.IsTrue(calibrations.ContainsKey(nodes[1]));
            Assert.AreEqual(alphas[0], calibrations[nodes[0]].Scaling, 0.3);
            Assert.AreEqual(betas[0], calibrations[nodes[0]].Bias, 0.2);
            Assert.AreEqual(5.0, calibrations[nodes[0]].Delay.TotalSeconds, 0.75);
            Assert.AreEqual(alphas[1], calibrations[nodes[1]].Scaling, 1e-4);
            Assert.AreEqual(betas[1], calibrations[nodes[1]].Bias, 1e-4);
            Assert.AreEqual(incrs[1], calibrations[nodes[1]].Delay.TotalSeconds, 0.75);
        }

        [Test]
        public void Test7()
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
            TimeSpan[] steps = new TimeSpan[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1) };
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
            Dictionary<DWISNodeID, CalibrationParameters> calibrations = new Dictionary<DWISNodeID, CalibrationParameters>();

            FuseAndCalibrateSignals.ManageCalibrations(values, calibrations);
            Assert.IsTrue(calibrations.ContainsKey(nodes[0]));
            Assert.IsTrue(calibrations.ContainsKey(nodes[1]));
            Assert.AreEqual(alphas[0], calibrations[nodes[0]].Scaling, 1e-4);
            Assert.AreEqual(betas[0], calibrations[nodes[0]].Bias, 1e-4);
            Assert.AreEqual(incrs[0], calibrations[nodes[0]].Delay.TotalSeconds, 0.5);
            Assert.AreEqual(alphas[1], calibrations[nodes[1]].Scaling, 1e-4);
            Assert.AreEqual(betas[1], calibrations[nodes[1]].Bias, 1e-4);
            Assert.AreEqual(incrs[1], calibrations[nodes[1]].Delay.TotalSeconds, 0.5);
        }

        [Test]
        public void Test8()
        {
            Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>>> values = new Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>>>();
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            double[] alphas = new double[] { 2, 1 };
            double[] betas = new double[] { 0, 0 };
            int[] incrs = new int[] { 0, 0 };
            DateTime[] starts = new DateTime[] { DateTime.Now, DateTime.Now };
            TimeSpan[] steps = new TimeSpan[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1) };
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
            Dictionary<DWISNodeID, CalibrationParameters> calibrations = new Dictionary<DWISNodeID, CalibrationParameters>();

            FuseAndCalibrateSignals.ManageCalibrations(values, calibrations);
            Assert.IsTrue(calibrations.ContainsKey(nodes[0]));
            Assert.IsTrue(calibrations.ContainsKey(nodes[1]));
            Assert.AreEqual(alphas[0], calibrations[nodes[0]].Scaling, 1e-4);
            Assert.AreEqual(betas[0], calibrations[nodes[0]].Bias, 1e-4);
            Assert.AreEqual(incrs[0], calibrations[nodes[0]].Delay.TotalSeconds, 0.5);
            Assert.AreEqual(alphas[1], calibrations[nodes[1]].Scaling, 1e-4);
            Assert.AreEqual(betas[1], calibrations[nodes[1]].Bias, 1e-4);
            Assert.AreEqual(incrs[1], calibrations[nodes[1]].Delay.TotalSeconds, 0.5);
        }

        [Test]
        public void Test9()
        {
            Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>>> values = new Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>>>();
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            double[] alphas = new double[] { 2, 1 };
            double[] betas = new double[] { 5, 0 };
            int[] incrs = new int[] { 0, 0 };
            DateTime[] starts = new DateTime[] { DateTime.Now, DateTime.Now };
            TimeSpan[] steps = new TimeSpan[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1) };
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
            Dictionary<DWISNodeID, CalibrationParameters> calibrations = new Dictionary<DWISNodeID, CalibrationParameters>();

            FuseAndCalibrateSignals.ManageCalibrations(values, calibrations);
            Assert.IsTrue(calibrations.ContainsKey(nodes[0]));
            Assert.IsTrue(calibrations.ContainsKey(nodes[1]));
            Assert.AreEqual(alphas[0], calibrations[nodes[0]].Scaling, 1e-4);
            Assert.AreEqual(betas[0], calibrations[nodes[0]].Bias, 1e-4);
            Assert.AreEqual(incrs[0], calibrations[nodes[0]].Delay.TotalSeconds, 0.75);
            Assert.AreEqual(alphas[1], calibrations[nodes[1]].Scaling, 1e-4);
            Assert.AreEqual(betas[1], calibrations[nodes[1]].Bias, 1e-4);
            Assert.AreEqual(incrs[1], calibrations[nodes[1]].Delay.TotalSeconds, 0.75);
        }
        [Test]
        public void Test10()
        {
            Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>>> values = new Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>>>();
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            double[] alphas = new double[] { 2, 1 };
            double[] betas = new double[] { 5, 0 };
            int[] incrs = new int[] { 3, 0 };
            DateTime[] starts = new DateTime[] { DateTime.Now, DateTime.Now - TimeSpan.FromSeconds(3) };
            TimeSpan[] steps = new TimeSpan[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1) };
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
            Dictionary<DWISNodeID, CalibrationParameters> calibrations = new Dictionary<DWISNodeID, CalibrationParameters>();

            FuseAndCalibrateSignals.ManageCalibrations(values, calibrations);
            Assert.IsTrue(calibrations.ContainsKey(nodes[0]));
            Assert.IsTrue(calibrations.ContainsKey(nodes[1]));
            Assert.AreEqual(alphas[0], calibrations[nodes[0]].Scaling, 1e-4);
            Assert.AreEqual(betas[0], calibrations[nodes[0]].Bias, 1e-4);
            Assert.AreEqual(0.0, calibrations[nodes[0]].Delay.TotalSeconds, 0.5);
            Assert.AreEqual(alphas[1], calibrations[nodes[1]].Scaling, 1e-4);
            Assert.AreEqual(betas[1], calibrations[nodes[1]].Bias, 1e-4);
            Assert.AreEqual(incrs[1], calibrations[nodes[1]].Delay.TotalSeconds, 0.5);
        }

        [Test]
        public void Test11()
        {
            Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>>> values = new Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>>>();
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            double[] alphas = new double[] { 2, 1 };
            double[] betas = new double[] { 5, 0 };
            int[] incrs = new int[] { 1, 0 };
            DateTime[] starts = new DateTime[] { DateTime.Now, DateTime.Now };
            TimeSpan[] steps = new TimeSpan[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1) };
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
            Dictionary<DWISNodeID, CalibrationParameters> calibrations = new Dictionary<DWISNodeID, CalibrationParameters>();

            FuseAndCalibrateSignals.ManageCalibrations(values, calibrations);
            Assert.IsTrue(calibrations.ContainsKey(nodes[0]));
            Assert.IsTrue(calibrations.ContainsKey(nodes[1]));
            Assert.AreEqual(alphas[0], calibrations[nodes[0]].Scaling, 0.2);
            Assert.AreEqual(betas[0], calibrations[nodes[0]].Bias, 0.2);
            Assert.AreEqual(1.0, calibrations[nodes[0]].Delay.TotalSeconds, 0.5);
            Assert.AreEqual(alphas[1], calibrations[nodes[1]].Scaling, 1e-4);
            Assert.AreEqual(betas[1], calibrations[nodes[1]].Bias, 1e-4);
            Assert.AreEqual(incrs[1], calibrations[nodes[1]].Delay.TotalSeconds, 0.5);
        }
        [Test]
        public void Test12()
        {
            Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>>> values = new Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>>>();
            double[] amplitudes = new double[] { 1.0, 0.5, 0.7, 1.0, 0.2, 0.8, 0.5, 0.3 };
            double tStep = 1.0;
            double period = 100.0;
            int l = 600;
            int n = 2;
            double[] alphas = new double[] { 2, 1 };
            double[] betas = new double[] { 5, 0 };
            int[] incrs = new int[] { 5, 0 };
            DateTime[] starts = new DateTime[] { DateTime.Now, DateTime.Now };
            TimeSpan[] steps = new TimeSpan[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1) };
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
            Dictionary<DWISNodeID, CalibrationParameters> calibrations = new Dictionary<DWISNodeID, CalibrationParameters>();

            FuseAndCalibrateSignals.ManageCalibrations(values, calibrations);
            Assert.IsTrue(calibrations.ContainsKey(nodes[0]));
            Assert.IsTrue(calibrations.ContainsKey(nodes[1]));
            Assert.AreEqual(alphas[0], calibrations[nodes[0]].Scaling, 0.3);
            Assert.AreEqual(betas[0], calibrations[nodes[0]].Bias, 0.2);
            Assert.AreEqual(5.0, calibrations[nodes[0]].Delay.TotalSeconds, 0.75);
            Assert.AreEqual(alphas[1], calibrations[nodes[1]].Scaling, 1e-4);
            Assert.AreEqual(betas[1], calibrations[nodes[1]].Bias, 1e-4);
            Assert.AreEqual(incrs[1], calibrations[nodes[1]].Delay.TotalSeconds, 0.75);
        }
    }
}
