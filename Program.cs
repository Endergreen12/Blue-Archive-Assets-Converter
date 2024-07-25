using Blue_Archive_Assets_Converter;
using MemoryPack;
using System.Reflection;
using System.Security.Cryptography;
using static Blue_Archive_Assets_Converter.Func;

Console.WriteLine("Blue Archive Assets Converter v{0} | Endergreen12", Assembly.GetExecutingAssembly().GetName().Version);
breakLine();

string mediaPatchPath = "";
Language language;
if (args.Length == 0 || !Enum.TryParse<Language>(args[0], out language))
{
    language = getUserLanguage();
}

Console.WriteLine(getLocalizedString(Message.EnterFolderPath, language));
mediaPatchPath = Console.ReadLine();

mediaPatchPath = mediaPatchPath.Replace("\"", ""); // Remove double quatation from path | パスからダブルクォーテーションを削除する
if(!Directory.Exists(mediaPatchPath))
{
    Console.WriteLine(getLocalizedString(Message.FolderDoesNotExist, language));
    pressAnyKey(language);
}
breakLine();

string mediaCatalogBinName = "MediaCatalog.bytes";
string catalogBinPath = Path.Combine(mediaPatchPath, mediaCatalogBinName);
if(!File.Exists(catalogBinPath))
{
    Console.WriteLine(getLocalizedString(Message.SpecifyBinary, language), mediaCatalogBinName);
    string specifiedBinPath = Console.ReadLine();
    specifiedBinPath = specifiedBinPath.Replace("\"", "");

    if (!File.Exists(specifiedBinPath))
    {
        Console.WriteLine(getLocalizedString(Message.BinaryNotFound, language), mediaCatalogBinName);
        pressAnyKey(language);
    }

    catalogBinPath = specifiedBinPath;
}
breakLine();

MediaType specifiedMediaType = MediaType.None;
string userSpecifiedMediaType = "";
Console.WriteLine(getLocalizedString(Message.SpecifyMediaType, language));
Console.WriteLine(String.Join(Environment.NewLine, Enum.GetNames<MediaType>())); // List of MediaType | MediaTypeの一覧
userSpecifiedMediaType = Console.ReadLine();
if(userSpecifiedMediaType != "" && !Enum.TryParse(userSpecifiedMediaType, out specifiedMediaType))
{
    Console.WriteLine(getLocalizedString(Message.FailedToParseMediaType, language));
    pressAnyKey(language);
}
breakLine();

Console.WriteLine(getLocalizedString(Message.MediaCatalogLoading, language));
byte[] catalogBin = File.ReadAllBytes(catalogBinPath);
MediaCatalog mediaCatalog = MemoryPackSerializer.Deserialize<MediaCatalog>(catalogBin);

string outputFolderName = "output";
if(!Directory.Exists(outputFolderName))
{
    Directory.CreateDirectory(outputFolderName);
    Console.WriteLine(getLocalizedString(Message.DirCreated, language), outputFolderName);
}
Directory.SetCurrentDirectory(outputFolderName);

Console.WriteLine(getLocalizedString(Message.CopyStart, language), outputFolderName);
var curPos = Console.GetCursorPosition();
var lastLogLength = 0;
foreach (KeyValuePair<string, Media> catalog in mediaCatalog.Table)
{
    Media media = catalog.Value;

    if (specifiedMediaType != MediaType.None && media.MediaType != specifiedMediaType)
    {
        continue;
    }

    string[] srcFileArray = Directory.GetFiles(mediaPatchPath, "*_" + media.Crc.ToString());
    bool isSame = false;
    if (srcFileArray.Length > 0)
    {
        if(File.Exists(media.Path))
        {
            byte[] srcFileHash = MD5.Create().ComputeHash(File.ReadAllBytes(srcFileArray[0]));
            byte[] existFileHash = MD5.Create().ComputeHash(File.ReadAllBytes(media.Path));
            isSame = true;
            for(int i = 0; i < srcFileHash.Length; i++)
            {
                if (srcFileHash[i] != existFileHash[i])
                    isSame = false;
            }
        }
        if (!isSame)
        {
            string[] newDirectoryArray = media.Path.Split('/');
            Directory.CreateDirectory(Path.Combine(newDirectoryArray.SkipLast(1).ToArray()));
            File.Copy(srcFileArray[0], media.Path, true);
        }
    }

    Console.SetCursorPosition(curPos.Left, curPos.Top);
    if (lastLogLength > 0)
        Console.Write(new string(' ', lastLogLength)); // すでに出力されているログを空白で埋めて消去 | Delete log already output with filling with space
    Console.SetCursorPosition(curPos.Left, curPos.Top);

    if(srcFileArray.Length > 0 && !isSame)
    {
        string log = srcFileArray[0] + " -> " + media.Path;
        lastLogLength = log.Length;
        Console.Write(log);
    } else
    {
        Console.WriteLine(getLocalizedString(isSame ? Message.FileExist : Message.SourceFileNotFound, language), media.Path, "*_" + media.Crc.ToString());
        curPos = Console.GetCursorPosition();
        lastLogLength = 0;
    }
}
breakLine();

Console.WriteLine(getLocalizedString(Message.Done, language), outputFolderName, Directory.GetCurrentDirectory());
pressAnyKey(language);