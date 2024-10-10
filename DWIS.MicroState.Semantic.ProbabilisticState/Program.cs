using DWIS.MicroState.Model;
using OSDC.DotnetLibraries.Drilling.DrillingProperties;
using System.Reflection;

namespace DWIS.MicroState.Semantic.DeterministicState
{
    public class Program
    {
        static void GenerateMermaidForMD(StreamWriter writer, string className, string propertyName, string? mermaid)
        {
            if (writer != null && !string.IsNullOrEmpty(className) && !string.IsNullOrEmpty(mermaid))
            {
                writer.WriteLine("# Semantic Graph for property `" + propertyName + "` of class `" + className + "`");
                writer.WriteLine(mermaid);
            }
        }
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
            Assembly? assembly = Assembly.GetAssembly(typeof(ProbabilisticMicroStates));
            if (assembly != null)
            {
                string tempPath = Directory.GetCurrentDirectory();
                DirectoryInfo? dir = new DirectoryInfo(tempPath);
                dir = dir?.Parent?.Parent?.Parent;
                if (dir != null)
                {
                    string tempFile = Path.Combine(dir.FullName, "SemanticProbabilisticMicroStates.md");
                    using (StreamWriter writer = new StreamWriter(tempFile))
                    {
                        var manifestFile = GeneratorSparQLManifestFile.GetManifestFile(assembly, typeof(ProbabilisticMicroStates).FullName, "ProbabilisticMicroStates", "DWIS", "DWIS:");
                        if (manifestFile != null)
                        {
                            GenerateMermaidForMD(writer, "ProbabilisticMicroStates", GeneratorSparQLManifestFile.GetMermaid(manifestFile));
                        }
                        var queries1 = GeneratorSparQLManifestFile.GetSparQLQueries(assembly, typeof(ProbabilisticMicroStates).FullName);
                        GenerateSparQLForMD(writer, "ProbabilisticMicroStates", queries1);
                    }
                    tempFile = Path.Combine(dir.FullName, "SemanticProbabilisticMicroStatesAxialVelocityTopOfString.md");
                    using (StreamWriter writer = new StreamWriter(tempFile))
                    {
                        var manifestFile = GeneratorSparQLManifestFile.GetManifestFile(assembly, typeof(ProbabilisticMicroStates).FullName, "AxialVelocityTopOfString", "ProbabilisiticMicroStateAxialVelocityTopOfString", "DWIS", "DWIS:");
                        if (manifestFile != null)
                        {
                            GenerateMermaidForMD(writer, "ProbabilisticMicroStates", "AxialVelocityTopOfString", GeneratorSparQLManifestFile.GetMermaid(manifestFile));
                        }
                    }
                }
            }
        }
    }
}