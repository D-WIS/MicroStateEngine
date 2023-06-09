using NJsonSchema;
using DWIS.MicroState.Model;

GenerateJsonSchemas();

static void GenerateJsonSchemas()
{
    string rootDir = ".\\";
    bool found = false;
    do
    {
        DirectoryInfo info = Directory.GetParent(rootDir);
        if (info != null && "DWIS.MicroState".Equals(info.Name))
        {
            found = true;
        }
        else
        {
            rootDir += "..\\";
        }
    } while (!found);
    rootDir += "DWIS.MicroState.JsonSchema\\";
    var RigOSCapabilitiesSchema = JsonSchema.FromType < Tuple < MicroStates, Thresholds, Signals, MicroStates.MicroStateIndex>>();
    var WellPathSchemaSchemaJson = RigOSCapabilitiesSchema.ToJson();
    using (StreamWriter writer = new StreamWriter(rootDir + "MicroStates.json"))
    {
        writer.WriteLine(WellPathSchemaSchemaJson);
    }
}
