# Semantic Queries for `ZeroAxialVelocityTopOfStringThreshold`
## Query-DWIS.MicroState.Model.Thresholds-ZeroAxialVelocityTopOfStringThreshold-000
```sparql
PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub: <http://ddhub.no/>
PREFIX quantity: <http://ddhub.no/UnitAndQuantity>

SELECT ?ZeroAxialVelocityTopOfStringThreshold
WHERE {
	?ZeroAxialVelocityTopOfStringThreshold rdf:type ddhub:DrillingSignal .
	?ZeroAxialVelocityTopOfStringThreshold#01 rdf:type ddhub:Limit .
	?ZeroAxialVelocityTopOfStringThreshold#01 ddhub:HasValue ?ZeroAxialVelocityTopOfStringThreshold .
	?ZeroAxialVelocityTopOfStringThreshold#01 ddhub:IsOfMeasurableQuantity quantity:BlockVelocity .
	?tos#01 rdf:type ddhub:TopOfStringReferenceLocation .
	?ZeroAxialVelocityTopOfStringThreshold#01 ddhub:IsPhysicallyLocatedAt ?tos#01 .
	?MovingAverage rdf:type ddhub:MovingAverage .
	?signal#01 rdf:type ddhub:DrillingDataPoint .
	?signal#01 ddhub:IsTransformationOutput ?MovingAverage .
}

```
## Query-DWIS.MicroState.Model.Thresholds-ZeroAxialVelocityTopOfStringThreshold-001
```sparql
PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub: <http://ddhub.no/>
PREFIX quantity: <http://ddhub.no/UnitAndQuantity>

SELECT ?ZeroAxialVelocityTopOfStringThreshold, ?factOptionSet
WHERE {
	?ZeroAxialVelocityTopOfStringThreshold rdf:type ddhub:DrillingSignal .
	?ZeroAxialVelocityTopOfStringThreshold#01 rdf:type ddhub:Limit .
	?ZeroAxialVelocityTopOfStringThreshold#01 ddhub:HasValue ?ZeroAxialVelocityTopOfStringThreshold .
	?ZeroAxialVelocityTopOfStringThreshold#01 ddhub:IsOfMeasurableQuantity quantity:BlockVelocity .
	?tos#01 rdf:type ddhub:TopOfStringReferenceLocation .
	?ZeroAxialVelocityTopOfStringThreshold#01 ddhub:IsPhysicallyLocatedAt ?tos#01 .
	?MovingAverage rdf:type ddhub:MovingAverage .
	?signal#01 rdf:type ddhub:DrillingDataPoint .
	?signal#01 ddhub:IsTransformationOutput ?MovingAverage .
	?ZeroAxialVelocityTopOfStringThreshold#01 ddhub:IsToBeComparedWith ?signal#01 .
  BIND ("1" as ?factOptionSet)
}

```
## Query-DWIS.MicroState.Model.Thresholds-ZeroAxialVelocityTopOfStringThreshold-002
```sparql
PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub: <http://ddhub.no/>
PREFIX quantity: <http://ddhub.no/UnitAndQuantity>

SELECT ?ZeroAxialVelocityTopOfStringThreshold, ?factOptionSet
WHERE {
	?ZeroAxialVelocityTopOfStringThreshold rdf:type ddhub:DrillingSignal .
	?ZeroAxialVelocityTopOfStringThreshold#01 rdf:type ddhub:Limit .
	?ZeroAxialVelocityTopOfStringThreshold#01 ddhub:HasValue ?ZeroAxialVelocityTopOfStringThreshold .
	?ZeroAxialVelocityTopOfStringThreshold#01 ddhub:IsOfMeasurableQuantity quantity:BlockVelocity .
	?tos#01 rdf:type ddhub:TopOfStringReferenceLocation .
	?ZeroAxialVelocityTopOfStringThreshold#01 ddhub:IsPhysicallyLocatedAt ?tos#01 .
	?MovingAverage rdf:type ddhub:MovingAverage .
	?signal#01 rdf:type ddhub:DrillingDataPoint .
	?signal#01 ddhub:IsTransformationOutput ?MovingAverage .
	?signal#01 ddhub:IsToBeComparedWith ?ZeroAxialVelocityTopOfStringThreshold#01 .
  BIND ("2" as ?factOptionSet)
}

```
