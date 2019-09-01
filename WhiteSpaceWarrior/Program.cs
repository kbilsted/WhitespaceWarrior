﻿using McMaster.Extensions.CommandLineUtils;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace WhiteSpaceWarrior
{
    class Program
    {
        public static int Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);
        
        [Option(Description = "start path")]
        public string Path { get; }

        [Option(ShortName = "v")]
        public int Verbosity { get; }


        bool showFilesWhenProcessing = false;
        bool printOnlyChangedFiles = false;

        private void OnExecute()
        {
            if (Verbosity == 0) 
                showFilesWhenProcessing = true;
            if (Verbosity == 1) 
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

        private static int PrintAndCompress(bool showFilesWhenProcessing, bool printOnlyChangedFiles, string path)
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

        private static int CompressFile(string path)
        {
            var isUtf8Bom = IsUtf8Bom(path);

            var enc = isUtf8Bom ? Encoding.UTF8 : Encoding.GetEncoding("iso-8859-1");
            var file = File.ReadAllText(path, enc);
            var lines = file.Count(x => x == '\n');

            var newFile = CallCompressors(file);

            if (newFile.Length != file.Length)
            {
                File.WriteAllText(path, newFile, enc);
                return lines - newFile.Count(x => x == '\n');
            }

            return 0;
        }

        private static string CallCompressors(string file)
        {
            file = Compressors.CompressProperties(file);
            file = Compressors.CompressRegionVersionHistory(file);
            file = new Compressors().Compress(file);

            return file;
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
