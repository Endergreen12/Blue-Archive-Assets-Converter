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
            // Preparation
            PressAnyKeyToExit,
            EnterFolderPath,
            FolderDoesNotExist,
            SpecifyBinary,
            BinaryNotFound,
            SpecifyMediaType,
            FailedToParseMediaType,

            // Loading, Copying
            MediaCatalogLoading,
            DirCreated,
            CopyStart,
            SourceFileNotFound,
            FileExist,
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

        public static void breakLine(int count = 1)
        {
            if (count == 1)
            {
                Console.WriteLine();
            } else {
                // Thanks https://dobon.net/vb/dotnet/string/repeat.html
                Console.Write(new string('*', count).Replace("*", Environment.NewLine));
            }
        }

        public static Language getUserLanguage()
        {
            string country = System.Globalization.CultureInfo.CurrentCulture.Name;
            Language language = Language.English;
            switch (country)
            {
                case "ja-JP":
                    language = Language.Japanese;
                    break;
            }

            return language;
        }

        public static string getLocalizedString(Message message, Language language)
        {
            string locString = "";
            string newLineStr = Environment.NewLine;
            switch(message)
            {
                // Preparation
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
                            locString = "MediaPatchフォルダーのパスを入力してください" + newLineStr + "(例:D:\\BlueArchive\\com.YostarJP.BlueArchive\\files\\MediaPatch)";
                            break;

                        default:
                            locString = "Enter the path to the MediaPatch folder" + newLineStr + "(e.g., D:\\BlueArchive\\com.YostarJP.BlueArchive\\files\\MediaPatch)";
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
                            locString = "{0}が指定されたフォルダの中に見つかりませんでした" + newLineStr + "利用可能な{0}をフルパスで(ファイル名も含めて)入力してください" + newLineStr + "(例:D:\\MediaCatalog\\MediaCatalog(1.44.278134).bytes)";
                            break;

                        default:
                            locString = "{0} was not found in the specified folder" + newLineStr + "Please specify the full path (include file name) to {0} manually available" + newLineStr + "(e.g., D:\\MediaCatalog\\MediaCatalog(1.44.278134).bytes)";
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

                case Message.SpecifyMediaType:
                    switch (language)
                    {
                        case Language.Japanese:
                            locString = "もしコピーするMediaTypeを指定したい場合は入力してください。ない場合は空欄にしてください" + newLineStr + "(例: Audio)" + newLineStr + "利用可能なMediaType一覧:";
                            break;

                        default:
                            locString = "If you want to specify the MediaType to be copied, please enter it. If not, please leave it blank" + newLineStr + "(e.g., Audio)" + newLineStr + "List of available MediaTypes:";
                            break;
                    }
                    break;

                case Message.FailedToParseMediaType:
                    switch (language)
                    {
                        case Language.Japanese:
                            locString = "MediaTypeのParseに失敗しました。上の一覧に含まれているものを入力してください";
                            break;

                        default:
                            locString = "Parse of MediaType failed. Please enter the one included in the list above";
                            break;
                    }
                    break;

                // Loading
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
                            locString = "ファイルのコピーを開始します" + newLineStr + "({0}フォルダにコピーされます)";
                            break;

                        default:
                            locString = "output folder is created in the current directory" + newLineStr + "(Files will be copied to the {0} folder)";
                            break;
                    }
                    break;

                case Message.SourceFileNotFound:
                    switch (language)
                    {
                        case Language.Japanese:
                            locString = "{0}のソースファイル{1}が見つかりませんでした";
                            break;

                        default:
                            locString = "Source file {1} for {0} could not be found";
                            break;
                    }
                    break;

                case Message.FileExist:
                    switch (language)
                    {
                        case Language.Japanese:
                            locString = "{0}はすでにコピーされていたため、スキップします";
                            break;

                        default:
                            locString = "Skip {0} because it was already copied";
                            break;
                    }
                    break;

                case Message.Done:
                    switch (language)
                    {
                        case Language.Japanese:
                            locString = "完了しました" + newLineStr + "{0}フォルダの場所は \"{1}\" です" + newLineStr + "完了までにかかった時間: {2}分 {3}秒 ({4}ミリ秒)";
                            break;

                        default:
                            locString = "Done" + newLineStr + "The location of the {0} folder is \"{1}\"" + newLineStr + "Time taken to complete: {2} minutes {3} seconds ({4} milliseconds)";
                            break;
                    }
                    break;
            }

            return locString;
        }
    }
}
