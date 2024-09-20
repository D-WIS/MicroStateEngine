using OSDC.DotnetLibraries.Drilling.DrillingProperties;
using OSDC.UnitConversion.Conversion.DrillingEngineering;
using DWIS.Vocabulary.Schemas;
using System.Reflection;
using DWIS.MicroState.Model;

namespace DWIS.MicroState.ExportSemanticDescription
{
    class Program
    {
        static void GenerateDWISForMD(StreamWriter writer, string propertyName, string? semantic)
        {
            if (writer != null && !string.IsNullOrEmpty(propertyName) && !string.IsNullOrEmpty(semantic))
            {
                writer.WriteLine("# Semantic Facts for `" + propertyName + "`");
                writer.WriteLine(semantic);
            }
        }
        static void GenerateMermaidForMD(StreamWriter writer, string propertyName, string? mermaid)
        {
            if (writer != null && !string.IsNullOrEmpty(propertyName) && !string.IsNullOrEmpty(mermaid))
            {
                writer.WriteLine("# Semantic Graph for `" + propertyName + "`");
                writer.WriteLine(mermaid);
            }
        }

        static void Main()
        {
            SignalGroup signalGroup = new SignalGroup();
            Assembly? assembly = Assembly.GetAssembly(typeof(SignalGroup));
            if (assembly != null)
            {
                string tempPath = Directory.GetCurrentDirectory();
                DirectoryInfo? dir = new DirectoryInfo(tempPath);
                dir = dir?.Parent?.Parent?.Parent;
                if (dir != null)
                {
                    string tempFile = Path.Combine(dir.FullName, "SignalGroupSemantics.md");
                    Type type = typeof(SignalGroup);
                    using (StreamWriter writer = new StreamWriter(tempFile))
                    {
                        PropertyInfo[] properties = type.GetProperties();
                        List<int> options = new List<int>() { 1, 11 };
                        // Print property information
                        foreach (PropertyInfo property in properties)
                        {
                            var manifestFile1 = GeneratorSparQLManifestFile.GetManifestFile(assembly, typeof(SignalGroup).FullName, property.Name, "DWIS-MicroState-Engine-Manifest", "DWIS", "DWIS:DigitalTwin:", options);
                            if (manifestFile1 != null)
                            {
                                GenerateDWISForMD(writer, property.Name, GeneratorSparQLManifestFile.GetDWIS(manifestFile1));
                                GenerateMermaidForMD(writer, property.Name, GeneratorSparQLManifestFile.GetMermaid(manifestFile1));
                            }
                        }
                    }
                }
            }
        }
    }
}
