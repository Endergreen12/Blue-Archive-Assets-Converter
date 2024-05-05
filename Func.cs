using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blue_Archive_Assets_Converter
{
    internal class Func
    {
        public enum Message
        {
            PressAnyKeyToExit,
            EnterFolderPath,
            FolderDoesNotExist,
            SpecifyBinary,
            BinaryNotFound,
            MediaCatalogLoading,
            DirCreated,
            CopyStart,
            Done
        }

        public enum Language
        {
            English,
            Japanese
        }

        public static void pressAnyKey(Language lang)
        {
            Console.WriteLine(getLocalizedString(Message.PressAnyKeyToExit, lang));
            Console.ReadKey();
            Environment.Exit(0);
        }

        // Localization | ローカライゼーション

        public static Language getUserLanguage()
        {
            string country = System.Globalization.CultureInfo.CurrentCulture.Name;
            Language language = Language.English; // Default language is English | デフォルトの言語は英語
            // Use switch statements to account for the possibility of adding languages later
            // 後から言語を追加することも考慮してswitchステートメントを使う
            switch (country)
            {
                case "ja-JP":
                    language = Language.Japanese;
                    break;
            }

            return language;
        }

        public static string getLocalizedString(Message message, Language language) // TODO: Find a better way to localize | よりよいローカライズの方法を見つける
        {                                                                           // default in switch means English | switchのdefaultは英語
            string locString = "";
            switch(message)
            {
                case Message.PressAnyKeyToExit:
                    switch (language)
                    {
                        case Language.Japanese:
                            locString = "何かキーを押して終了します...";
                            break;

                        default:
                            locString = "Press any key to exit...";
                            break;
                    }
                    break;

                case Message.EnterFolderPath:
                    switch(language)
                    {
                        case Language.Japanese:
                            locString = "MediaPatchフォルダーのパスを入力してください\n(例:D:\\BlueArchive\\com.YostarJP.BlueArchive\\files\\MediaPatch)";
                            break;

                        default:
                            locString = "Enter the path to the MediaPatch folder\n(e.g., D:\\BlueArchive\\com.YostarJP.BlueArchive\\files\\MediaPatch)";
                            break;
                    }
                    break;

                case Message.FolderDoesNotExist:
                    switch (language)
                    {
                        case Language.Japanese:
                            locString = "指定されたディレクトリが存在しませんでした";
                            break;

                        default:
                            locString = "The specified directory does not exist";
                            break;
                    }
                    break;

                case Message.SpecifyBinary:
                    switch (language)
                    {
                        case Language.Japanese:
                            locString = "{0}が指定されたフォルダの中に見つかりませんでした\n利用可能な{0}をフルパスで(ファイル名も含めて)入力してください\n(例:D:\\MediaCatalog\\MediaCatalog(1.44.278134).bytes)";
                            break;

                        default:
                            locString = "{0} was not found in the specified folder\nPlease specify the full path (include file name) to {0} manually available\n(e.g., D:\\MediaCatalog\\MediaCatalog(1.44.278134).bytes)";
                            break;
                    }
                    break;

                case Message.BinaryNotFound:
                    switch (language)
                    {
                        case Language.Japanese:
                            locString = "指定されたファイルが見つかりませんでした";
                            break;

                        default:
                            locString = "The specified file could not be found";
                            break;
                    }
                    break;

                case Message.MediaCatalogLoading:
                    switch (language)
                    {
                        case Language.Japanese:
                            locString = "MediaCatalogを読み込んでいます...";
                            break;

                        default:
                            locString = "Loading MediaCatalog...";
                            break;
                    }
                    break;

                case Message.DirCreated:
                    switch (language)
                    {
                        case Language.Japanese:
                            locString = "{0}フォルダをカレントディレクトリに作成しました";
                            break;

                        default:
                            locString = "{0} folder is created in the current directory";
                            break;
                    }
                    break;

                case Message.CopyStart:
                    switch (language)
                    {
                        case Language.Japanese:
                            locString = "ファイルのコピーを開始します\n({0}フォルダにコピーされます)";
                            break;

                        default:
                            locString = "output folder is created in the current directory\n(Files will be copied to the {0} folder)";
                            break;
                    }
                    break;

                case Message.Done:
                    switch (language)
                    {
                        case Language.Japanese:
                            locString = "完了しました\n{0}フォルダはこのプログラムが実行されたフォルダにあるはずです";
                            break;

                        default:
                            locString = "Done\nThe {0} folder should be in the folder where this program was run";
                            break;
                    }
                    break;
            }

            return locString;
        }
    }
}
