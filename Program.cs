using Blue_Archive_Assets_Converter;
using static Blue_Archive_Assets_Converter.Func;
using MemoryPack;
using System.Reflection;

Console.WriteLine("Blue Archive Assets Converter v{0} | Endergreen12", Assembly.GetExecutingAssembly().GetName().Version);
breakLine();

string mediaPatchPath = "";
Language language = getUserLanguage();


/*                                                                         
                            Preparation | 準備                           
                                                                          */


Console.WriteLine(getLocalizedString(Message.EnterFolderPath, language));
mediaPatchPath = Console.ReadLine();

mediaPatchPath = mediaPatchPath.Replace("\"", ""); // Remove double quatation from path | パスからダブルクォーテーションを削除する
if(!Directory.Exists(mediaPatchPath)) // Exit program if directory does not exist | ディレクトリが存在しなかったらプログラムを終了する
{
    Console.WriteLine(getLocalizedString(Message.FolderDoesNotExist, language));
    pressAnyKey(language);
}

breakLine();

string mediaCatalogBinName = "MediaCatalog.bytes";
string catalogBinPath = Path.Combine(mediaPatchPath, mediaCatalogBinName);
if(!File.Exists(catalogBinPath)) // If MediaCatalog.bytes is not in the specified folder, ask the user for the binary path
{                                // 指定されたフォルダの中にMediaCatalog.bytesがない場合はユーザーにバイナリのパスを求める
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


/*                                                                         
                            Loading | ローディング                           
                                                                          */


Console.WriteLine(getLocalizedString(Message.MediaCatalogLoading, language));

// Load MediaCatalog | MediaCatalogを読み込む
byte[] catalogBin = File.ReadAllBytes(catalogBinPath);
MediaCatalog mediaCatalog = MemoryPackSerializer.Deserialize<MediaCatalog>(catalogBin);

string outputFolderName = "output";
if(!Directory.Exists(outputFolderName))
{
    Directory.CreateDirectory(outputFolderName);
    Console.WriteLine(getLocalizedString(Message.DirCreated, language), outputFolderName);
}
Directory.SetCurrentDirectory(outputFolderName);

// Copy the files using the information in each stored Media
// 格納されたそれぞれのMediaの情報を使いファイルをコピーする
Console.WriteLine(getLocalizedString(Message.CopyStart, language), outputFolderName);
var curPos = Console.GetCursorPosition();
var lastLogLength = 0;
foreach (KeyValuePair<string, Media> catalog in mediaCatalog.Table)
{
    Media media = catalog.Value;
    string[] gotFiles = Directory.GetFiles(mediaPatchPath, "*_" + media.Crc.ToString());
    // Since the file name contains Crc, use it to confirm its existence
    // ファイル名にCrcを含んでいるのでそれを利用して存在を確認する
    // TODO: Reveal the mysterious UInt64 numbers in front of the Crc | Crcの前についてる謎のUInt64の数字の正体を明かす
    // This one is a mystery | こいつが謎 -> [652901576978586]_[4235271580] <- This is Crc | これはCrc
    if (gotFiles.Length > 0)
    {
        // Create directory | ディレクトリを作成                                        // Example | 例 (Path: "Audio/VOC_JP/JP_Arona/Arona_Work_Sleep_In_2.ogg"):
        string[] newDirectoryArray = media.Path.Split('/');                             // ["Audio", "VOC_JP", "JP_Arona", "Arona_Work_Sleep_In_2.ogg"]
        Directory.CreateDirectory(String.Join("\\", newDirectoryArray.SkipLast(1)));    // "Audio\\VOC_JP\\JP_Arona"
        File.Copy(gotFiles[0], media.Path, true);                                       // 17395964499024812656_2952918613 will be copied to output\Audio\VOC_JP\JP_Arona\Arona_Work_Sleep_In_2.ogg

        // Log output on the same line, but it covers the previous output, so delete the previous output
        // 同じ行でログを出力するが前の出力にかぶさるので前の出力を消す
        string log = gotFiles[0] + " -> " + media.Path;

        if(lastLogLength > 0)
            Console.Write(new string(' ', lastLogLength));

        lastLogLength = log.Length;
        Console.SetCursorPosition(curPos.Left, curPos.Top);
        Console.Write(log);
    }
}

breakLine(2);
Console.WriteLine(getLocalizedString(Message.Done, language), outputFolderName, Directory.GetCurrentDirectory());
pressAnyKey(language);