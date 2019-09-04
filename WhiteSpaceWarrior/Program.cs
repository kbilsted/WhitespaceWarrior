using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace WhiteSpaceWarrior
{
    [Command(
        FullName = @"NAME
    WhiteSpaceWarrior ",
        Description = @"DESCRIPTION
    This command strips unnecesarry cruft from c# files.

LICENSE
    Freeware - (c) Kasper B. Graversen 2019

VERSION
    v0.02")]
    class Program
    {
        public static int Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);

        [Required]
        [Argument(0, Description = "The path from which to recursevely compress cs files")]
        public string Path { get; }

        [RegularExpression("0|1")]
        [Option(CommandOptionType.SingleValue, ShortName = "v", Description ="Either use 0 or 1")]
        public int Verbosity { get; }

        [Option(CommandOptionType.NoValue, Description = "Skip removing #region")]
        public bool SkipRegions { get; }

        bool showFilesWhenProcessing = false;
        bool printOnlyChangedFiles = false;

        private void OnExecute()
        {
            try
            {
                if (Verbosity == 1)
                    showFilesWhenProcessing = true;
                if (Verbosity == 0)
                    printOnlyChangedFiles = true;

                int totalLinesReduced = 0;

                foreach (var path in Directory.EnumerateFiles(Path, "*.cs", SearchOption.AllDirectories))
                {
                    var linesReduced = PrintAndCompress(showFilesWhenProcessing, printOnlyChangedFiles, path);

                    totalLinesReduced += linesReduced;
                }

                Console.WriteLine();
                Console.WriteLine($"Total lines reduced: {totalLinesReduced}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType());
                Console.WriteLine(e.Message);
            }
        }

        private int PrintAndCompress(bool showFilesWhenProcessing, bool printOnlyChangedFiles, string path)
        {
            if (showFilesWhenProcessing)
                Console.Write($"{">>>",5} {path}      ");

            var linesReduced = CompressFile(path);

            Console.SetCursorPosition(0, Console.CursorTop);

            if (showFilesWhenProcessing || (linesReduced != 0 && printOnlyChangedFiles))
            {
                Console.WriteLine($"{linesReduced,5} {path}");
            }

            return linesReduced;
        }

        private int CompressFile(string path)
        {
            var isUtf8Bom = IsUtf8Bom(path);

            var enc = isUtf8Bom ? Encoding.UTF8 : Encoding.GetEncoding("iso-8859-1");
            var file = File.ReadAllText(path, enc);
            var lines = file.Count(x => x == '\n');

            var newFile = new Compressors(new CompressOptions(SkipRegions)).Compress(file);

            if (newFile.Length != file.Length)
            {
                File.WriteAllText(path, newFile, enc);
                return lines - newFile.Count(x => x == '\n');
            }

            return 0;
        }

        private static bool IsUtf8Bom(string path)
        {
            var b = File.ReadAllBytes(path);
            if (b.Length < 3)
                return false;

            bool isUtf8Bom = true;
            for (int i = 0; i < Encoding.UTF8.GetPreamble().Length; i++)
            {
                if (Encoding.UTF8.GetPreamble()[i] != b[i])
                {
                    isUtf8Bom = false;
                }
            }

            return isUtf8Bom;
        }

    }
}
