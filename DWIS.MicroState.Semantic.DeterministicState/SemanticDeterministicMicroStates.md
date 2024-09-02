# Semantic Graph for `MicroStates`
```mermaid
flowchart TD
	 classDef typeClass fill:#f96;
	 classDef classClass fill:#9dd0ff;
	 classDef opcClass fill:#ff9dd0;
	 classDef quantityClass fill:#d0ff9d;
	DWIS:ComputedData([DWIS:ComputedData]) --> opc:string([opc:string]):::opcClass
	DWIS:ComputedData_01([DWIS:ComputedData_01]) --> ComputedData([ComputedData]):::typeClass
	DWIS:ProcessState([DWIS:ProcessState]) --> ProcessState([ProcessState]):::typeClass
	DWIS:DrillingProcessStateInterpreter([DWIS:DrillingProcessStateInterpreter]) --> DWISDrillingProcessStateInterpreter([DWISDrillingProcessStateInterpreter]):::typeClass
	DWIS:ProcessState([DWIS:ProcessState]) -- http://ddhub.no/BelongsToClass --> http://ddhub.no/DeterministicModel([http://ddhub.no/DeterministicModel]):::classClass
	DWIS:ComputedData_01([DWIS:ComputedData_01]) -- http://ddhub.no/HasDynamicValue --> DWIS:ComputedData([DWIS:ComputedData]):::classClass
	DWIS:ComputedData_01([DWIS:ComputedData_01]) -- http://ddhub.no/IsGeneratedBy --> DWIS:ProcessState([DWIS:ProcessState]):::classClass
	DWIS:ProcessState([DWIS:ProcessState]) -- http://ddhub.no/IsProvidedBy --> DWIS:DrillingProcessStateInterpreter([DWIS:DrillingProcessStateInterpreter]):::classClass
```

# Semantic Queries for `MicroStates`
## Query-DWIS.MicroState.Model.MicroStates-000
```sparql
PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub: <http://ddhub.no/>
PREFIX quantity: <http://ddhub.no/UnitAndQuantity>

SELECT ?ComputedData
WHERE {
	?ComputedData rdf:type ddhub:DynamicDrillingSignal .
	?ComputedData#01 rdf:type ddhub:ComputedData .
	?ComputedData#01 ddhub:HasDynamicValue ?ComputedData .
	?ProcessState rdf:type ddhub:ProcessState .
	?ProcessState ddhub:BelongsToClass ddhub:DeterministicModel .
	?ComputedData#01 ddhub:IsGeneratedBy ?ProcessState .
	?DrillingProcessStateInterpreter rdf:type ddhub:DWISDrillingProcessStateInterpreter .
	?ProcessState ddhub:IsProvidedBy ?DrillingProcessStateInterpreter .
}

```
