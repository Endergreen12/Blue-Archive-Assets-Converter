using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Blue_Archive_Assets_Converter
{
    internal class Utils
    {
        const string APP_NAME = "Blue Archive Assets Converter";
        public static string mediaCatalogJson = "MediaCatalog.json";
        public static bool verboseMode = false;
        public static List<string> GetTypeList(string jsonPath, string type) // どうやってJsonを読み込めばいいのかわからなかったのでごり押しです 非効率的すぎワロタ
        {
            if (verboseMode)
                Console.WriteLine("タイプ\"{0}\"のListの構築を開始", type);

            string json = File.ReadAllText(Path.Combine(jsonPath, mediaCatalogJson));
            string[] dataArray = json.Split(",");
            string[] toBeRemovedStrArray = { "\\", "\"", ":", type };
            List<string> processedList = new List<string>();
            for(int i = 0; i <= dataArray.Length - 1; i++)
            {
                string targetString = dataArray[i];
                if(targetString.Contains(type))
                {
                    foreach(string toBeRemovedStr in toBeRemovedStrArray)
                    {
                        targetString = targetString.Replace(toBeRemovedStr, "");
                    }
                    processedList.Add(targetString);

                    if (verboseMode)
                        Console.WriteLine(targetString);
                }
            }
            return processedList;
        }

        public enum Status
        {
            Init,
            Copying,
            Done
        }
        public static void UpdateConsoleTitle(Status status, int arg1 = 0, int arg2 = 0)
        {
            string title;
            switch(status)
            {
                case Status.Copying:
                    title = APP_NAME + " - " + $"ファイルをコピー中 ({arg1} / {arg2})";
                    break;

                case Status.Done:
                    title = APP_NAME + " - " + "完了";
                    break;

                default:
                    title = APP_NAME;
                    break;
            }

            Console.Title = title;
        }

        // FileCompare関数はMicrosoftさんの"Visual C# を使用してFile-Compare関数を作成する"からいただきました。

        // This method accepts two strings the represent two files to
        // compare. A return value of 0 indicates that the contents of the files
        // are the same. A return value of any other value indicates that the
        // files are not the same.
        public static bool FileCompare(string file1, string file2)
        {
            int file1byte;
            int file2byte;
            FileStream fs1;
            FileStream fs2;

            // Determine if the same file was referenced two times.
            if (file1 == file2)
            {
                // Return true to indicate that the files are the same.
                return true;
            }

            // Open the two files.
            fs1 = new FileStream(file1, FileMode.Open);
            fs2 = new FileStream(file2, FileMode.Open);

            // Check the file sizes. If they are not the same, the files
            // are not the same.
            if (fs1.Length != fs2.Length)
            {
                // Close the file
                fs1.Close();
                fs2.Close();

                // Return false to indicate files are different
                return false;
            }

            // Read and compare a byte from each file until either a
            // non-matching set of bytes is found or until the end of
            // file1 is reached.
            do
            {
                // Read one byte from each file.
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            }
            while ((file1byte == file2byte) && (file1byte != -1));

            // Close the files.
            fs1.Close();
            fs2.Close();

            // Return the success of the comparison. "file1byte" is
            // equal to "file2byte" at this point only if the files are
            // the same.
            return ((file1byte - file2byte) == 0);
        }
    }
}
