using DWIS.MicroState.Model;
using OSDC.DotnetLibraries.Drilling.DrillingProperties;
using System.Reflection;

namespace DWIS.MicroState.Semantic.DeterministicState
{
    public class Program
    {
        static void GenerateMermaidForMD(StreamWriter writer, string className, string? mermaid)
        {
            if (writer != null && !string.IsNullOrEmpty(className) && !string.IsNullOrEmpty(mermaid))
            {
                writer.WriteLine("# Semantic Graph for `" + className + "`");
                writer.WriteLine(mermaid);
            }
        }
        static void GenerateSparQLForMD(StreamWriter writer, string className, Dictionary<string, QuerySpecification>? queries)
        {
            if (writer != null && !string.IsNullOrEmpty(className) && queries != null)
            {
                writer.WriteLine("# Semantic Queries for `" + className + "`");
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
        static void Main()
        {
            Assembly? assembly = Assembly.GetAssembly(typeof(MicroStates));
            if (assembly != null)
            {
                string tempPath = Directory.GetCurrentDirectory();
                DirectoryInfo? dir = new DirectoryInfo(tempPath);
                dir = dir?.Parent?.Parent?.Parent;
                if (dir != null)
                {
                    string tempFile = Path.Combine(dir.FullName, "SemanticDeterministicMicroStates.md");
                    using (StreamWriter writer = new StreamWriter(tempFile))
                    {
                        var manifestFile = GeneratorSparQLManifestFile.GetManifestFile(assembly, typeof(MicroStates).FullName, "DeterministicMicroState", "DWIS", "DWIS:");
                        if (manifestFile != null)
                        {
                            GenerateMermaidForMD(writer, "MicroStates", GeneratorSparQLManifestFile.GetMermaid(manifestFile));
                        }
                        var queries1 = GeneratorSparQLManifestFile.GetSparQLQueries(assembly, typeof(MicroStates).FullName);
                        GenerateSparQLForMD(writer, "MicroStates", queries1);
                    }
                }
            }
        }
    }
}