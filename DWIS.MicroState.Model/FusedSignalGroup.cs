﻿using DWIS.API.DTO;
using DWIS.Client.ReferenceImplementation;
using DWIS.Vocabulary.Schemas;
using OSDC.DotnetLibraries.Drilling.DrillingProperties;
using System.Reflection;
using System.Text.Json;

namespace DWIS.MicroState.Model
{
    [AccessToVariable(CommonProperty.VariableAccessType.Assignable)]
    [SemanticTypeVariable("FusedSignalsForMicroStates")]
    [SemanticFact("FusedSignalsForMicroStates", Nouns.Enum.DynamicDrillingSignal)]
    [SemanticFact("FusedSignalsForMicroStates#01", Nouns.Enum.DigitalTwinAdvice)]
    [SemanticFact("FusedSignalsForMicroStates#01", Verbs.Enum.HasDynamicValue, "FusedSignalsForMicroStates")]
    [SemanticFact("MicroStateInterpreter", Nouns.Enum.DWISDrillingProcessStateInterpreter)]
    [SemanticFact("FusedSignalsForMicroStates#01", Verbs.Enum.IsProvidedBy, "MicroStateInterpreter")]
    [SemanticFact("SignalFusionInterpreter#01", Nouns.Enum.Interpreter)]
    [SemanticFact("SignalFusionInterpreter#01", Nouns.Enum.DescriptiveModel)]
    [SemanticFact("SignalFusionInterpreter#01", Nouns.Enum.ForwardModel)]
    [SemanticFact("SignalFusionInterpreter#01", Nouns.Enum.WhiteBoxModel)]
    [SemanticFact("SignalFusionInterpreter#01", Nouns.Enum.SpecializedModel)]
    [SemanticFact("SignalFusionInterpreter#01", Nouns.Enum.EmpiricalModel)]
    [SemanticFact("SignalFusionInterpreter#01", Nouns.Enum.StochasticModel)]
    [SemanticFact("FusedSignalsForMicroStates#01", Verbs.Enum.IsComputedBy, "SignalFusionInterpreter#01")]
    [SemanticFact("DigitalTwinSignals#01", Nouns.Enum.DigitalTwinAdvice)]
    [SemanticFact("DigitalTwin", Nouns.Enum.Simulator)]
    [SemanticFact("DigitalTwinSignals#01", Verbs.Enum.IsRecommendedBy, "DigitalTwin")]
    [SemanticFact("DigitalTwinSignals#01", Verbs.Enum.IsProvidedTo, "MicroStateInterpreter#01")]
    [SemanticFact("DigitalTwinSignals#01", Verbs.Enum.IsComputationInput, "SignalFusionInterpreter#01")]
    public class FusedSignalGroup : SignalGroup
    {
        private static string prefix_ = "DWIS:MicroState:FusedSignalGroup:";
        private static string companyName_ = "DWIS";

        public override bool RegisterToBlackboard(IOPCUADWISClient? DWISClient, ref QueryResult? placeHolder)
        {
            bool ok = false;
            if (DWISClient != null)
            {
                Type type = GetType();
                Assembly assembly = type.Assembly;

                string? manifestName = type.FullName;
                if (!string.IsNullOrEmpty(manifestName))
                {
                    ManifestFile? manifestFile = GeneratorSparQLManifestFile.GetManifestFile(assembly, type.FullName, manifestName, companyName_, prefix_);
                    Dictionary<string, QuerySpecification>? queries = GeneratorSparQLManifestFile.GetSparQLQueries(assembly, type.FullName);
                    if (queries != null && queries.Count > 0 && manifestFile != null)
                    {
                        QueryResult? res = null;
                        List<List<string>> vars = new List<List<string>>();
                        foreach (var kvp in queries)
                        {
                            if (kvp.Value != null && !string.IsNullOrEmpty(kvp.Value.SparQL))
                            {
                                if (kvp.Value.Variables != null)
                                {
                                    vars.Add(kvp.Value.Variables);
                                }
                                var result = DWISClient.GetQueryResult(kvp.Value.SparQL);
                                if (result != null && result.Results != null && result.Results.Count > 0)
                                {
                                    res = result;
                                    break;
                                }
                            }
                        }
                        List<string>? variables = Utils.CommonVariables(vars);
                        // if we couldn't find any answer then the manifest must be injected
                        if (res == null)
                        {
                            var r = DWISClient.Inject(manifestFile);
                            if (r != null && r.Success)
                            {
                                if (r.ProvidedVariables != null && r.ProvidedVariables.Count > 0)
                                {
                                    placeHolder = new QueryResult();
                                    QueryResultRow row = new QueryResultRow();
                                    List<NodeIdentifier> items = new List<NodeIdentifier>();
                                    placeHolder.VariablesHeader = variables;
                                    row.Items = items;
                                    foreach (var kvp in r.ProvidedVariables)
                                    {
                                        DWISClient.GetNameSpace(kvp.InjectedID.NameSpaceIndex, out string ns);
                                        items.Add(new NodeIdentifier() { ID = kvp.InjectedID.ID, NameSpace = ns });
                                    }
                                    placeHolder.Add(row);
                                }
                            }
                        }
                        else
                        {
                            // a manifest has already been injected.
                            placeHolder = res;
                            ok = true;
                        }
                    }
                }
            }
            return ok;
        }

        public override bool SendToBlackboard(IOPCUADWISClient? DWISClient, QueryResult? placeHolder)
        {
            if (DWISClient != null && placeHolder != null)
            {
                bool ok = false;
                if (placeHolder != null && placeHolder.Count > 0 && placeHolder[0].Count > 0)
                {
                    string json = JsonSerializer.Serialize(this);
                    if (!string.IsNullOrEmpty(json))
                    {
                        {
                            NodeIdentifier id = placeHolder[0][0];
                            if (id != null && !string.IsNullOrEmpty(id.ID) && !string.IsNullOrEmpty(id.NameSpace))
                            {
                                // OPC-UA code to set the value at the node id = ID
                                (string nameSpace, string id, object value, DateTime sourceTimestamp)[] outputs = new (string nameSpace, string id, object value, DateTime sourceTimestamp)[1];
                                outputs[0].nameSpace = id.NameSpace;
                                outputs[0].id = id.ID;
                                outputs[0].value = json;
                                outputs[0].sourceTimestamp = DateTime.UtcNow;
                                ok = DWISClient.UpdateAnyVariables(outputs);
                            }
                        }
                    }
                }
                return ok;
            }
            else
            {
                return false;
            }
        }
    }
}
