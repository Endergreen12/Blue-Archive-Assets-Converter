using MemoryPack;
using Blue_Archive_Classes;
using static Utils.Utils;
using static Utils.Ask;

Console.WriteLine("Blue Archive Assets Converter");
Console.WriteLine();
DisplayHelpIfArgIsHelp(args, "Usage: Blue-Archive-Assets-Converter [CatalogType] [Asset folder path] " +
    "[Catalog binary path] [Output folder path]" + newLineStr + "Example: Blue-Archive-Assets-Converter MediaCatalog E:\\MediaPatch E:\\MediaPatch\\MediaCatalog.bytes .\\output");

var specifiedCatalogType = CatalogType.MediaCatalog;
var assetFolderPath = "";
var catalogBinPath = "";
var outputFolderPath = "";

// Ask CatalogType
ParseOrAskENumValue<CatalogType>("Please enter the Catalog you wish to convert" + newLineStr + "Please select from the Catalog List above" + newLineStr + "You can select by number, but you can also type the name as it is",
    ref specifiedCatalogType, 0, args);

assetFolderPath = ParseOrAskAndValidPath(false, "Enter the path of the asset folder", 1, args);
catalogBinPath = ParseOrAskAndValidPath(true, "Enter the path of the catalog binary", 2, args);
outputFolderPath = ParseOrAskAndValidPath(false, "Enter the path of the output destination", 3, args);

Directory.SetCurrentDirectory(outputFolderPath);

Console.WriteLine("Start copying" + newLineStr + "Logs are not displayed to improve processing speed");

switch(specifiedCatalogType)
{
    case CatalogType.MediaCatalog:
        CopyFilesWithCatalog(catalogBinPath, typeof(MediaCatalog));
        break;

    case CatalogType.TableCatalog:
        CopyFilesWithCatalog(catalogBinPath, typeof(TableCatalog));
        break;

    default:
        Console.WriteLine($"Error: The specified CatalogType {specifiedCatalogType} is invalid");
        break;
}

Console.WriteLine("Done" + newLineStr + "Press enter key to exit...");
Console.ReadLine();

void CopyFilesWithCatalog(string catalogBinPath, Type type)
{
    byte[]? catalogBin = null;
    if(File.Exists(catalogBinPath))
    {
        catalogBin = File.ReadAllBytes(catalogBinPath);
    } else
    {
        Console.WriteLine("Catalog binary file not found");
        return;
    }

    if(catalogBin == null)
    {
        Console.WriteLine("Failed to load catalog binary - variable is null");
        return;
    }

    if (type == typeof(MediaCatalog))               // MediaCatalog copy process
    {
        var catalog = MemoryPackSerializer.Deserialize<MediaCatalog>(catalogBin);
        if (catalog == null)
        {
            Console.WriteLine("Failed to load catalog - variable is null");
            return;
        }

        foreach (var pair in catalog.Table)
        {
            var media = pair.Value;
            var srcFileArray = Directory.GetFiles(assetFolderPath, "*_" + media.Crc.ToString());
            if (srcFileArray.Length > 0)
            {
                var pathArray = media.Path.Split("/");
                Directory.CreateDirectory(Path.Combine(pathArray.SkipLast(1).ToArray())); // Remove file name from path and create directories
                File.Copy(srcFileArray[0], Path.Combine(pathArray));
            }
        }
    } else if(type == typeof(TableCatalog))         // TableCatalog copy process
    {
        var catalog = MemoryPackSerializer.Deserialize<TableCatalog>(catalogBin);
        if (catalog == null)
        {
            Console.WriteLine("Failed to load catalog - variable is null");
            return;
        }

        foreach (var pair in catalog.Table)
        {
            var tableBundle = pair.Value;

            var srcFileArray = Directory.GetFiles(assetFolderPath, "*_" + tableBundle.Crc.ToString());
            if (srcFileArray.Length > 0)
            {
                File.Copy(srcFileArray[0], tableBundle.Name);
            }
        }
    }
}

enum CatalogType
{
    MediaCatalog,
    TableCatalog
}