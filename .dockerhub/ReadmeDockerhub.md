# DWIS Microstates Interpretation Engine

## Context
This work is performed as part of the Drilling and Wells Interoperability Standard (D-WIS) sub-commmittee. D-WIS is a
subcommittee of the Society of Petroleum Engineer's Drilling Automation Technical Section.

## Prerequisites
The `DWIS Microstates Interpretation Engine` makes use of the `DWIS Blackboard`. So you shall install the `DWIS Blackboard` before installing
the `DWIS Microstates Interpretation Engine`.

## Getting started
The `docker run` command for windows is:
```
docker run --name microstatesengine -v C:\Volumes\DWISMicroStateSignalGenerator:/home digiwells/dwismicrostateinterpretationengine
```
where `C:\Volumes\DWISMicroStateSignalGenerator` is any folder where you would like to access the config.json file that is used to configure
the application.

and the `docker run` command for linux is:
```
docker run --name microstatesengine -v /home/Volumes/DWISMicroStateSignalGenerator:/home digiwells/dwismicrostateinterpretationengine
```
where `/home/Volumes/DWISMicroStateSignalGenerator` is any directory where you would like to access the config.json file that is used to
configure the application.

## Configuration
A configuration file is available in the directory/folder that is connected to the internal `/home` directory. The name of the configuration
file is `config.json` and is in Json format.

The configuration file has the following properties:
- `LoopDuration` (a TimeSpan, default 1s): this property defines the loop duration of the service, i.e., the time interval used to check if new signals are available.
- `OPCUAURL` (a string, default "opc.tcp://localhost:48030"): this property defines the `URL` used to connect to the `DWIS Blackboard`
- `DefaultProbability` (a double, default 0.1): this property defines the default probability for Bernoulli Drilling Properties for which the 
probability has not been defined.
- `DefaultStandardDeviation` (a double, default 0.1): this property defines the default standard deviation for Gaussian Drilling Properties
for which the standard deviation has not been informed.
- `CircularBufferSize` (an integer, default 300): this property defines the maximum number of elements in circular buffer that are used by
the digital twin calibration algorithm.
- `CalibrationMinTimeWindow` (a TimeSpan, default 120s): this property defines the minimum time window that is acceptable to start doing
calibration of digital twin signals.
- `CalibrationTimeWindowFactor` (a double, default 0.5): this property defines the proportion of data that is used in circular buffer to calibrate
the signals from multiple digital twins.
- `CalibrationConvergenceTolerance` (a double, default 1e-6): this property defines the tolerance of the `Levenberg-Marquardt` algorithm
used to calibrate the signals from multiple digital twins.
- `CalibrationMaxNumberOfIterations` (an integer, default 1000): this property defines the maximum number of iterations that are acceptable
while running the `Levenberg-Marquardt` algorithm used to calibrate the signals from multiple digital twins.
- `GenerateRandomValues` (a boolean, default false): this property is used to tell the `DWIS Microstates Interpretation Engine` to generate
randome values.

## Using the Microstates Interpretation Engine
The `DWIS Microstates Interpretation Engine` searches for `SignalGroup` on the `DWIS Blackboard`. If any are found, then they are used
to estimate the drilling process microstates. It also searches for `Thresholds` as they are used in the logical statements that define
the drilling process state. If there are several `SignalGroup` available on the `DWIS Blackboard`, the signals are first calibrated by optimizing
a scaling and a bias factors as well as a time delay such that they are all of the same order of magnitude. Then the signals are fused using
the informed standard deviation provided by each of the digital twins for each of the signals. It is the calibrated and fused value that
is used in the logical expressions that define the microstates.

The `DWIS Microstates Interpretation Engine` outputs on the `DWIS Blackboard` the calibrated and fused `SignalGroup` that is used for the
estimation of the microstates. It also publishes on the `DWIS Blackboard` a deterministic `MicroStates` and a probabilistic `MicroStates`.

The Json schemas for the different objects manipulated by the `DWIS Microstates Interpretation Engine` can be found here:
https://github.com/D-WIS/MicroStateEngine/blob/main/DWIS.MicroState.JsonSchema/MicroStates.json.


The Sparql query used to retrieve the `SignalGroups` is:
```sparql
PREFIX rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub:<http://ddhub.no/>
PREFIX quantity:<http://ddhub.no/UnitAndQuantity>

SELECT ?DigitalTwinSignalsForMicroStates
WHERE {
	?DigitalTwinSignalsForMicroStates_01 rdf:type ddhub:DigitalTwinAdvice .
	?DigitalTwinSignalsForMicroStates_01 ddhub:HasDynamicValue ?DigitalTwinSignalsForMicroStates .
	?DigitalTwin rdf:type ddhub:Simulator .
	?DigitalTwinSignalsForMicroStates_01 ddhub:IsRecommendedBy ?DigitalTwin .
	?MicroStateInterpreter rdf:type ddhub:DWISDrillingProcessStateInterpreter .
	?DigitalTwinSignalsForMicroStates_01 ddhub:IsProvidedTo ?MicroStateInterpreter .
}

```

The Sparql query used to retrieve the `Thresholds` is:
```sparql
PREFIX rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub:<http://ddhub.no/>
PREFIX quantity:<http://ddhub.no/UnitAndQuantity>

SELECT ?MicroStateThresholds
WHERE {
	?MicroStateThresholds_01 rdf:type ddhub:ConfigurationData .
	?MicroStateThresholds_01 ddhub:HasDynamicValue ?MicroStateThresholds .
	?MicroStateInterpreter rdf:type ddhub:DWISDrillingProcessStateInterpreter .
	?MicroStateThresholds_01 ddhub:IsProvidedTo ?MicroStateInterpreter .
	?MicroStateThresholds_01 ddhub:IsLimitFor ?MicroStateInterpreter .
}

```

The Sparql query to retrieve the calibrated and fused `FusedSignalGroup` is:
```sparql
PREFIX rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub:<http://ddhub.no/>
PREFIX quantity:<http://ddhub.no/UnitAndQuantity>

SELECT ?FusedSignalsForMicroStates
WHERE {
	?FusedSignalsForMicroStates_01 rdf:type ddhub:DigitalTwinAdvice .
	?FusedSignalsForMicroStates_01 ddhub:HasDynamicValue ?FusedSignalsForMicroStates .
	?MicroStateInterpreter rdf:type ddhub:DWISDrillingProcessStateInterpreter .
	?FusedSignalsForMicroStates_01 ddhub:IsProvidedBy ?MicroStateInterpreter .
	?SignalFusionInterpreter_01 rdf:type ddhub:Interpreter .
	?SignalFusionInterpreter_01 rdf:type ddhub:DescriptiveModel .
	?SignalFusionInterpreter_01 rdf:type ddhub:ForwardModel .
	?SignalFusionInterpreter_01 rdf:type ddhub:WhiteBoxModel .
	?SignalFusionInterpreter_01 rdf:type ddhub:SpecializedModel .
	?SignalFusionInterpreter_01 rdf:type ddhub:EmpiricalModel .
	?SignalFusionInterpreter_01 rdf:type ddhub:StochasticModel .
	?FusedSignalsForMicroStates_01 ddhub:IsComputedBy ?SignalFusionInterpreter_01 .
	?DigitalTwinSignals_01 rdf:type ddhub:DigitalTwinAdvice .
	?DigitalTwin rdf:type ddhub:Simulator .
	?DigitalTwinSignals_01 ddhub:IsRecommendedBy ?DigitalTwin .
	?DigitalTwinSignals_01 ddhub:IsProvidedTo ?MicroStateInterpreter_01 .
	?DigitalTwinSignals_01 ddhub:IsComputationInput ?SignalFusionInterpreter_01 .
	?DigitalTwinSignalsForMicroStates_01 rdf:type ddhub:DigitalTwinAdvice .
	?DigitalTwinSignalsForMicroStates_01 ddhub:HasDynamicValue ?DigitalTwinSignalsForMicroStates .
	?DigitalTwin rdf:type ddhub:Simulator .
	?DigitalTwinSignalsForMicroStates_01 ddhub:IsRecommendedBy ?DigitalTwin .
	?MicroStateInterpreter rdf:type ddhub:DWISDrillingProcessStateInterpreter .
	?DigitalTwinSignalsForMicroStates_01 ddhub:IsProvidedTo ?MicroStateInterpreter .
}

```

The Sparql query used to retrieve the deterministic `MicroStates` is:
```sparql
PREFIX rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub:<http://ddhub.no/>
PREFIX quantity:<http://ddhub.no/UnitAndQuantity>

SELECT ?DeterministicState
WHERE {
	?DeterministicState_01 rdf:type ddhub:ComputedData .
	?DeterministicState_01 rdf:type ddhub:JSonDataType .
	?DeterministicState_01 ddhub:HasDynamicValue ?DeterministicState .
	?DeterministicProcessState rdf:type ddhub:ProcessState .
	?DeterministicProcessState rdf:type ddhub:DeterministicModel .
	?DeterministicState_01 ddhub:IsGeneratedBy ?DeterministicProcessState .
	?ProcessStateInterpreter_01 rdf:type ddhub:DWISDrillingProcessStateInterpreter .
	?DeterministicState_01 ddhub:IsProvidedBy ?ProcessStateInterpreter_01 .
}

```

The Spartql query used to retrieve the probabilistic `ProbabilisticMicroStates` is:
```sparql
PREFIX rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub:<http://ddhub.no/>
PREFIX quantity:<http://ddhub.no/UnitAndQuantity>

SELECT ?ProbabilisticState
WHERE {
	?ProbabilisticState_01 rdf:type ddhub:ComputedData .
	?ProbabilisticState_01 rdf:type ddhub:JSonDataType .
	?ProbabilisticState_01 ddhub:HasDynamicValue ?ProbabilisticState .
	?ProbabiliticProcessState rdf:type ddhub:ProcessState .
	?ProbabiliticProcessState rdf:type ddhub:StochasticModel .
	?ProbabilisticState_01 ddhub:IsGeneratedBy ?ProbabiliticProcessState .
	?ProcessStateInterpreter_01 rdf:type ddhub:DWISDrillingProcessStateInterpreter .
	?ProbabilisticState_01 ddhub:IsProvidedBy ?ProcessStateInterpreter_01 .
}

```

## Source Code
The source code of the `DWIS Microstates Interpretation Engine` is written in C# and can be found here:
https://github.com/D-WIS/MicroStateEngine/tree/main.

## Peer-reviewed Documentation
A peer-reviewed documentation of the `DWIS Microstates Interpretation Engine` is available in open access here: https://doi.org/10.2118/212537-PA

The reference of the paper is (in SPE Journal format):
Cayeux, E., Macpherson, J., Pirovolou, D., Laing, M., and F. Florence. "A General Framework to Describe Drilling Process States." SPE J. (2024;): doi: https://doi.org/10.2118/212537-PA

## Contributors
The code of the `DWIS Microstates Interpretation Engine` is developed and maintained by Eric Cayeux (NORCE Norwegian Research Center).

