// 最初にMediaPatchフォルダーをユーザーに指定させ、コピー先フォルダも指定させる。
// pathArrayとcrcArrayを取得する。
// forでcrcArrayを使ってファイルの存在を確認し、もし存在する場合はpathArrayからpathを取得し、"/"で文字列を分割し、最後のファイル名を除いたディレクトリを作成し、
// そこにソースファイルをコピーし、名前を分割した文字列の最後の文字列に変更する。
// もしそこにファイルが既に存在していた場合は、一致するか確認し、しない場合は削除してコピーしなおす
// TODO: もっといい方法を見つける

using static Blue_Archive_Assets_Converter.Utils;

UpdateConsoleTitle(Status.Init);

Console.WriteLine("{0}を含むディレクトリのフルパスを指定してください。\nおそらくMediaPatchという名前のフォルダだと思います", mediaCatalogJson);
string? path = Console.ReadLine();
Console.WriteLine();
Console.WriteLine("保存先のフォルダのフルパスを指定してください。");
string? destPath = Console.ReadLine();
Console.WriteLine();
Console.WriteLine("変換したいフォルダを指定してください。\n例: Audio/Cafe/Scenario/Video\nもしすべてのフォルダを変換したい場合は空欄にしてください。");
string? folderName = Console.ReadLine();
Console.WriteLine();
Console.WriteLine("ログを表示しますか？表示すると処理速度が低下します。\n速度を早くしたい場合は表示しないことをお勧めします。表示しなくてもタイトルバーで進捗状況を確認できます。(Y/N)");
string? verboseCheck = Console.ReadLine();
if (verboseCheck != null && verboseCheck == "Y" || verboseCheck == "y")
    verboseMode = true;

Console.WriteLine();

List<string> pathList = new List<string>();
List<string> crcList = new List<string>();
List<string> fileNameList = new List<string>();
if (path != null)
{
    pathList = GetTypeList(path, "path");
    crcList = GetTypeList(path, "Crc");
    fileNameList = GetTypeList(path, "fileName");
}

if(folderName != null && folderName != "") // 変換するフォルダが指定された場合、ここでそのフォルダ以外のものをリストから削除
{
    if (verboseMode)
        Console.WriteLine("変換フォルダが指定されているため、リストの再構築を開始");

    int ogCount = 0;
    while(ogCount != pathList.Count)
    {
        if (!pathList[ogCount].StartsWith(folderName + "/"))
        {
            if (verboseMode)
                Console.WriteLine("リストから削除: {0}", pathList[ogCount]);

            pathList.RemoveAt(ogCount);
            crcList.RemoveAt(ogCount);
            fileNameList.RemoveAt(ogCount);
        } else
            ogCount++;
    }
}

if (verboseMode)
    Console.WriteLine("ファイルの変換を開始");

for (int i = 0; i < pathList.Count; i++)
{
    string[] files = new string[] { };
    if (path != null)
    {
        if (verboseMode)
            Console.WriteLine("ソースファイルが存在するか確認: {0}", $"*_{crcList[i]}.dat");

        files = Directory.GetFiles(path, $"*_{crcList[i]}.dat");
    }
    if (files.Length >= 1)
    {
        string copyPath = pathList[i];
        string willBeCopiedPath = "";
        if (destPath != null)
            willBeCopiedPath = Path.Combine(destPath, copyPath);
        string sourcePath = files[0].Substring(0, files[0].IndexOf("_"));
        if (destPath != null)
        {
            string dir = Path.Combine(destPath, copyPath.Replace(fileNameList[i], ""));
            if (!Directory.Exists(dir))
            {
                if (verboseMode)
                    Console.WriteLine("ディレクトリ\"{0}\"は存在しないので作成します", dir);

                Directory.CreateDirectory(dir);
            }

            if (File.Exists(willBeCopiedPath) && !FileCompare(willBeCopiedPath, sourcePath))
            {
                if (verboseMode)
                    Console.WriteLine("ファイルは既に変換されていましたが、ソースファイルと異なっていたので、削除します。");

                File.Delete(willBeCopiedPath);
            }

            if (!File.Exists(willBeCopiedPath) && File.Exists(sourcePath))
            {
                if (verboseMode)
                    Console.WriteLine("ファイルをコピー: {0}", willBeCopiedPath);

                File.Copy(sourcePath, willBeCopiedPath, false);
            } else
            {
                if (verboseMode)
                    Console.WriteLine("ファイルのコピーをスキップ: {0}", willBeCopiedPath);
            }
        }
    }

    UpdateConsoleTitle(Status.Copying, i, pathList.Count);
}

UpdateConsoleTitle(Status.Done);
Console.WriteLine("完了しました。何かキーを押して終了します...");
Console.Read();