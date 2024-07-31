using Blue_Archive_Assets_Converter;
using MemoryPack;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using static Blue_Archive_Assets_Converter.Func;

Console.WriteLine("Blue Archive Assets Converter v{0} | Endergreen12", Assembly.GetExecutingAssembly().GetName().Version);
breakLine();

Language language;
if (args.Length == 0 || !Enum.TryParse<Language>(args[0], out language))
{
    language = getUserLanguage();
}

var targetFolderPath = "";
Console.WriteLine(getLocalizedString(Message.EnterPath, language), "MediaPatch");
targetFolderPath = Console.ReadLine();

if(targetFolderPath == null)
{
    targetFolderPath = "";
}

targetFolderPath = targetFolderPath.Replace("\"", ""); // Remove double quatation from path | パスからダブルクォーテーションを削除する
if(!Directory.Exists(targetFolderPath))
{
    Console.WriteLine(getLocalizedString(Message.FolderDoesNotExist, language));
    pressAnyKeyToExit(language);
}

var targetBinPath = "";
Console.WriteLine(getLocalizedString(Message.SpecifyBinary, language));
targetBinPath = Console.ReadLine();

breakLine();

var specifiedMediaType = MediaType.None;
var userSpecifiedMediaType = "";
Console.WriteLine(getLocalizedString(Message.SpecifyMediaType, language));
Console.WriteLine(String.Join(Environment.NewLine, Enum.GetNames<MediaType>())); // List of MediaType | MediaTypeの一覧
userSpecifiedMediaType = Console.ReadLine();
if(userSpecifiedMediaType != "" && !Enum.TryParse(userSpecifiedMediaType, out specifiedMediaType))
{
    Console.WriteLine(getLocalizedString(Message.FailedToParseMediaType, language));
    pressAnyKeyToExit(language);
}

Console.WriteLine(getLocalizedString(Message.MediaCatalogLoading, language));
byte[] catalogBin = File.ReadAllBytes(catalogBinPath);
var mediaCatalog = MemoryPackSerializer.Deserialize<MediaCatalog>(catalogBin);
if (mediaCatalog == null)
{
    Console.WriteLine("mediaCatalog is null");
    Console.ReadLine();
    Environment.Exit(0);
}

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
var sw = new Stopwatch();
sw.Start();
int num = 1;
foreach (var catalog in mediaCatalog.Table)
{
    Media media = catalog.Value;

    if (specifiedMediaType != MediaType.None && media.MediaType != specifiedMediaType)
    {
        continue;
    }

    string[] srcFileArray = Directory.GetFiles(targetFolderPath, "*_" + media.Crc.ToString());
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
                {
                    isSame = false;
                    break;
                }
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

        if (num == mediaCatalog.Table.Count)
            breakLine();
    } else
    {
        Console.WriteLine(getLocalizedString(isSame ? Message.FileExist : Message.SourceFileNotFound, language), media.Path, "*_" + media.Crc.ToString());
        curPos = Console.GetCursorPosition();
        lastLogLength = 0;
    }

    num++;
}
sw.Stop();
breakLine();

Console.WriteLine(getLocalizedString(Message.Done, language), outputFolderName, Directory.GetCurrentDirectory(), sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.ElapsedMilliseconds);
pressAnyKeyToExit(language);