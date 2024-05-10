# Semantic Queries for `AxialVelocityTopOfString`
## Query-DWIS.MicroState.Model.SignalGroup-AxialVelocityTopOfString-000
```sparql
PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub: <http://ddhub.no/>
PREFIX quantity: <http://ddhub.no/UnitAndQuantity>

SELECT ?v_tos
WHERE {
	?v_tos rdf:type ddhub:DynamicDrillingSignal .
	?v_tos#01 rdf:type ddhub:PhysicalData .
	?v_tos#01 ddhub:HasDynamicValue ?v_tos .
	?v_tos#01 ddhub:IsOfMeasurableQuantity quantity:BlockVelocity .
	?tos#01 rdf:type ddhub:TopOfStringReferenceLocation .
	?v_tos#01 ddhub:IsPhysicallyLocatedAt ?tos#01 .
	?MovingAverage rdf:type ddhub:MovingAverage .
	?v_tos#01 ddhub:IsTransformationOutput ?MovingAverage .
}

```
## Query-DWIS.MicroState.Model.SignalGroup-AxialVelocityTopOfString-001
```sparql
PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub: <http://ddhub.no/>
PREFIX quantity: <http://ddhub.no/UnitAndQuantity>

SELECT ?v_tos, ?sigma_v_tos, ?factOptionSet
WHERE {
	?v_tos rdf:type ddhub:DynamicDrillingSignal .
	?v_tos#01 rdf:type ddhub:PhysicalData .
	?v_tos#01 ddhub:HasDynamicValue ?v_tos .
	?v_tos#01 ddhub:IsOfMeasurableQuantity quantity:BlockVelocity .
	?tos#01 rdf:type ddhub:TopOfStringReferenceLocation .
	?v_tos#01 ddhub:IsPhysicallyLocatedAt ?tos#01 .
	?MovingAverage rdf:type ddhub:MovingAverage .
	?v_tos#01 ddhub:IsTransformationOutput ?MovingAverage .
	?sigma_v_tos rdf:type ddhub:DrillingSignal .
	?sigma_v_tos#01 rdf:type ddhub:DrillingDataPoint .
	?sigma_v_tos#01 ddhub:HasValue ?sigma_v_tos .
	?GaussianUncertainty#01 rdf:type ddhub:GaussianUncertainty .
	?v_tos#01 ddhub:HasUncertainty ?GaussianUncertainty#01 .
	?GaussianUncertainty#01 ddhub:HasUncertaintyStandardDeviation ?sigma_v_tos#01 .
  BIND ("1" as ?factOptionSet)
}

```
## Query-DWIS.MicroState.Model.SignalGroup-AxialVelocityTopOfString-002
```sparql
PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub: <http://ddhub.no/>
PREFIX quantity: <http://ddhub.no/UnitAndQuantity>

SELECT ?v_tos, ?sigma_v_tos, ?factOptionSet
WHERE {
	?v_tos rdf:type ddhub:DynamicDrillingSignal .
	?v_tos#01 rdf:type ddhub:PhysicalData .
	?v_tos#01 ddhub:HasDynamicValue ?v_tos .
	?v_tos#01 ddhub:IsOfMeasurableQuantity quantity:BlockVelocity .
	?tos#01 rdf:type ddhub:TopOfStringReferenceLocation .
	?v_tos#01 ddhub:IsPhysicallyLocatedAt ?tos#01 .
	?MovingAverage rdf:type ddhub:MovingAverage .
	?v_tos#01 ddhub:IsTransformationOutput ?MovingAverage .
	?sigma_v_tos rdf:type ddhub:DrillingSignal .
	?sigma_v_tos#01 rdf:type ddhub:DrillingDataPoint .
	?sigma_v_tos#01 ddhub:HasValue ?sigma_v_tos .
	?GaussianUncertainty#01 rdf:type ddhub:GaussianUncertainty .
	?v_tos#01 ddhub:HasUncertainty ?GaussianUncertainty#01 .
	?GaussianUncertainty#01 ddhub:HasUncertaintyStandardDeviation ?sigma_v_tos#01 .
	?GaussianUncertainty#01 ddhub:HasUncertaintyMean ?v_tos#01 .
  BIND ("1,11" as ?factOptionSet)
}

```
## Query-DWIS.MicroState.Model.SignalGroup-AxialVelocityTopOfString-003
```sparql
PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub: <http://ddhub.no/>
PREFIX quantity: <http://ddhub.no/UnitAndQuantity>

SELECT ?v_tos, ?v_tos_prec, ?v_tos_acc, ?factOptionSet
WHERE {
	?v_tos rdf:type ddhub:DynamicDrillingSignal .
	?v_tos#01 rdf:type ddhub:PhysicalData .
	?v_tos#01 ddhub:HasDynamicValue ?v_tos .
	?v_tos#01 ddhub:IsOfMeasurableQuantity quantity:BlockVelocity .
	?tos#01 rdf:type ddhub:TopOfStringReferenceLocation .
	?v_tos#01 ddhub:IsPhysicallyLocatedAt ?tos#01 .
	?MovingAverage rdf:type ddhub:MovingAverage .
	?v_tos#01 ddhub:IsTransformationOutput ?MovingAverage .
	?v_tos_prec rdf:type ddhub:DrillingSignal .
	?v_tos_prec#01 rdf:type ddhub:DrillingDataPoint .
	?v_tos_prec#01 ddhub:HasValue ?v_tos_prec .
	?v_tos_acc rdf:type ddhub:DrillingSignal .
	?v_tos_acc#01 rdf:type ddhub:DrillingDataPoint .
	?v_tos_acc#01 ddhub:HasValue ?v_tos_acc .
	?SensorUncertainty#01 rdf:type ddhub:SensorUncertainty .
	?SensorUncertainty#01 ddhub:HasUncertaintyPrecision ?v_tos_prec#01 .
	?SensorUncertainty#01 ddhub:HasUncertaintyAccuracy ?v_tos_acc#01 .
	?v_tos#01 ddhub:HasUncertainty ?SensorUncertainty#01 .
  BIND ("2" as ?factOptionSet)
}

```
## Query-DWIS.MicroState.Model.SignalGroup-AxialVelocityTopOfString-004
```sparql
PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub: <http://ddhub.no/>
PREFIX quantity: <http://ddhub.no/UnitAndQuantity>

SELECT ?v_tos, ?v_tos_prec, ?v_tos_acc, ?factOptionSet
WHERE {
	?v_tos rdf:type ddhub:DynamicDrillingSignal .
	?v_tos#01 rdf:type ddhub:PhysicalData .
	?v_tos#01 ddhub:HasDynamicValue ?v_tos .
	?v_tos#01 ddhub:IsOfMeasurableQuantity quantity:BlockVelocity .
	?tos#01 rdf:type ddhub:TopOfStringReferenceLocation .
	?v_tos#01 ddhub:IsPhysicallyLocatedAt ?tos#01 .
	?MovingAverage rdf:type ddhub:MovingAverage .
	?v_tos#01 ddhub:IsTransformationOutput ?MovingAverage .
	?v_tos_prec rdf:type ddhub:DrillingSignal .
	?v_tos_prec#01 rdf:type ddhub:DrillingDataPoint .
	?v_tos_prec#01 ddhub:HasValue ?v_tos_prec .
	?v_tos_acc rdf:type ddhub:DrillingSignal .
	?v_tos_acc#01 rdf:type ddhub:DrillingDataPoint .
	?v_tos_acc#01 ddhub:HasValue ?v_tos_acc .
	?SensorUncertainty#01 rdf:type ddhub:SensorUncertainty .
	?SensorUncertainty#01 ddhub:HasUncertaintyPrecision ?v_tos_prec#01 .
	?SensorUncertainty#01 ddhub:HasUncertaintyAccuracy ?v_tos_acc#01 .
	?v_tos#01 ddhub:HasUncertainty ?SensorUncertainty#01 .
	?SensorUncertainty#01 ddhub:HasUncertaintyMean ?v_tos#01 .
  BIND ("2,21" as ?factOptionSet)
}

```
## Query-DWIS.MicroState.Model.SignalGroup-AxialVelocityTopOfString-005
```sparql
PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub: <http://ddhub.no/>
PREFIX quantity: <http://ddhub.no/UnitAndQuantity>

SELECT ?v_tos, ?v_tos_fs, ?v_tos_prop, ?factOptionSet
WHERE {
	?v_tos rdf:type ddhub:DynamicDrillingSignal .
	?v_tos#01 rdf:type ddhub:PhysicalData .
	?v_tos#01 ddhub:HasDynamicValue ?v_tos .
	?v_tos#01 ddhub:IsOfMeasurableQuantity quantity:BlockVelocity .
	?tos#01 rdf:type ddhub:TopOfStringReferenceLocation .
	?v_tos#01 ddhub:IsPhysicallyLocatedAt ?tos#01 .
	?MovingAverage rdf:type ddhub:MovingAverage .
	?v_tos#01 ddhub:IsTransformationOutput ?MovingAverage .
	?v_tos_fs rdf:type ddhub:DrillingSignal .
	?v_tos_fs#01 rdf:type ddhub:DrillingDataPoint .
	?v_tos_fs#01 ddhub:HasValue ?v_tos_fs#01 .
	?v_tos_prop rdf:type ddhub:DrillingSignal .
	?v_tos_prop#01 rdf:type ddhub:DrillingDataPoint .
	?v_tos_prop#01 ddhub:HasValue ?v_tos_prop#01 .
	?FullScaleUncertainty#01 rdf:type ddhub:FullScaleUncertainty .
	?FullScaleUncertainty#01 ddhub:HasFullScale ?v_tos_fs#01 .
	?FullScaleUncertainty#01 ddhub:HasProportionError ?v_tos_prop#01 .
	?v_tos#01 ddhub:HasUncertainty ?FullScaleUncertainty#01 .
  BIND ("3" as ?factOptionSet)
}

```
## Query-DWIS.MicroState.Model.SignalGroup-AxialVelocityTopOfString-006
```sparql
PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub: <http://ddhub.no/>
PREFIX quantity: <http://ddhub.no/UnitAndQuantity>

SELECT ?v_tos, ?v_tos_fs, ?v_tos_prop, ?factOptionSet
WHERE {
	?v_tos rdf:type ddhub:DynamicDrillingSignal .
	?v_tos#01 rdf:type ddhub:PhysicalData .
	?v_tos#01 ddhub:HasDynamicValue ?v_tos .
	?v_tos#01 ddhub:IsOfMeasurableQuantity quantity:BlockVelocity .
	?tos#01 rdf:type ddhub:TopOfStringReferenceLocation .
	?v_tos#01 ddhub:IsPhysicallyLocatedAt ?tos#01 .
	?MovingAverage rdf:type ddhub:MovingAverage .
	?v_tos#01 ddhub:IsTransformationOutput ?MovingAverage .
	?v_tos_fs rdf:type ddhub:DrillingSignal .
	?v_tos_fs#01 rdf:type ddhub:DrillingDataPoint .
	?v_tos_fs#01 ddhub:HasValue ?v_tos_fs#01 .
	?v_tos_prop rdf:type ddhub:DrillingSignal .
	?v_tos_prop#01 rdf:type ddhub:DrillingDataPoint .
	?v_tos_prop#01 ddhub:HasValue ?v_tos_prop#01 .
	?FullScaleUncertainty#01 rdf:type ddhub:FullScaleUncertainty .
	?FullScaleUncertainty#01 ddhub:HasFullScale ?v_tos_fs#01 .
	?FullScaleUncertainty#01 ddhub:HasProportionError ?v_tos_prop#01 .
	?v_tos#01 ddhub:HasUncertainty ?FullScaleUncertainty#01 .
	?FullScaleUncertainty#01 ddhub:HasUncertaintyMean ?v_tos#01 .
  BIND ("3,31" as ?factOptionSet)
}

```
