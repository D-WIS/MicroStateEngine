# DWIS Microstates

Microstate interpretation engine, models, semantic definitions, and supporting tools (threshold server, signal generators, state readers) for DWIS drilling process state estimation.

## Repository layout
- `DWIS.MicroState.sln` — solution aggregating all projects.
- Engine & services:
  - `DWIS.MicroState.InterpretationEngine/` — main microstate interpreter service (uses DWIS Blackboard signals/thresholds, outputs deterministic & probabilistic states).
  - `DWIS.MicroState.ThresholdsServer/` — publishes microstate thresholds to the blackboard.
  - `DWIS.MicroState.SignalGenerator.Test/`, `StateReader.Test/`, `ThresholdsGenerator.Test/`, `DWIS.MicroState.UnitTests/` — test utilities and unit tests.
  - `DWIS.MicroState.ExportSemanticDescription/` — export semantic graphs/descriptions.
- Models & schemas:
  - `DWIS.MicroState.Model/` and `DWIS.MicroState.ModelShared/` — full model vs. data structures (also available as NuGet packages).
  - `DWIS.MicroState.JsonSchema/`, `JsonSD/`, `JsonCS/` — JSON schema, schema generation, and code generation for microstate payloads.
  - `DWIS.MicroState.Semantic.*` — semantic definitions for deterministic/probabilistic states, realtime signals, thresholds.
- Tooling:
  - `DWIS.MicroState.ModelShared`/`Model` NuGet packages; `nuget.config` included for feeds.
  - `home/config.json` — sample runtime configuration for services.

## Build
- `dotnet build DWIS.MicroState.sln`

## Run (interpretation engine)
- Container (examples from README):
  - Windows: `docker run --name microstatesengine -v C:\Volumes\DWISMicroStateSignalGenerator:/home digiwells/dwismicrostateinterpretationengine:stable`
  - Linux: `docker run --name microstatesengine -v /home/Volumes/DWISMicroStateSignalGenerator:/home digiwells/dwismicrostateinterpretationengine:stable`
- Config is loaded from `/home/config.json`; see `home/config.json` for loop duration, OPC UA URL, probabilities/std devs, calibration settings, and random value generation.

## Notes
- Engine consumes `SignalGroup` and `Thresholds` from the DWIS Blackboard via SPARQL, calibrates/fuses multi-source signals, and publishes deterministic/probabilistic microstates back to the blackboard with semantic descriptions.
- JSON schemas for exchanged objects: `DWIS.MicroState.JsonSchema/MicroStates.json`.
- NuGet dependencies include DWIS reference client and OSDC drilling properties libs; ensure blackboard is running before starting the interpreter/threshold server.
- Semantic SPARQL examples and detailed descriptions remain below for reference.
