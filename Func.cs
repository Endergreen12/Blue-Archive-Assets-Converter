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
            EnterPath,
            NotFound,
            SpecifyMediaType,
            FailedToParseMediaType,

            CatalogLoading,
            SourceFileNotFound,
            FileExist,
            Done
        }

        public enum Language
        {
            English,
            Japanese
        }

        public enum CatalogType
        {
            MediaCatalog,
            TableCatalog
        }

        public static void pressAnyKeyToExit(Language lang)
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

        public static string get

        public static string getLocalizedString(Message message, Language language)
        {
            string locString = "";
            string newLineStr = Environment.NewLine;
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

                case Message.EnterPath:
                    switch(language)
                    {
                        case Language.Japanese:
                            locString = "\"{0}\" のパスを入力してください";
                            break;

                        default:
                            locString = "Enter the path to \"{0}\"";
                            break;
                    }
                    break;

                case Message.NotFound:
                    switch (language)
                    {
                        case Language.Japanese:
                            locString = "\"{0}\" が存在しませんでした";
                            break;

                        default:
                            locString = "The specified directory \"{0}\" does not exist";
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

                case Message.CatalogLoading:
                    switch (language)
                    {
                        case Language.Japanese:
                            locString = "カタログを読み込んでいます...";
                            break;

                        default:
                            locString = "Loading Catalog...";
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
                            locString = "{0}はすでに存在していたため、スキップします";
                            break;

                        default:
                            locString = "Skip {0} because it already existed";
                            break;
                    }
                    break;

                case Message.Done:
                    switch (language)
                    {
                        case Language.Japanese:
                            locString = "完了しました" + newLineStr + "完了までにかかった時間: {2}分 {3}秒 ({4}ミリ秒)";
                            break;

                        default:
                            locString = "Done" + newLineStr + "Time taken to complete: {2} minutes {3} seconds ({4} milliseconds)";
                            break;
                    }
                    break;
            }

            return locString;
        }
    }
}
