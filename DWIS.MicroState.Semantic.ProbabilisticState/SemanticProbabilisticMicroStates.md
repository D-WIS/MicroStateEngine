# Semantic Graph for `ProbabilisticMicroStates`
```mermaid
flowchart TD
	 classDef typeClass fill:#f96;
	 classDef classClass fill:#9dd0ff;
	 classDef opcClass fill:#ff9dd0;
	 classDef quantityClass fill:#d0ff9d;
	DWIS:ProbabilisticState([DWIS:ProbabilisticState]) --> opc:string([opc:string]):::opcClass
	DWIS:ProbabilisticState_01([DWIS:ProbabilisticState_01]) --> ComputedData([ComputedData]):::typeClass
	DWIS:ProbabiliticProcessState([DWIS:ProbabiliticProcessState]) --> ProcessState([ProcessState]):::typeClass
	DWIS:ProcessStateInterpreter_01([DWIS:ProcessStateInterpreter_01]) --> DWISDrillingProcessStateInterpreter([DWISDrillingProcessStateInterpreter]):::typeClass
	DWIS:ProbabilisticState_01([DWIS:ProbabilisticState_01]) -- http://ddhub.no/BelongsToClass --> http://ddhub.no/JSonDataType([http://ddhub.no/JSonDataType]):::classClass
	DWIS:ProbabiliticProcessState([DWIS:ProbabiliticProcessState]) -- http://ddhub.no/BelongsToClass --> http://ddhub.no/StochasticModel([http://ddhub.no/StochasticModel]):::classClass
	DWIS:ProbabilisticState_01([DWIS:ProbabilisticState_01]) -- http://ddhub.no/HasDynamicValue --> DWIS:ProbabilisticState([DWIS:ProbabilisticState]):::classClass
	DWIS:ProbabilisticState_01([DWIS:ProbabilisticState_01]) -- http://ddhub.no/IsGeneratedBy --> DWIS:ProbabiliticProcessState([DWIS:ProbabiliticProcessState]):::classClass
	DWIS:ProbabilisticState_01([DWIS:ProbabilisticState_01]) -- http://ddhub.no/IsProvidedBy --> DWIS:ProcessStateInterpreter_01([DWIS:ProcessStateInterpreter_01]):::classClass
```

# Semantic Queries for `ProbabilisticMicroStates`
## Query-DWIS.MicroState.Model.ProbabilisticMicroStates-000
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
