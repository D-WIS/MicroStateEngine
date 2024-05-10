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

