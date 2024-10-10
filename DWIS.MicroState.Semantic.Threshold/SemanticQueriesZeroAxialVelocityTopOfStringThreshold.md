# Semantic Queries for `ZeroAxialVelocityTopOfStringThreshold`
## Query-DWIS.MicroState.Model.Thresholds-ZeroAxialVelocityTopOfStringThreshold-000
```sparql
PREFIX rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub:<http://ddhub.no/>
PREFIX quantity:<http://ddhub.no/UnitAndQuantity>

SELECT ?ZeroAxialVelocityTopOfStringThreshold
WHERE {
	?ZeroAxialVelocityTopOfStringThreshold_01 rdf:type ddhub:Limit .
	?ZeroAxialVelocityTopOfStringThreshold_01 ddhub:HasValue ?ZeroAxialVelocityTopOfStringThreshold .
	?ZeroAxialVelocityTopOfStringThreshold_01 ddhub:IsOfMeasurableQuantity quantity:BlockVelocityDrilling .
	?tos_01 rdf:type ddhub:TopOfStringReferenceLocation .
	?ZeroAxialVelocityTopOfStringThreshold_01 ddhub:IsPhysicallyLocatedAt ?tos_01 .
	?MovingAverage rdf:type ddhub:MovingAverage .
	?signal_01 rdf:type ddhub:DrillingDataPoint .
	?signal_01 ddhub:IsTransformationOutput ?MovingAverage .
}

```
## Query-DWIS.MicroState.Model.Thresholds-ZeroAxialVelocityTopOfStringThreshold-001
```sparql
PREFIX rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub:<http://ddhub.no/>
PREFIX quantity:<http://ddhub.no/UnitAndQuantity>

SELECT ?ZeroAxialVelocityTopOfStringThreshold ?factOptionSet
WHERE {
	?ZeroAxialVelocityTopOfStringThreshold_01 rdf:type ddhub:Limit .
	?ZeroAxialVelocityTopOfStringThreshold_01 ddhub:HasValue ?ZeroAxialVelocityTopOfStringThreshold .
	?ZeroAxialVelocityTopOfStringThreshold_01 ddhub:IsOfMeasurableQuantity quantity:BlockVelocityDrilling .
	?tos_01 rdf:type ddhub:TopOfStringReferenceLocation .
	?ZeroAxialVelocityTopOfStringThreshold_01 ddhub:IsPhysicallyLocatedAt ?tos_01 .
	?MovingAverage rdf:type ddhub:MovingAverage .
	?signal_01 rdf:type ddhub:DrillingDataPoint .
	?signal_01 ddhub:IsTransformationOutput ?MovingAverage .
	?ZeroAxialVelocityTopOfStringThreshold_01 ddhub:IsToBeComparedWith ?signal_01 .
  BIND (' 1' as ?factOptionSet)
}

```
## Query-DWIS.MicroState.Model.Thresholds-ZeroAxialVelocityTopOfStringThreshold-002
```sparql
PREFIX rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub:<http://ddhub.no/>
PREFIX quantity:<http://ddhub.no/UnitAndQuantity>

SELECT ?ZeroAxialVelocityTopOfStringThreshold ?factOptionSet
WHERE {
	?ZeroAxialVelocityTopOfStringThreshold_01 rdf:type ddhub:Limit .
	?ZeroAxialVelocityTopOfStringThreshold_01 ddhub:HasValue ?ZeroAxialVelocityTopOfStringThreshold .
	?ZeroAxialVelocityTopOfStringThreshold_01 ddhub:IsOfMeasurableQuantity quantity:BlockVelocityDrilling .
	?tos_01 rdf:type ddhub:TopOfStringReferenceLocation .
	?ZeroAxialVelocityTopOfStringThreshold_01 ddhub:IsPhysicallyLocatedAt ?tos_01 .
	?MovingAverage rdf:type ddhub:MovingAverage .
	?signal_01 rdf:type ddhub:DrillingDataPoint .
	?signal_01 ddhub:IsTransformationOutput ?MovingAverage .
	?signal_01 ddhub:IsToBeComparedWith ?ZeroAxialVelocityTopOfStringThreshold_01 .
  BIND (' 2' as ?factOptionSet)
}

```
