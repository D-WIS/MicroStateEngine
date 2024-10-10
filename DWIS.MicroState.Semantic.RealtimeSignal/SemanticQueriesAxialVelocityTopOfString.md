# Semantic Queries for `AxialVelocityTopOfString`
## Query-DWIS.MicroState.Model.SignalGroup-AxialVelocityTopOfString-000
```sparql
PREFIX rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub:<http://ddhub.no/>
PREFIX quantity:<http://ddhub.no/UnitAndQuantity>

SELECT ?v_tos
WHERE {
	?v_tos_01 rdf:type ddhub:PhysicalData .
	?v_tos_01 rdf:type ddhub:ContinuousDataType .
	?v_tos_01 ddhub:HasDynamicValue ?v_tos .
	?v_tos_01 ddhub:IsOfMeasurableQuantity quantity:BlockVelocityDrilling .
	?tos_01 rdf:type ddhub:TopOfStringReferenceLocation .
	?v_tos_01 ddhub:IsPhysicallyLocatedAt ?tos_01 .
	?MovingAverage rdf:type ddhub:MovingAverage .
	?v_tos_01 ddhub:IsTransformationOutput ?MovingAverage .
}

```
## Query-DWIS.MicroState.Model.SignalGroup-AxialVelocityTopOfString-001
```sparql
PREFIX rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub:<http://ddhub.no/>
PREFIX quantity:<http://ddhub.no/UnitAndQuantity>

SELECT ?v_tos ?sigma_v_tos ?factOptionSet
WHERE {
	?v_tos_01 rdf:type ddhub:PhysicalData .
	?v_tos_01 rdf:type ddhub:ContinuousDataType .
	?v_tos_01 ddhub:HasDynamicValue ?v_tos .
	?v_tos_01 ddhub:IsOfMeasurableQuantity quantity:BlockVelocityDrilling .
	?tos_01 rdf:type ddhub:TopOfStringReferenceLocation .
	?v_tos_01 ddhub:IsPhysicallyLocatedAt ?tos_01 .
	?MovingAverage rdf:type ddhub:MovingAverage .
	?v_tos_01 ddhub:IsTransformationOutput ?MovingAverage .
	?sigma_v_tos_01 rdf:type ddhub:DrillingDataPoint .
	?sigma_v_tos_01 ddhub:HasValue ?sigma_v_tos .
	?GaussianUncertainty_01 rdf:type ddhub:GaussianUncertainty .
	?v_tos_01 ddhub:HasUncertainty ?GaussianUncertainty_01 .
	?GaussianUncertainty_01 ddhub:HasUncertaintyStandardDeviation ?sigma_v_tos_01 .
  BIND (' 1' as ?factOptionSet)
}

```
## Query-DWIS.MicroState.Model.SignalGroup-AxialVelocityTopOfString-002
```sparql
PREFIX rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub:<http://ddhub.no/>
PREFIX quantity:<http://ddhub.no/UnitAndQuantity>

SELECT ?v_tos ?sigma_v_tos ?factOptionSet
WHERE {
	?v_tos_01 rdf:type ddhub:PhysicalData .
	?v_tos_01 rdf:type ddhub:ContinuousDataType .
	?v_tos_01 ddhub:HasDynamicValue ?v_tos .
	?v_tos_01 ddhub:IsOfMeasurableQuantity quantity:BlockVelocityDrilling .
	?tos_01 rdf:type ddhub:TopOfStringReferenceLocation .
	?v_tos_01 ddhub:IsPhysicallyLocatedAt ?tos_01 .
	?MovingAverage rdf:type ddhub:MovingAverage .
	?v_tos_01 ddhub:IsTransformationOutput ?MovingAverage .
	?sigma_v_tos_01 rdf:type ddhub:DrillingDataPoint .
	?sigma_v_tos_01 ddhub:HasValue ?sigma_v_tos .
	?GaussianUncertainty_01 rdf:type ddhub:GaussianUncertainty .
	?v_tos_01 ddhub:HasUncertainty ?GaussianUncertainty_01 .
	?GaussianUncertainty_01 ddhub:HasUncertaintyStandardDeviation ?sigma_v_tos_01 .
	?GaussianUncertainty_01 ddhub:HasUncertaintyMean ?v_tos_01 .
  BIND (' 1 11' as ?factOptionSet)
}

```
## Query-DWIS.MicroState.Model.SignalGroup-AxialVelocityTopOfString-003
```sparql
PREFIX rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub:<http://ddhub.no/>
PREFIX quantity:<http://ddhub.no/UnitAndQuantity>

SELECT ?v_tos ?v_tos_prec ?v_tos_acc ?factOptionSet
WHERE {
	?v_tos_01 rdf:type ddhub:PhysicalData .
	?v_tos_01 rdf:type ddhub:ContinuousDataType .
	?v_tos_01 ddhub:HasDynamicValue ?v_tos .
	?v_tos_01 ddhub:IsOfMeasurableQuantity quantity:BlockVelocityDrilling .
	?tos_01 rdf:type ddhub:TopOfStringReferenceLocation .
	?v_tos_01 ddhub:IsPhysicallyLocatedAt ?tos_01 .
	?MovingAverage rdf:type ddhub:MovingAverage .
	?v_tos_01 ddhub:IsTransformationOutput ?MovingAverage .
	?v_tos_prec_01 rdf:type ddhub:DrillingDataPoint .
	?v_tos_prec_01 ddhub:HasValue ?v_tos_prec .
	?v_tos_acc_01 rdf:type ddhub:DrillingDataPoint .
	?v_tos_acc_01 ddhub:HasValue ?v_tos_acc .
	?SensorUncertainty_01 rdf:type ddhub:SensorUncertainty .
	?SensorUncertainty_01 ddhub:HasUncertaintyPrecision ?v_tos_prec_01 .
	?SensorUncertainty_01 ddhub:HasUncertaintyAccuracy ?v_tos_acc_01 .
	?v_tos_01 ddhub:HasUncertainty ?SensorUncertainty_01 .
  BIND (' 2' as ?factOptionSet)
}

```
## Query-DWIS.MicroState.Model.SignalGroup-AxialVelocityTopOfString-004
```sparql
PREFIX rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub:<http://ddhub.no/>
PREFIX quantity:<http://ddhub.no/UnitAndQuantity>

SELECT ?v_tos ?v_tos_prec ?v_tos_acc ?factOptionSet
WHERE {
	?v_tos_01 rdf:type ddhub:PhysicalData .
	?v_tos_01 rdf:type ddhub:ContinuousDataType .
	?v_tos_01 ddhub:HasDynamicValue ?v_tos .
	?v_tos_01 ddhub:IsOfMeasurableQuantity quantity:BlockVelocityDrilling .
	?tos_01 rdf:type ddhub:TopOfStringReferenceLocation .
	?v_tos_01 ddhub:IsPhysicallyLocatedAt ?tos_01 .
	?MovingAverage rdf:type ddhub:MovingAverage .
	?v_tos_01 ddhub:IsTransformationOutput ?MovingAverage .
	?v_tos_prec_01 rdf:type ddhub:DrillingDataPoint .
	?v_tos_prec_01 ddhub:HasValue ?v_tos_prec .
	?v_tos_acc_01 rdf:type ddhub:DrillingDataPoint .
	?v_tos_acc_01 ddhub:HasValue ?v_tos_acc .
	?SensorUncertainty_01 rdf:type ddhub:SensorUncertainty .
	?SensorUncertainty_01 ddhub:HasUncertaintyPrecision ?v_tos_prec_01 .
	?SensorUncertainty_01 ddhub:HasUncertaintyAccuracy ?v_tos_acc_01 .
	?v_tos_01 ddhub:HasUncertainty ?SensorUncertainty_01 .
	?SensorUncertainty_01 ddhub:HasUncertaintyMean ?v_tos_01 .
  BIND (' 2 21' as ?factOptionSet)
}

```
## Query-DWIS.MicroState.Model.SignalGroup-AxialVelocityTopOfString-005
```sparql
PREFIX rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub:<http://ddhub.no/>
PREFIX quantity:<http://ddhub.no/UnitAndQuantity>

SELECT ?v_tos ?v_tos_fs ?v_tos_prop ?factOptionSet
WHERE {
	?v_tos_01 rdf:type ddhub:PhysicalData .
	?v_tos_01 rdf:type ddhub:ContinuousDataType .
	?v_tos_01 ddhub:HasDynamicValue ?v_tos .
	?v_tos_01 ddhub:IsOfMeasurableQuantity quantity:BlockVelocityDrilling .
	?tos_01 rdf:type ddhub:TopOfStringReferenceLocation .
	?v_tos_01 ddhub:IsPhysicallyLocatedAt ?tos_01 .
	?MovingAverage rdf:type ddhub:MovingAverage .
	?v_tos_01 ddhub:IsTransformationOutput ?MovingAverage .
	?v_tos_fs_01 rdf:type ddhub:DrillingDataPoint .
	?v_tos_fs_01 ddhub:HasValue ?v_tos_fs_01 .
	?v_tos_prop_01 rdf:type ddhub:DrillingDataPoint .
	?v_tos_prop_01 ddhub:HasValue ?v_tos_prop_01 .
	?FullScaleUncertainty_01 rdf:type ddhub:FullScaleUncertainty .
	?FullScaleUncertainty_01 ddhub:HasFullScale ?v_tos_fs_01 .
	?FullScaleUncertainty_01 ddhub:HasProportionError ?v_tos_prop_01 .
	?v_tos_01 ddhub:HasUncertainty ?FullScaleUncertainty_01 .
  BIND (' 3' as ?factOptionSet)
}

```
## Query-DWIS.MicroState.Model.SignalGroup-AxialVelocityTopOfString-006
```sparql
PREFIX rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX ddhub:<http://ddhub.no/>
PREFIX quantity:<http://ddhub.no/UnitAndQuantity>

SELECT ?v_tos ?v_tos_fs ?v_tos_prop ?factOptionSet
WHERE {
	?v_tos_01 rdf:type ddhub:PhysicalData .
	?v_tos_01 rdf:type ddhub:ContinuousDataType .
	?v_tos_01 ddhub:HasDynamicValue ?v_tos .
	?v_tos_01 ddhub:IsOfMeasurableQuantity quantity:BlockVelocityDrilling .
	?tos_01 rdf:type ddhub:TopOfStringReferenceLocation .
	?v_tos_01 ddhub:IsPhysicallyLocatedAt ?tos_01 .
	?MovingAverage rdf:type ddhub:MovingAverage .
	?v_tos_01 ddhub:IsTransformationOutput ?MovingAverage .
	?v_tos_fs_01 rdf:type ddhub:DrillingDataPoint .
	?v_tos_fs_01 ddhub:HasValue ?v_tos_fs_01 .
	?v_tos_prop_01 rdf:type ddhub:DrillingDataPoint .
	?v_tos_prop_01 ddhub:HasValue ?v_tos_prop_01 .
	?FullScaleUncertainty_01 rdf:type ddhub:FullScaleUncertainty .
	?FullScaleUncertainty_01 ddhub:HasFullScale ?v_tos_fs_01 .
	?FullScaleUncertainty_01 ddhub:HasProportionError ?v_tos_prop_01 .
	?v_tos_01 ddhub:HasUncertainty ?FullScaleUncertainty_01 .
	?FullScaleUncertainty_01 ddhub:HasUncertaintyMean ?v_tos_01 .
  BIND (' 3 31' as ?factOptionSet)
}

```
