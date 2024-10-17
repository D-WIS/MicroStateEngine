using NJsonSchema;
using System.Reflection;
using DWIS.MicroState.Model;
using OSDC.DotnetLibraries.Drilling.DrillingProperties;
using System.Text.Json;

GenerateJsonSchemas();

static void GenerateJsonSchemas()
{
    string rootDir = "." + Path.DirectorySeparatorChar;
    bool found = false;
    do
    {
        DirectoryInfo? info = Directory.GetParent(rootDir);
        if (info != null && "MicroStateEngine".Equals(info.Name))
        {
            found = true;
        }
        else
        {
            rootDir += ".." + Path.DirectorySeparatorChar;
        }
    } while (!found);
    rootDir += "DWIS.MicroState.JsonSchema" + Path.DirectorySeparatorChar;
    var microStatesSchema = JsonSchema.FromType <Tuple<MicroStates, ProbabilisticMicroStates, MicroStateIndex, FusedSignalGroup, Thresholds, Calibrations>>();
    var schemaJson = microStatesSchema.ToJson();
    using (StreamWriter writer = new StreamWriter(rootDir + "MicroStates.json"))
    {
        writer.WriteLine(schemaJson);
    }
    MicroStates microStates = new MicroStates();
    Assembly assy = microStates.GetType().Assembly;
    var dict = MetaDataDrillingProperty.GetDrillingPropertyMetaData(assy);
    if (dict != null)
    {
        var metaDataJson = JsonSerializer.Serialize(dict.ToArray());
        if (!string.IsNullOrEmpty(metaDataJson))
        {
            using (StreamWriter writer = new StreamWriter(rootDir + "MetaDataMicroStates.json"))
            {
                writer.WriteLine(metaDataJson);
            }
        }
    }
}
