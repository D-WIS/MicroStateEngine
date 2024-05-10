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
                    string tempFile = Path.Combine(dir.FullName, "SemanticQueriesAxialVelocityTopOfString.md");
                    using (StreamWriter writer = new StreamWriter(tempFile))
                    {
                        var queries1 = GeneratorSparQLManifestFile.GetSparQLQueries(assembly, typeof(SignalGroup).FullName, "AxialVelocityTopOfString");
                        GenerateSparQLForMD(writer, "AxialVelocityTopOfString", queries1);
                    }
                }
            }
        }
    }
}