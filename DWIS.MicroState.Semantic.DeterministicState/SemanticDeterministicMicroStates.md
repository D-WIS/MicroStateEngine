# Semantic Graph for `MicroStates`
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

# Semantic Queries for `MicroStates`
## Query-DWIS.MicroState.Model.MicroStates-000
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
