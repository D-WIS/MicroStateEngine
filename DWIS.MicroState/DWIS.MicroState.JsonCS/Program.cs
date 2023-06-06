using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;

await Generate(args);

static async Task Generate(string[] args)
{
    string solutionRootDir = ".\\";
    bool found = false;
    do
    {
        DirectoryInfo info = Directory.GetParent(solutionRootDir);
        if (info != null && "DWIS.MicroState".Equals(info.Name))
        {
            found = true;
        }
        else
        {
            solutionRootDir += "..\\";
        }
    } while (!found);
    string jsonSchemaRootDir = solutionRootDir + "DWIS.MicroState.JsonSchema\\";
    string sourceCodeDir = solutionRootDir + "DWIS.MicroState.ModelShared\\";
    if (args != null && args.Length >= 1 && Directory.Exists(args[0]))
    {
        sourceCodeDir = args[0];
    }
    string codeNamespace = "DWIS.MicroState.ModelShared";
    if (args != null && args.Length >= 2 && !string.IsNullOrEmpty(args[1]))
    {
        codeNamespace = args[1];
    }
    JsonSchema pipeModelSchema = await JsonSchema.FromFileAsync(jsonSchemaRootDir + "MicroStates.json");
    CSharpGeneratorSettings settings = new CSharpGeneratorSettings();
    settings.Namespace = codeNamespace;
    var pipeModelGenerator = new CSharpGenerator(pipeModelSchema, settings);
    var pipeModelFile = pipeModelGenerator.GenerateFile();
    using (StreamWriter writer = new StreamWriter(sourceCodeDir + "MicroStatesFromJson.cs"))
    {
        writer.WriteLine(pipeModelFile);
    }
}
