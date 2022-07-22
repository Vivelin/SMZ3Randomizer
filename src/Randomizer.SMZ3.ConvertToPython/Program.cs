// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.RegularExpressions;
using CSharpToPython;
using Randomizer.Shared;
using Randomizer.SMZ3;
/*
foreach (ItemType i in Enum.GetValues(typeof(ItemType)))
{
Console.Write($"{Enum.GetName(typeof(ItemType), i)}");
Console.WriteLine($" {(int)i}");
}
*/

Config config = new Config();
World world = new World(config, "", 1, "");

var pythonDirectory = Path.Combine(GetSourceDirectory(), "Randomizer.SMZ3.ConvertToPython\\PythonFiles");
var convertedDirectory = Path.Combine(GetSourceDirectory(), "Randomizer.SMZ3.ConvertToPython\\PythonFiles\\Converted");
var regionsDirectory = Path.Combine(GetSourceDirectory(), "Randomizer.SMZ3.ConvertToPython\\PythonFiles\\Converted\\Regions");

System.IO.Directory.CreateDirectory(regionsDirectory);


EnumToPyFile<WallJumpDifficulty>(Path.Combine(convertedDirectory, "WallJumpDifficulty.py"));
EnumToPyFile<ItemType>(Path.Combine(convertedDirectory, "ItemType.py"));
EnumToPyFile<LocationType>(Path.Combine(convertedDirectory, "LocationType.py"));
EnumToPyFile<Reward>(Path.Combine(convertedDirectory, "RewardType.py"), "RewardType");

//var file = Path.Combine(GetSourceDirectory(), "Randomizer.SMZ3\\Regions\\SuperMetroid\\Norfair\\UpperNorfairEast.cs");
var file = Path.Combine(GetSourceDirectory(), "Randomizer.SMZ3\\Regions\\Zelda\\HyruleCastle.cs");

foreach (var region in world.Regions)
{
    RegionToPyFile(region);
}

BasicFileToPyFile(typeof(Location));
BasicFileToPyFile(typeof(Region));
BasicFileToPyFile(typeof(SMRegion));
BasicFileToPyFile(typeof(Z3Region));
BasicFileToPyFile(typeof(Logic), new() { { "public static(.|\\r|\\n)*", "}}" } } );
BasicFileToPyFile(typeof(World));
BasicFileToPyFile(typeof(Config));
BasicFileToPyFile(typeof(Room));
BasicFileToPyFile(typeof(Progression), new() { { "public Progression[^}]+}", "" } } );

string CSharpToPython(string code)
{
    try
    {
        var parsed = Microsoft.CodeAnalysis.CSharp.SyntaxFactory.ParseSyntaxTree(code).GetRoot();
        var rewritten = MultiLineLambdaRewriter.RewriteMultiLineLambdas(parsed);
        var pyAst = new CSharpToPythonConvert().Visit(rewritten);
        var converted = PythonAstPrinter.PrintPythonAst(pyAst);
        return converted;
    }
    catch (Exception ex)
    {
        return ex.ToString();
    }
}

void BasicFileToPyFile(Type type, Dictionary<string, string>? toRemoveRegularExpressions = null)
{
    Console.WriteLine("Creating file for " + type.Name);
    var sourceFilePath = "Randomizer.SMZ3" + type.FullName?.Replace("Randomizer.SMZ3", "").Replace(".", "\\") + ".cs";
    var cSharpCode = File.ReadAllText(Path.Combine(GetSourceDirectory(), sourceFilePath));

    // Clean up C# code to allow files to parse properly
    cSharpCode = Regex.Replace(cSharpCode, "using[^;]+;", "");
    cSharpCode = Regex.Replace(cSharpCode, "new\\s+List\\s*<\\s*[a-z]+\\s*>(\\(\\s*\\))?\\s+{[^}]+}", "null");
    if (toRemoveRegularExpressions != null)
    {
        foreach (var regex in toRemoveRegularExpressions.Keys)
        {
            cSharpCode = Regex.Replace(cSharpCode, regex, toRemoveRegularExpressions[regex]);
        }
    }

    var pythonCode = CSharpToPython(cSharpCode).Split(Environment.NewLine);

    var sb = new StringBuilder();

    var classObjects = new List<string>();
    foreach (var line in pythonCode)
    {
        var trimmedLine = line.Trim();
        if (trimmedLine.StartsWith("def"))
        {
            classObjects.Add(Regex.Replace(trimmedLine.Replace("def ", ""), "\\(.*", ""));
        }
        sb.AppendLine(line);
    }

    foreach (var classObject in classObjects)
    {
        sb.Replace(" " + classObject, " self." + classObject);
        sb.Replace("(" + classObject, "(self." + classObject);
        sb.Replace("def self." + classObject, "def " + classObject);
        sb.Replace("class self." + classObject, "class " + classObject);
        sb.Replace("from self." + classObject, "from " + classObject);
        sb.Replace("import self." + classObject, "import " + classObject);
    }

    var outFile = Path.Combine(convertedDirectory, type.Name + ".py");
    File.WriteAllText(outFile, sb.ToString());

}

void RegionToPyFile(Region region)
{
    var sourceFilePath = "Randomizer.SMZ3" + region.GetType().FullName?.Replace("Randomizer.SMZ3", "").Replace(".", "\\") + ".cs";
    var cSharpCode = File.ReadAllText(Path.Combine(GetSourceDirectory(), sourceFilePath));
    cSharpCode = Regex.Replace(cSharpCode, "using[^;]+;", "");
    cSharpCode = Regex.Replace(cSharpCode, "new\\s+List\\s*<\\s*[a-z]+\\s*>(\\(\\s*\\))?\\s+{[^}]+}", "null");
    var pythonCode = CSharpToPython(cSharpCode).Split(Environment.NewLine);
    var sb = new StringBuilder();

    // Add imports
    sb.AppendLine("from ItemType import ItemType");
    sb.AppendLine("from LocationType import LocationType");
    sb.AppendLine("from RewardType import RewardType");
    sb.AppendLine("from Location import Location");
    sb.AppendLine("from SMRegion import SMRegion");
    sb.AppendLine("from Z3Region import Z3Region");
    sb.AppendLine("from Region import Region");
    sb.AppendLine("from Logic import Logic");
    sb.AppendLine("from World import World");
    sb.AppendLine("from Config import Config");
    sb.AppendLine("from Room import Room");

    var classObjects = new List<string>();
    foreach (var line in pythonCode)
    {
        var trimmedLine = line.Trim();
        if (trimmedLine.StartsWith("def"))
        {
            classObjects.Add(Regex.Replace(trimmedLine.Replace("def ", ""), "\\(.*", ""));
        }
        sb.AppendLine(line);
    }

    foreach (var classObject in classObjects)
    {
        sb.Replace(" " + classObject, " self." + classObject);
        sb.Replace("(" + classObject, "(self." + classObject);
        sb.Replace("def self." + classObject, "def " + classObject);
        sb.Replace("class self." + classObject, "class " + classObject);
        sb.Replace("from self." + classObject, "from " + classObject);
        sb.Replace("import self." + classObject, "import " + classObject);
    }

    sb.Replace("RegionItems = ", "self.RegionItems = ");
    sb.Replace("Reward.None", "RewardType.Empty");
    sb.Replace("self.Reward.", "RewardType.");
    sb.Replace("self.RewardType.", "RewardType.");

    var outFile = Path.Combine(regionsDirectory, region.GetType().Name + ".py");
    File.WriteAllText(outFile, sb.ToString());
}

void EnumToPyFile<t>(string file, string rename = "") where t : Enum
{
    var enumName = string.IsNullOrEmpty(rename) ? typeof(t).Name : rename;
    var builder = new StringBuilder();

    builder.AppendLine("from enum import Enum");
    builder.AppendLine();
    builder.AppendLine($"class {enumName}(Enum):");

    foreach (t i in Enum.GetValues(typeof(t)))
    {
        var name = Enum.GetName(typeof(t), i) == "None" ? "Empty" : Enum.GetName(typeof(t), i);
        var value = Convert.ChangeType(i, Enum.GetUnderlyingType(typeof(t)));
        builder.AppendLine($"\t{name} = {value}");
    }

    File.WriteAllText(file, builder.ToString());
}

string GetSourceDirectory()
{
    var currentDirectory = System.IO.Directory.GetCurrentDirectory();
    var directory = Directory.GetParent(currentDirectory);
    while (directory.Name != "src")
    {
        directory = Directory.GetParent(directory.FullName);
    }
    return directory.FullName;
}
