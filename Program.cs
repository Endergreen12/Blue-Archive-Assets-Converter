using Blue_Archive_Assets_Converter;
using MemoryPack;
using System.Reflection;

Console.WriteLine("Blue Archive Assets Converter v{0} | Endergreen12", Assembly.GetExecutingAssembly().GetName().Version);

string mediaPatchPath = "";

if (args[0] != null && args[0] == "")
{
    mediaPatchPath = args[0];
}

if(!Directory.Exists(mediaPatchPath))
{
    Console.WriteLine("");
}