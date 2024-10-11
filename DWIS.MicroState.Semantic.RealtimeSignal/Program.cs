using DWIS.MicroState.Model;
using OSDC.DotnetLibraries.Drilling.DrillingProperties;
using System.Reflection;

namespace DWIS.MicroState.Semantic.DeterministicState
{
    public class Program
    {
        static void GenerateSparQLForMD(StreamWriter writer, string propertyName, Dictionary<string, QuerySpecification>? queries)
        {
            if (writer != null && !string.IsNullOrEmpty(propertyName) && queries != null)
            {
                writer.WriteLine("# Semantic Queries for `" + propertyName + "`");
                foreach (var query in queries)
                {
                    if (query.Value != null)
                    {
                        writer.WriteLine("## " + query.Key);
                        writer.WriteLine("```sparql");
                        writer.WriteLine(query.Value.SparQL);
                        writer.WriteLine("```");
                    }
                }
            }
        }
        static void GenerateMermaidForMD(StreamWriter writer, string name, string? mermaid)
        {
            if (writer != null && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(mermaid))
            {
                writer.WriteLine("# Semantic Graph for `" + name + "`");
                writer.WriteLine(mermaid);
            }
        }
        static void Main()
        {
            Assembly? assembly = Assembly.GetAssembly(typeof(SignalGroup));
            if (assembly != null)
            {
                string tempPath = Directory.GetCurrentDirectory();
                DirectoryInfo? dir = new DirectoryInfo(tempPath);
                dir = dir?.Parent?.Parent?.Parent;
                if (dir != null)
                {
                    string tempFile = Path.Combine(dir.FullName, "SemanticSignals.md");
                    using (StreamWriter writer = new StreamWriter(tempFile))
                    {
                        var manifestFile = GeneratorSparQLManifestFile.GetManifestFile(assembly, typeof(SignalGroup).FullName, "SignalGroup", "DWIS", "DWIS:");
                        if (manifestFile != null)
                        {
                            GenerateMermaidForMD(writer, "SignalGroup", GeneratorSparQLManifestFile.GetMermaid(manifestFile));
                        }
                        var queries1 = GeneratorSparQLManifestFile.GetSparQLQueries(assembly, typeof(SignalGroup).FullName);
                        GenerateSparQLForMD(writer, "SignalGroup", queries1);
                    }
                    tempFile = Path.Combine(dir.FullName, "SemanticQueriesAxialVelocityTopOfString.md");
                    using (StreamWriter writer = new StreamWriter(tempFile))
                    {
                        var queries1 = GeneratorSparQLManifestFile.GetSparQLQueries(assembly, typeof(SignalGroup).FullName, "AxialVelocityTopOfString");
                        GenerateSparQLForMD(writer, "AxialVelocityTopOfString", queries1);
                    }
                    tempFile = Path.Combine(dir.FullName, "SemanticGraphsUCS.md");
                    using (StreamWriter writer = new StreamWriter(tempFile))
                    {
                        var manifestFile1 = GeneratorSparQLManifestFile.GetManifestFile(assembly, typeof(SignalGroup).FullName, "UCS", "UCS", "DWIS", "DWIS:");
                        if (manifestFile1 != null)
                        {
                            GenerateMermaidForMD(writer, "UCS", GeneratorSparQLManifestFile.GetMermaid(manifestFile1));
                        }
                        var manifestFile2 = GeneratorSparQLManifestFile.GetManifestFile(assembly, typeof(SignalGroup).FullName, "UCSSlope", "UCSSlope", "DWIS", "DWIS:");
                        if (manifestFile2 != null)
                        {
                            GenerateMermaidForMD(writer, "UCSSlope", GeneratorSparQLManifestFile.GetMermaid(manifestFile2));
                        }
                    }
                }
            }
        }
    }
}