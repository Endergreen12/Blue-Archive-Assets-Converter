using MemoryPack;
using Blue_Archive_Classes;
using static Utils.Ask;

var newLineStr = Environment.NewLine;
var specifiedCatalogType = CatalogType.MediaCatalog;
var assetFolderPath = "";
var catalogBinPath = "";
var outputFolderPath = "";

// Ask CatalogType
if(args.Length == 0 || args.Length >= 1 && !Enum.TryParse(args[0], out specifiedCatalogType))
{
    while(true)
    {
        Console.WriteLine("Catalog List:");
        foreach(var catalog in Enum.GetValues<CatalogType>())
        {
            Console.WriteLine((int)catalog + ": " + catalog); // "0: MediaCatalog"
        }
        Console.WriteLine();
        var specifiedCatalogTypeStr = AskStringForUser("Please enter the Catalog you wish to convert" + newLineStr + "Please select from the Catalog List above" + newLineStr + "You can select by number, but you can also type the name as it is");
        if (Enum.TryParse(specifiedCatalogTypeStr, out specifiedCatalogType) && (int)specifiedCatalogType < Enum.GetValues<CatalogType>().Length)
            break;

        Console.WriteLine("Parse to CatalogType failed" + newLineStr + "Please enter it again");
        Console.WriteLine();
    }
}

assetFolderPath = AskAndValidPath(false, "Enter the path of the asset folder", 1, args);
catalogBinPath = AskAndValidPath(true, "Enter the path of the catalog binary", 2, args);
outputFolderPath = AskAndValidPath(false, "Enter the path of the output destination", 3, args);

Directory.SetCurrentDirectory(outputFolderPath);

Console.WriteLine("Start copying" + newLineStr + "Logs are not displayed to improve processing speed");

switch(specifiedCatalogType)
{
    case CatalogType.MediaCatalog:
        CopyFilesWithMediaCatalog(catalogBinPath);
        break;

    case CatalogType.TableCatalog:
        CopyFilesWithTableCatalog(catalogBinPath);
        break;
}

Console.WriteLine("Done" + newLineStr + "Press enter key to exit...");
Console.ReadLine();

void CopyFilesWithMediaCatalog(string catalogBinPath)
{
    var catalog = MemoryPackSerializer.Deserialize<MediaCatalog>(File.ReadAllBytes(catalogBinPath));
    if(catalog == null)
    {
        Console.WriteLine("Failed to load catalog");
        return;
    }
    foreach(var pair in catalog.Table)
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
}

void CopyFilesWithTableCatalog(string catalogBinPath)
{
    var catalog = MemoryPackSerializer.Deserialize<TableCatalog>(File.ReadAllBytes(catalogBinPath));
    if (catalog == null)
    {
        Console.WriteLine("Failed to load catalog");
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

enum CatalogType
{
    MediaCatalog,
    TableCatalog
}