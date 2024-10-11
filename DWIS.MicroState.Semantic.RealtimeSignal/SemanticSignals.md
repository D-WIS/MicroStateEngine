# Semantic Graph for `SignalGroup`
```mermaid
flowchart TD
	 classDef typeClass fill:#f96;
	 classDef classClass fill:#9dd0ff;
	 classDef opcClass fill:#ff9dd0;
	 classDef quantityClass fill:#d0ff9d;
	DWIS:DigitalTwinSignalsForMicroStates([DWIS:DigitalTwinSignalsForMicroStates]) --> opc:string([opc:string]):::opcClass
	DWIS:DigitalTwinSignalsForMicroStates_01([DWIS:DigitalTwinSignalsForMicroStates_01]) --> DigitalTwinAdvice([DigitalTwinAdvice]):::typeClass
	DWIS:DigitalTwin([DWIS:DigitalTwin]) --> Simulator([Simulator]):::typeClass
	DWIS:MicroStateInterpreter([DWIS:MicroStateInterpreter]) --> DWISDrillingProcessStateInterpreter([DWISDrillingProcessStateInterpreter]):::typeClass
	DWIS:DigitalTwinSignalsForMicroStates_01([DWIS:DigitalTwinSignalsForMicroStates_01]) -- http://ddhub.no/HasDynamicValue --> DWIS:DigitalTwinSignalsForMicroStates([DWIS:DigitalTwinSignalsForMicroStates]):::classClass
	DWIS:DigitalTwinSignalsForMicroStates_01([DWIS:DigitalTwinSignalsForMicroStates_01]) -- http://ddhub.no/IsRecommendedBy --> DWIS:DigitalTwin([DWIS:DigitalTwin]):::classClass
	DWIS:DigitalTwinSignalsForMicroStates_01([DWIS:DigitalTwinSignalsForMicroStates_01]) -- http://ddhub.no/IsProvidedTo --> DWIS:MicroStateInterpreter([DWIS:MicroStateInterpreter]):::classClass
```

# Semantic Queries for `SignalGroup`
## Query-DWIS.MicroState.Model.SignalGroup-000
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
