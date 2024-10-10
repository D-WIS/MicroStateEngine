This package is developed as part of the Society of Petroleum (SPE) Drilling and Wells Interoperability Standards (D-WIS), a sub-committee of the Drilling System Automation Technical Section.
This package contains the data model used by the D-WIS microstate interpretation engine.

There are 4 classes:
- `MicroStates`
- `ProbabilisticMicroStates`
- `SignalGroup`
- `Thresholds`

# Microstates
The class `MicroStates` is used to represent the deterministic version of the interpreted drilling process microstates. It has a TimeStampUTC property and 5 32 bit integers to store the encoded values of the microstates.

Each microstate is encoded on 2 bits, therefore providing 4 combinations with the combination 00 reserved to mean `undefined`. The index for each microstate is encoded in an enumeration: `MicroStateIndex`.

The `MicroStates` class defines two methods to update the value at a given microstate position: `UpdateMicroState` and to read the value at a given microstate index: `GetValue`. The value is passed as a `byte` and only 
the two least significant bits are used.

The `MicroStates` has the following default semantic:
``` dwis DeterministicState
DynamicDrillingSignal:DeterministicState
ComputedData:DeterministicState#01
JSonDataType:DeterministicState#01
DeterministicState#01 HasDynamicValue DeterministicState
ProcessState:DeterministicProcessState
DeterministicModel:DeterministicProcessState
DeterministicState#01 IsGeneratedBy DeterministicProcessState
DWISDrillingProcessStateInterpreter:ProcessStateInterpreter#01
DeterministicState#01 IsProvidedBy ProcessStateInterpreter#01
```

This semantic translates into the following semantic graph:
```mermaid
flowchart TD
	 classDef typeClass fill:#f96;
	 classDef classClass fill:#9dd0ff;
	 classDef opcClass fill:#ff9dd0;
	 classDef quantityClass fill:#d0ff9d;
	DWIS:DeterministicState([DWIS:DeterministicState]) --> opc:string([opc:string]):::opcClass
	DWIS:DeterministicState_01([DWIS:DeterministicState_01]) --> ComputedData([ComputedData]):::typeClass
	DWIS:DeterministicProcessState([DWIS:DeterministicProcessState]) --> ProcessState([ProcessState]):::typeClass
	DWIS:ProcessStateInterpreter_01([DWIS:ProcessStateInterpreter_01]) --> DWISDrillingProcessStateInterpreter([DWISDrillingProcessStateInterpreter]):::typeClass
	DWIS:DeterministicState_01([DWIS:DeterministicState_01]) -- http://ddhub.no/BelongsToClass --> http://ddhub.no/JSonDataType([http://ddhub.no/JSonDataType]):::classClass
	DWIS:DeterministicProcessState([DWIS:DeterministicProcessState]) -- http://ddhub.no/BelongsToClass --> http://ddhub.no/DeterministicModel([http://ddhub.no/DeterministicModel]):::classClass
	DWIS:DeterministicState_01([DWIS:DeterministicState_01]) -- http://ddhub.no/HasDynamicValue --> DWIS:DeterministicState([DWIS:DeterministicState]):::classClass
	DWIS:DeterministicState_01([DWIS:DeterministicState_01]) -- http://ddhub.no/IsGeneratedBy --> DWIS:DeterministicProcessState([DWIS:DeterministicProcessState]):::classClass
	DWIS:DeterministicState_01([DWIS:DeterministicState_01]) -- http://ddhub.no/IsProvidedBy --> DWIS:ProcessStateInterpreter_01([DWIS:ProcessStateInterpreter_01]):::classClass
```

And to retrieve the deterministic microstate on the `Blackboard`, one can use the following SparQL query:
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

The `DynamicDrillingSignal`, i.e., the live OPC-UA variable has the type string and contains a Json serialization of the class `MicroStates`. The Json schema for the classes defined in
`DWIS.MicroState.Model` can be found here: https://github.com/D-WIS/MicroStateEngine/blob/main/DWIS.MicroState.JsonSchema/MicroStates.json. The meta data describing the semantic of the
class `MicroState` can be found here: https://github.com/D-WIS/MicroStateEngine/blob/main/DWIS.MicroState.JsonSchema/MetaDataMicroStates.json.


