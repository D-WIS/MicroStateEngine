using DWIS.API.DTO;
using OSDC.DotnetLibraries.General.Common;
using OSDC.DotnetLibraries.Drilling.DrillingProperties;
using OSDC.UnitConversion.Conversion.DrillingEngineering;
using MathNet.Numerics;
using MathNet.Numerics.Optimization;
using MathNet.Numerics.LinearAlgebra.Complex;
using System.Linq;
using Accord.Math.Optimization;
using Accord.Math;
using MathNet.Numerics.Distributions;

namespace DWIS.MicroState.Model
{
    public static class FuseAndCalibrateSignals
    {
        public static TimeSpan MinTimeWindow = TimeSpan.FromSeconds(120);
        public static double CalibrationTimeWindowFactor = 0.5;
        public static double ConvergenceTolerance = 1e-6;
        public static int MaxNumberOfIterations = 1000;

        public static void FuseAndCalibrateData(GaussianDrillingProperty drillProp, Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>> valuesToFuse, Dictionary<DWISNodeID, CalibrationParameters> calibrations, double defaultStandardDeviation)
        {
            if (drillProp != null && valuesToFuse != null)
            {
                List<GaussianDrillingProperty> signals = new List<GaussianDrillingProperty>();
                //manage calibrations
                ManageCalibrations(valuesToFuse, calibrations);
                // find the interpolation time
                DateTime interpolationTime = FindInterpolationTime<GaussianDrillingProperty>(valuesToFuse, calibrations);
                if (interpolationTime == DateTime.MaxValue)
                {
                    foreach (var kpv in valuesToFuse)
                    {
                        if (kpv.Value != null && kpv.Value.Count > 0 && kpv.Value.Peek() != null)
                        {
                            double scaling = 1.0;
                            double bias = 0.0;
                            CalibrationParameters? calibrationParameters = FindCalibration(calibrations, kpv.Key);
                            if (calibrationParameters != null)
                            {
                                scaling = calibrationParameters.Scaling;
                                bias = calibrationParameters.Bias;
                            }
                            GaussianDrillingProperty val = kpv.Value.Peek().Item2;
                            if (val != null)
                            {
                                GaussianDrillingProperty newValue = new GaussianDrillingProperty();
                                newValue.Mean = val.Mean * scaling + bias;
                                newValue.StandardDeviation = val.StandardDeviation * scaling;
                                signals.Add(newValue);
                            }
                        }
                    }
                }
                else
                {
                    // interpolate
                    foreach (var kpv in valuesToFuse)
                    {
                        if (kpv.Value != null && kpv.Value.Count > 0)
                        {
                            double scaling = 1.0;
                            double bias = 0.0;
                            TimeSpan delay = TimeSpan.Zero;
                            CalibrationParameters? calibrationParameters = FindCalibration(calibrations, kpv.Key);
                            if (calibrationParameters != null)
                            {
                                delay = calibrationParameters.Delay;
                                scaling = calibrationParameters.Scaling;
                                bias = calibrationParameters.Bias;
                            }
                            GaussianDrillingProperty? newValue = Interpolate(kpv.Value, interpolationTime, delay, scaling, bias);
                            if (newValue != null)
                            {
                                signals.Add(newValue);
                            }
                        }
                    }
                }
                // fuse the values
                if (signals.Count > 0)
                {
                    double meanSum = 0;
                    double invVarSum = 0;
                    foreach (var signal in signals)
                    {
                        if (signal.GaussianValue != null)
                        {
                            double? mean = signal.GaussianValue.Mean;
                            if (mean != null)
                            {
                                double? stdDev = signal.GaussianValue.StandardDeviation;
                                if (stdDev == null)
                                {
                                    stdDev = defaultStandardDeviation;
                                }
                                invVarSum += 1.0 / (stdDev.Value * stdDev.Value);
                                meanSum += mean.Value / (stdDev.Value * stdDev.Value);
                            }
                        }
                    }
                    if (!Numeric.EQ(invVarSum, 0))
                    {
                        drillProp.Mean = meanSum / invVarSum;
                        drillProp.StandardDeviation = 1.0 / Numeric.SqrtEqual(invVarSum);
                    }
                }
            }
        }
        public static void FuseAndCalibrateData(BernoulliDrillingProperty drillProp, Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, BernoulliDrillingProperty>>> valuesToFuse, Dictionary<DWISNodeID, CalibrationParameters> calibrations, double defaultProbability)
        {
            if (drillProp != null)
            {
                List<BernoulliDrillingProperty> signals = new List<BernoulliDrillingProperty>();
                // manage calibrations
                ManageCalibrations(valuesToFuse, calibrations);

                // find the interpolation time
                DateTime interpolationTime = FindInterpolationTime<BernoulliDrillingProperty>(valuesToFuse, calibrations);
                if (interpolationTime == DateTime.MaxValue)
                {
                    foreach (var kpv in valuesToFuse)
                    {
                        if (kpv.Value != null && kpv.Value.Count > 0 && kpv.Value.Peek() != null)
                        {
                            double scaling = 1.0;
                            double bias = 0.0;
                            CalibrationParameters? calibrationParameters = FindCalibration(calibrations, kpv.Key);
                            if (calibrationParameters != null)
                            {
                                scaling = calibrationParameters.Scaling;
                                bias = calibrationParameters.Bias;
                            }
                            BernoulliDrillingProperty val = kpv.Value.Peek().Item2;
                            if (val != null)
                            {
                                BernoulliDrillingProperty newValue = new BernoulliDrillingProperty();
                                newValue.Probability = val.Probability;
                                signals.Add(newValue);
                            }
                        }
                    }
                }
                else
                {
                    // interpolate
                    foreach (var kpv in valuesToFuse)
                    {
                        if (kpv.Value != null && kpv.Value.Count > 0)
                        {
                            double scaling = 1.0;
                            double bias = 0.0;
                            TimeSpan delay = TimeSpan.Zero;
                            CalibrationParameters? calibrationParameters = FindCalibration(calibrations, kpv.Key);
                            if (calibrationParameters != null)
                            {
                                delay = calibrationParameters.Delay;
                                scaling = calibrationParameters.Scaling;
                                bias = calibrationParameters.Bias;
                            }
                            BernoulliDrillingProperty? newValue = Interpolate(kpv.Value, interpolationTime, delay);
                            if (newValue != null)
                            {
                                signals.Add(newValue);
                            }
                        }
                    }
                }

                // fuse the values
                if (signals.Count > 0)
                {
                    double sumProbability = 0;
                    double productProbability = 1;
                    foreach (var signal in signals)
                    {
                        if (signal.BernoulliValue != null)
                        {
                            double? probability = signal.BernoulliValue.Probability;
                            if (probability == null)
                            {
                                probability = defaultProbability;
                            }
                            sumProbability += probability.Value;
                            productProbability *= probability.Value;
                        }
                    }
                    double fusedProbability = sumProbability - productProbability;
                    drillProp.Probability = fusedProbability;
                }
            }
        }
        public static void FuseAndCalibrateData(ScalarDrillingProperty drillProp, Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>>> valuesToFuse, Dictionary<DWISNodeID, CalibrationParameters> calibrations)
        {
            if (drillProp != null)
            {
                List<ScalarDrillingProperty> signals = new List<ScalarDrillingProperty>();
                // manage calibrations
                ManageCalibrations(valuesToFuse, calibrations);

                // find the interpolation time
                DateTime interpolationTime = FindInterpolationTime<ScalarDrillingProperty>(valuesToFuse, calibrations);
                foreach (var kpv in valuesToFuse)
                {
                    if (kpv.Value != null && kpv.Value.Count > 0)
                    {
                        var data = kpv.Value.Peek();
                        if (data != null && data.Item2 != null)
                        {
                            DateTime lastTimeStamp = data.Item1;
                            // find the calibrated delay
                            TimeSpan delay = TimeSpan.Zero;
                            if (calibrations != null && calibrations.ContainsKey(kpv.Key))
                            {
                                CalibrationParameters calibrationParameters = calibrations[kpv.Key];
                                if (calibrationParameters != null)
                                {
                                    delay = calibrationParameters.Delay;
                                }
                            }
                            if (lastTimeStamp < interpolationTime)
                            {
                                interpolationTime = lastTimeStamp + delay;
                            }
                        }
                    }
                }
                if (interpolationTime == DateTime.MaxValue)
                {
                    foreach (var kpv in valuesToFuse)
                    {
                        if (kpv.Value != null && kpv.Value.Count > 0 && kpv.Value.Peek() != null)
                        {
                            double scaling = 1.0;
                            double bias = 0.0;
                            CalibrationParameters? calibrationParameters = FindCalibration(calibrations, kpv.Key);
                            if (calibrationParameters != null)
                            {
                                scaling = calibrationParameters.Scaling;
                                bias = calibrationParameters.Bias;
                            }
                            ScalarDrillingProperty val = kpv.Value.Peek().Item2;
                            if (val != null)
                            {
                                ScalarDrillingProperty newValue = new ScalarDrillingProperty();
                                newValue.ScalarValue = val.ScalarValue * scaling + bias;
                                signals.Add(newValue);
                            }
                        }
                    }
                }
                else
                {
                    // interpolate
                    foreach (var kpv in valuesToFuse)
                    {
                        if (kpv.Value != null && kpv.Value.Count > 0)
                        {
                            double scaling = 1.0;
                            double bias = 0.0;
                            TimeSpan delay = TimeSpan.Zero;
                            CalibrationParameters? calibrationParameters = FindCalibration(calibrations, kpv.Key);
                            if (calibrationParameters != null)
                            {
                                delay = calibrationParameters.Delay;
                                scaling = calibrationParameters.Scaling;
                                bias = calibrationParameters.Bias;
                            }
                            ScalarDrillingProperty? newValue = Interpolate(kpv.Value, interpolationTime, delay, scaling, bias);
                            if (newValue != null)
                            {
                                signals.Add(newValue);
                            }
                        }
                    }
                }

                // fuse the values
                if (signals.Count > 0)
                {
                    double sum = 0;
                    int count = 0;
                    foreach (var signal in signals)
                    {
                        if (signal.ScalarValue != null)
                        {
                            sum += signal.ScalarValue.Value;
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        drillProp.ScalarValue = sum / count;
                    }
                }
            }
        }

        public static void ManageCalibrations(Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>> valuesToFuse, Dictionary<DWISNodeID, CalibrationParameters> calibrations)
        {
            if (valuesToFuse != null && calibrations != null)
            {
                foreach (var kvp in valuesToFuse)
                {
                    if (!calibrations.ContainsKey(kvp.Key))
                    {
                        calibrations.Add(kvp.Key, new CalibrationParameters() { Scaling = 1.0, Bias = 0.0, Delay = TimeSpan.Zero });
                    }
                }
                // Reinterpolate all the signals with a constant time step and such that the timestamps are synchrone for a common time interval
                // find the min and max timestamps
                DateTime minTimeStamp = DateTime.MinValue;
                DateTime maxTimeStamp = DateTime.MaxValue;
                GetMinMaxTimeStamps(valuesToFuse, out minTimeStamp, out maxTimeStamp);
                if (minTimeStamp != DateTime.MinValue && maxTimeStamp != DateTime.MaxValue && minTimeStamp < maxTimeStamp && (maxTimeStamp - minTimeStamp) >= MinTimeWindow)
                {
                    TimeSpan timeStep = TimeSpan.FromSeconds(1);
                    Dictionary<DWISNodeID, double[]> reinteroplatedData = new Dictionary<DWISNodeID, double[]>();
                    Reinterpolate(valuesToFuse, minTimeStamp, maxTimeStamp, timeStep, reinteroplatedData);

                    // apply an optimization algorithm to minimize the chi-square on a suitable time-window centered on
                    Optimize(reinteroplatedData, calibrations, timeStep);
                }
            }
        }

        public static void ManageCalibrations(Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, BernoulliDrillingProperty>>> valuesToFuse, Dictionary<DWISNodeID, CalibrationParameters> calibrations)
        {
            foreach (var kvp in valuesToFuse)
            {
                if (!calibrations.ContainsKey(kvp.Key))
                {
                    calibrations.Add(kvp.Key, new CalibrationParameters() { Scaling = 1.0, Bias = 0.0, Delay = TimeSpan.Zero });
                }
            }
        }

        public static void ManageCalibrations(Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>>> valuesToFuse, Dictionary<DWISNodeID, CalibrationParameters> calibrations)
        {
            foreach (var kvp in valuesToFuse)
            {
                if (!calibrations.ContainsKey(kvp.Key))
                {
                    calibrations.Add(kvp.Key, new CalibrationParameters() { Scaling = 1.0, Bias = 0.0, Delay = TimeSpan.Zero });
                }
            }
            // Reinterpolate all the signals with a constant time step and such that the timestamps are synchrone for a common time interval
            // find the min and max timestamps
            DateTime minTimeStamp = DateTime.MinValue;
            DateTime maxTimeStamp = DateTime.MaxValue;
            GetMinMaxTimeStamps(valuesToFuse, out minTimeStamp, out maxTimeStamp);
            if (minTimeStamp != DateTime.MinValue && maxTimeStamp != DateTime.MaxValue && minTimeStamp < maxTimeStamp && (maxTimeStamp - minTimeStamp) >= MinTimeWindow)
            {
                TimeSpan timeStep = TimeSpan.FromSeconds(1);
                Dictionary<DWISNodeID, double[]> reinteroplatedData = new Dictionary<DWISNodeID, double[]>();
                Reinterpolate(valuesToFuse, minTimeStamp, maxTimeStamp, timeStep, reinteroplatedData);

                // apply an optimization algorithm to minimize the chi-square on a suitable time-window centered on
                Optimize(reinteroplatedData, calibrations, timeStep);
            }
        }

        public static DateTime FindInterpolationTime<T>(Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, T>>> valuesToFuse, Dictionary<DWISNodeID, CalibrationParameters> calibrations)
        {
            DateTime interpolationTime = DateTime.MaxValue;
            foreach (var kpv in valuesToFuse)
            {
                if (kpv.Value != null && kpv.Value.Count > 0)
                {
                    var data = kpv.Value.Peek();
                    if (data != null && data.Item2 != null)
                    {
                        DateTime lastTimeStamp = data.Item1;
                        // find the calibrated delay
                        TimeSpan delay = TimeSpan.Zero;
                        CalibrationParameters? calibrationParameters = FindCalibration(calibrations, kpv.Key);
                        if (calibrationParameters != null && calibrations.ContainsKey(kpv.Key))
                        {
                            delay = calibrationParameters.Delay;
                        }
                        if (lastTimeStamp < interpolationTime)
                        {
                            interpolationTime = lastTimeStamp + delay;
                        }
                    }
                }
            }
            return interpolationTime;
        }

        public static CalibrationParameters? FindCalibration(Dictionary<DWISNodeID, CalibrationParameters>? calibrations, DWISNodeID? nodeID )
        {
            if (calibrations != null && nodeID != null && calibrations.ContainsKey(nodeID))
            {
                CalibrationParameters calibrationParameters = calibrations[nodeID];
                return calibrationParameters;
            }
            return null;
        }

        public static GaussianDrillingProperty? Interpolate(CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>> data, DateTime interpolationTime, TimeSpan delay, double scaling, double bias)
        {
            GaussianDrillingProperty? result = null;
            IEnumerable<Tuple<DateTime, GaussianDrillingProperty>> items = data.GetItems();
            interpolationTime += delay;
            if (items != null)
            {
                Tuple<DateTime, GaussianDrillingProperty>? v1 = null;
                Tuple<DateTime, GaussianDrillingProperty>? v2 = null;
                foreach (var item in items)
                {
                    if (item.Item1 <= interpolationTime)
                    {
                        v1 = item;
                    }
                    else
                    {
                        v2 = item;
                        break;
                    }
                }
                if (v1 != null && v2 != null)
                {
                    double factor = 0;
                    if (v2.Item1 > v1.Item1)
                    {
                        factor = (interpolationTime - v1.Item1).TotalSeconds / (v2.Item1 - v1.Item1).TotalSeconds;
                    }
                    result = new GaussianDrillingProperty();
                    result.Mean = bias + scaling * (v1.Item2.Mean + factor * (v2.Item2.Mean - v1.Item2.Mean));
                    result.StandardDeviation = v1.Item2.StandardDeviation * scaling;
                }
                if (result == null)
                {
                    v1 = data.Peek();
                    if (v1 != null && v1.Item2 != null)
                    {
                        result = new GaussianDrillingProperty();
                        result.Mean = bias + v1.Item2.Mean * scaling;
                        result.StandardDeviation = v1.Item2.StandardDeviation * scaling;
                    }
                }
            }
            return result;
        }

        public static BernoulliDrillingProperty? Interpolate(CircularBuffer<Tuple<DateTime, BernoulliDrillingProperty>> data, DateTime interpolationTime, TimeSpan delay)
        {
            BernoulliDrillingProperty? result = null;
            IEnumerable<Tuple<DateTime, BernoulliDrillingProperty>> items = data.GetItems();
            interpolationTime += delay;
            if (items != null)
            {
                Tuple<DateTime, BernoulliDrillingProperty>? v1 = null;
                Tuple<DateTime, BernoulliDrillingProperty>? v2 = null;
                foreach (var item in items)
                {
                    if (item.Item1 <= interpolationTime)
                    {
                        v1 = item;
                    }
                    else
                    {
                        v2 = item;
                        break;
                    }
                }
                if (v1 != null && v2 != null)
                {
                    double factor = 0;
                    if (v2.Item1 > v1.Item1)
                    {
                        factor = (interpolationTime - v1.Item1).TotalSeconds / (v2.Item1 - v1.Item1).TotalSeconds;
                    }
                    result = new BernoulliDrillingProperty();
                    result.Probability = v1.Item2.Probability + factor * (v2.Item2.Probability - v1.Item2.Probability);
                }
                if (result == null)
                {
                    v1 = data.Peek();
                    if (v1 != null && v1.Item2 != null)
                    {
                        result = new BernoulliDrillingProperty();
                        result.Probability = v1.Item2.Probability;
                    }
                }
            }
            return result;
        }

        public static ScalarDrillingProperty? Interpolate(CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>> data, DateTime interpolationTime, TimeSpan delay, double scaling, double bias)
        {
            ScalarDrillingProperty? result = null;
            IEnumerable<Tuple<DateTime, ScalarDrillingProperty>> items = data.GetItems();
            interpolationTime += delay;
            if (items != null)
            {
                Tuple<DateTime, ScalarDrillingProperty>? v1 = null;
                Tuple<DateTime, ScalarDrillingProperty>? v2 = null;
                foreach (var item in items)
                {
                    if (item.Item1 <= interpolationTime)
                    {
                        v1 = item;
                    }
                    else
                    {
                        v2 = item;
                        break;
                    }
                }
                if (v1 != null && v2 != null)
                {
                    double factor = 0;
                    if (v2.Item1 > v1.Item1)
                    {
                        factor = (interpolationTime - v1.Item1).TotalSeconds / (v2.Item1 - v1.Item1).TotalSeconds;
                    }
                    result = new ScalarDrillingProperty();
                    result.ScalarValue = bias + scaling * (v1.Item2.ScalarValue + factor * (v2.Item2.ScalarValue - v1.Item2.ScalarValue));
                }
                if (result == null)
                {
                    v1 = data.Peek();
                    if (v1 != null && v1.Item2 != null)
                    {
                        result = new ScalarDrillingProperty();
                        result.ScalarValue = bias + v1.Item2.ScalarValue * scaling;
                    }
                }
            }
            return result;
        }

        public static void GetMinMaxTimeStamps<T>(Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, T>>> valuesToFuse, out DateTime minTimeStamp, out DateTime maxTimeStamp)
        {
            minTimeStamp = DateTime.MinValue;
            maxTimeStamp = DateTime.MaxValue;
            foreach (var kvp in valuesToFuse)
            {
                if (kvp.Value != null)
                {
                    DateTime min = DateTime.MaxValue;
                    DateTime max = DateTime.MinValue;
                    foreach (var tuple in kvp.Value.GetItems())
                    {
                        if (tuple.Item1 < min)
                        {
                            min = tuple.Item1;
                        }
                        if (tuple.Item1 > max)
                        {
                            max = tuple.Item1;
                        }
                    }
                    if (min > minTimeStamp)
                    {
                        minTimeStamp = min;
                    }
                    if (max < maxTimeStamp)
                    {
                        maxTimeStamp = max;
                    }
                }
            }
        }
        public static void Reinterpolate(Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, GaussianDrillingProperty>>> valuesToFuse, DateTime minTimeStamp, DateTime maxTimeStamp, TimeSpan timeStep, Dictionary<DWISNodeID, double[]> reinterpolated)
        {
            if (valuesToFuse != null && reinterpolated != null && minTimeStamp < maxTimeStamp && timeStep > TimeSpan.Zero)
            {
                int count = (int)((maxTimeStamp - timeStep - minTimeStamp).TotalSeconds / timeStep.TotalSeconds);
                if (count >= MinTimeWindow.TotalSeconds)
                {
                    foreach (var kvp in valuesToFuse)
                    {
                        if (kvp.Value != null)
                        {
                            IEnumerable<Tuple<DateTime, GaussianDrillingProperty>> values = kvp.Value.GetItems();
                            if (values != null)
                            {
                                int i = 0;
                                DateTime previousTimeStamp = DateTime.MinValue;
                                double previousValue = 0;
                                double[] vals = new double[count];
                                foreach (var tuple in values)
                                {
                                    if (tuple.Item2 != null && tuple.Item2.GaussianValue != null && tuple.Item2.GaussianValue.Mean != null && Numeric.GE(tuple.Item1, minTimeStamp, 0.5) && Numeric.LE(tuple.Item1, maxTimeStamp, 0.5) && i < count)
                                    {
                                        DateTime timeStamp = minTimeStamp + i * timeStep;
                                        while (Numeric.LT(timeStamp, tuple.Item1, 0.5))
                                        {
                                            if (previousTimeStamp != DateTime.MinValue)
                                            {
                                                double factor = (timeStamp - previousTimeStamp).TotalSeconds / (tuple.Item1- previousTimeStamp).TotalSeconds;
                                                vals[i] = previousValue + factor * (tuple.Item2.GaussianValue.Mean.Value - previousValue);
                                            }
                                            else
                                            {
                                                vals[i] = tuple.Item2.GaussianValue.Mean.Value;
                                            }
                                            i++;
                                            timeStamp += timeStep;
                                        } 
                                        previousTimeStamp = tuple.Item1;
                                        previousValue = tuple.Item2.GaussianValue.Mean.Value;
                                    }
                                }
                                if (i != count)
                                {
                                    bool glups = true;
                                }
                                reinterpolated.Add(kvp.Key, vals);
                            }
                        }
                    }
                }
            }
        }
        public static void Reinterpolate(Dictionary<DWISNodeID, CircularBuffer<Tuple<DateTime, ScalarDrillingProperty>>> valuesToFuse, DateTime minTimeStamp, DateTime maxTimeStamp, TimeSpan timeStep, Dictionary<DWISNodeID, double[]> reinterpolated)
        {
            if (valuesToFuse != null && reinterpolated != null && minTimeStamp < maxTimeStamp && timeStep > TimeSpan.Zero)
            {
                int count = (int)((maxTimeStamp - timeStep - minTimeStamp).TotalSeconds / timeStep.TotalSeconds);
                if (count >= MinTimeWindow.TotalSeconds)
                {
                    foreach (var kvp in valuesToFuse)
                    {
                        if (kvp.Value != null)
                        {
                            IEnumerable<Tuple<DateTime, ScalarDrillingProperty>> values = kvp.Value.GetItems();
                            if (values != null)
                            {
                                int i = 0;
                                DateTime previousTimeStamp = DateTime.MinValue;
                                double previousValue = 0;
                                double[] vals = new double[count];
                                foreach (var tuple in values)
                                {
                                    if (tuple.Item2 != null && tuple.Item2.ScalarValue != null && Numeric.GE(tuple.Item1, minTimeStamp, 0.5) && Numeric.LE(tuple.Item1, maxTimeStamp, 0.5) && i < count)
                                    {
                                        DateTime timeStamp = minTimeStamp + i * timeStep;
                                        while (timeStamp < tuple.Item1)
                                        {
                                            if (previousTimeStamp != DateTime.MinValue)
                                            {
                                                double factor = (timeStamp - previousTimeStamp).TotalSeconds / (tuple.Item1 - previousTimeStamp).TotalSeconds;
                                                vals[i] = previousValue + factor * (tuple.Item2.ScalarValue.Value - previousValue);
                                            }
                                            else
                                            {
                                                vals[i] = tuple.Item2.ScalarValue.Value;
                                            }
                                            i++;
                                            timeStamp += timeStep;
                                        }
                                        previousTimeStamp = tuple.Item1;
                                        previousValue = tuple.Item2.ScalarValue.Value;
                                    }
                                }
                                if (i != count)
                                {
                                    bool glups = true;
                                }
                                reinterpolated.Add(kvp.Key, vals);
                            }
                        }
                    }
                }
            }
        }

        public static void Optimize(Dictionary<DWISNodeID, double[]> reinterpolated, Dictionary<DWISNodeID, CalibrationParameters> calibrations, TimeSpan timeStep)
        {
            if (reinterpolated != null && calibrations != null && reinterpolated.Count >= 2)
            {
                List<double> guesses = new List<double>();
                foreach ( var kvp in reinterpolated )
                {
                    if (!calibrations.ContainsKey(kvp.Key))
                    {
                        calibrations.Add(kvp.Key, new CalibrationParameters() { Scaling = 1.0, Bias = 0, Delay = TimeSpan.Zero });
                    }
                    var parameters = calibrations[kvp.Key];
                    guesses.Add(parameters.Scaling);
                    guesses.Add(parameters.Bias);
                    guesses.Add(parameters.Delay.TotalSeconds/timeStep.TotalSeconds);
                }
                // remove the last 3 values
                guesses.RemoveAt(guesses.Count - 1);
                guesses.RemoveAt(guesses.Count - 1);
                guesses.RemoveAt(guesses.Count - 1);
                double[] initialGuess = guesses.ToArray();

                int n = reinterpolated.Count;
                KeyValuePair<DWISNodeID, double[]>[] array = reinterpolated.ToArray();
                var first = array[0];
                int l = 0;
                if (first.Value != null)
                {
                    l = first.Value.Length;
                }
                int m = (int)(CalibrationTimeWindowFactor * l); // width of window
                int k_min = l / 2 - m / 2; // first index in the window if not delays
                LeastSquaresFunction function = (double[] parameters, double[] input) => { return ChiSquare(parameters, input, n, l, m, k_min); };
                LeastSquaresGradientFunction gradient = (double[] parameters, double[] input, double[] result) => { ChiSquarePartialDerivatives(parameters, input, result, n, l, m, k_min); };
                var optimizer = new LevenbergMarquardt(initialGuess.Length);
                optimizer.MaxIterations = MaxNumberOfIterations;
                optimizer.Tolerance = ConvergenceTolerance;
                optimizer.Function = function;
                optimizer.Gradient = gradient;
                optimizer.Solution = initialGuess;
                double[][] inputs = new double[1][];
                inputs[0] = new double[n * l];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < l; j++)
                    {
                        inputs[0][i * l + j] = array[i].Value[j];
                    }
                }
                double[] outputs = new double[] { 0 };
                double val = optimizer.Minimize(inputs, outputs);
                if (optimizer.HasConverged)
                {
                    int i = 0;
                    foreach (var kvp in calibrations)
                    {
                        if (i == calibrations.Count-1)
                        {
                            kvp.Value.Scaling = 1;
                            kvp.Value.Bias = 0;
                            kvp.Value.Delay = TimeSpan.Zero;
                        }
                        else
                        {
                            kvp.Value.Scaling = optimizer.Solution[3 * i + 0];
                            kvp.Value.Bias = optimizer.Solution[3 * i + 1];
                            kvp.Value.Delay = optimizer.Solution[3 * i + 2] * timeStep;
                        }
                        i++;
                    }
                }
            }
        }

        public static double ChiSquare(double[] parameters, double[] inputs, int n, int l, int m, int k_min)
        {
            double chiSquare = 0;
            if (inputs != null && parameters != null && n * l == inputs.Length)
            {
                double[] alphas = new double[n];
                double[] betas = new double[n];
                int[] ks = new int[n];
                for (int i = 0; i < n - 1; i++)
                {
                    alphas[i] = parameters[3 * i + 0];
                    betas[i] = parameters[3 * i + 1];
                    ks[i] = (int)parameters[3 * i + 2];
                }
                alphas[n - 1] = 1;
                betas[n - 1] = 0;
                ks[n - 1] = 0;
                for (int k = k_min; k < k_min + m; k++)
                {
                    for (int i = 0; i < n - 1; i++)
                    {
                        for (int j = i + 1; j < n; j++)
                        {
                            int ki = Math.Min(l - 1, Math.Max(0, k - ks[i]));
                            int kj = Math.Min(l - 1, Math.Max(0, k - ks[j]));
                            double a = (inputs[i * l + ki] - betas[i]) / alphas[i] - (inputs[j * l + kj] - betas[j]) / alphas[j];
                            chiSquare += a * a;
                        }
                    }
                }
            }
            return chiSquare;
        }

        public static void ChiSquarePartialDerivatives(double[] parameters, double[] inputs, double[] result, int n, int l, int m, int k_min)
        {
            if (n > 1)
            {
                double[] alphas = new double[n];
                double[] betas = new double[n];
                int[] ks = new int[n];
                for (int i = 0; i < n - 1; i++)
                {
                    alphas[i] = parameters[3 * i + 0];
                    betas[i] = parameters[3 * i + 1];
                    ks[i] = (int)parameters[3 * i + 2];
                }
                alphas[n - 1] = 1;
                betas[n - 1] = 0;
                ks[n - 1] = 0;
                double chiSquare = ChiSquare(parameters, inputs, n, l, m, k_min);
                for (int i = 0; i < n-1; i++)
                {

                    double dchiOverdAlpha = 0;
                    double dchiOverdBeta = 0;
                    double dchiOverdt = 0;

                    for (int k = k_min; k < k_min + m; k++)
                    {
                        for (int j = i + 1; j < n; j++)
                        {
                            int ki = Math.Min(l - 1, Math.Max(0, k - ks[i]));
                            int kj = Math.Min(l - 1, Math.Max(0, k - ks[j]));
                            double factor = (inputs[i * l + ki] - betas[i]) / alphas[i] - (inputs[j * l + kj] - betas[j]) / alphas[j];
                            dchiOverdAlpha += -2 * (inputs[i * l + ki] - betas[i]) * factor / (alphas[i] * alphas[i]);
                            dchiOverdBeta += -2 * factor / alphas[i];
                            double slope = 0;
                            if (ki > 0 && ki < l-2)
                            {
                                slope = -(inputs[i * l + ki + 1] - inputs[i * l + ki - 1]) / 2.0;
                            }
                            else if (ki > 0)
                            {
                                slope = -(inputs[i * l + ki] - inputs[i * l + ki - 1]) / 1.0;
                            }
                            else if (ki < l - 2)
                            {
                                slope = -(inputs[i * l + ki + 1] - inputs[i * l + ki]) / 1.0;
                            }
                            dchiOverdt += 2.0 * slope * factor / alphas[i];
                        }
                    }        
                    result[3 * i + 0] = dchiOverdAlpha;
                    result[3 * i + 1] = dchiOverdBeta;
                    result[3 * i + 2] = dchiOverdt;
                }
            }
        }
    }
}
