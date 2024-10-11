# Semantic Graph for `Thresholds`
```mermaid
flowchart TD
	 classDef typeClass fill:#f96;
	 classDef classClass fill:#9dd0ff;
	 classDef opcClass fill:#ff9dd0;
	 classDef quantityClass fill:#d0ff9d;
	DWIS:MicroStateThresholds([DWIS:MicroStateThresholds]) --> opc:string([opc:string]):::opcClass
	DWIS:MicroStateThresholds_01([DWIS:MicroStateThresholds_01]) --> ConfigurationData([ConfigurationData]):::typeClass
	DWIS:MicroStateInterpreter([DWIS:MicroStateInterpreter]) --> DWISDrillingProcessStateInterpreter([DWISDrillingProcessStateInterpreter]):::typeClass
	DWIS:MicroStateThresholds_01([DWIS:MicroStateThresholds_01]) -- http://ddhub.no/HasDynamicValue --> DWIS:MicroStateThresholds([DWIS:MicroStateThresholds]):::classClass
	DWIS:MicroStateThresholds_01([DWIS:MicroStateThresholds_01]) -- http://ddhub.no/IsProvidedTo --> DWIS:MicroStateInterpreter([DWIS:MicroStateInterpreter]):::classClass
	DWIS:MicroStateThresholds_01([DWIS:MicroStateThresholds_01]) -- http://ddhub.no/IsLimitFor --> DWIS:MicroStateInterpreter([DWIS:MicroStateInterpreter]):::classClass
```

# Semantic Queries for `Thresholds`
## Query-DWIS.MicroState.Model.Thresholds-000
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
