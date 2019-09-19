using System;
using System.IO;
using System.Linq;
using System.Text;
using McMaster.Extensions.CommandLineUtils;

namespace WhiteSpaceWarrior
{
    class Statistics
    {
        public int TotalLinesBefore;
        public int TotalLinesAfter;
    }

    class Program
    {
        public static int Main(string[] args)
            => CommandLineApplication.Execute<Options>(args);

        private readonly Options options;

        public Program(Options options)
        {
            this.options = options;
        }

        public void Execute()
        {
            try
            {
                if (!options.NoLogo)
                    ShowLogo();

                var stats = new Statistics();

                foreach (var path in Directory.EnumerateFiles(options.Path, "*.cs", SearchOption.AllDirectories))
                {
                    PrintAndCompress(path, stats);
                }

                Console.WriteLine();
                var percent = stats.TotalLinesBefore == 0 
                    ? 0 
                    : 100M - (stats.TotalLinesAfter / (decimal)stats.TotalLinesBefore * 100M);
                Console.WriteLine($"Lines Before: {stats.TotalLinesBefore}, after: {stats.TotalLinesAfter}. Reduction: {stats.TotalLinesBefore-stats.TotalLinesAfter} = {percent:.00}%");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType());
                Console.WriteLine(e.Message);
            }
        }

        private void PrintAndCompress(string path, Statistics stats)
        {
            if (options.Verbosity == VerbosityLevel.ShowAllFiles)
                Console.Write($"{">>>",5} {path}      ");

            var linesReduced = CompressFile(path, stats);

            Console.SetCursorPosition(0, Console.CursorTop);

            if (options.Verbosity == VerbosityLevel.ShowAllFiles
                || (linesReduced != 0 && options.Verbosity == VerbosityLevel.ShowOnlyChangedFiles))
            {
                Console.WriteLine($"{linesReduced,5} {path}");
            }
        }

        private int CompressFile(string path, Statistics stats)
        {
            var isUtf8Bom = IsUtf8Bom(path);

            var enc = isUtf8Bom ? Encoding.UTF8 : Encoding.GetEncoding("iso-8859-1");
            var file = File.ReadAllText(path, enc);
            int lines = file.Count(x => x == '\n');

            var newFile = new CSharpCompressors(options).Compress(file);

            if (newFile.Length != file.Length)
            {
                File.WriteAllText(path, newFile, enc);

                var linesAfter = newFile.Count(x => x == '\n');
                stats.TotalLinesBefore += lines;
                stats.TotalLinesAfter += linesAfter;
                return lines - linesAfter;
            }
            else
            {
                stats.TotalLinesBefore += lines;
                stats.TotalLinesAfter += lines;
                return 0;
            }
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


        void ShowLogo()
        {
            var logo = @"
                             &@@@@@@                                 
                           @@@@@@@@@@@@@@                            
 @@@                      @@@          @@@@&                         
  @@@@                    @@              @@@@                       
    @@@@              @@@@@@@@@@            @@@@                     
     @@@             @@@&     @@@             @@@                    
     @@@            @@@        @@&             @@@                   
     @@@            @@@ @@&&@@ @@@             @@@                   
     @@@           @@@@@@@@@@@@@@@&             @@@                  
     @@@         @@@@@          @@@@@           @@@                  
     @@@       @@@@                @@@@          @@@                 
     @@@      @@@                    @@@         @@@                 
     @@@     @@@  @@@ @@@             @@@        @@@                 
     @@@    @@@   @@@ @@@              @@@        @@@                
     @@@    &@@                        @@@        @@@                
     @@@    &@@ @@@      @@@           @@@        @@@                
     @@@    &@@  @@@@@@@@@@            @@@         @@@               
     @@@    &@@                        @@@         @@@               
     @@@    &@@@@@@@@@@@@@@@@@@@@@@@@@@@@@          @@@              
     @@@    &@@                        @@@          @@@              
     @@@    @@@                        @@@          @@@              
     @@@    &@@                        @@@           @@@             
     @@@    @@@                        @@@           @@@             
     @@@   @@@@@@@@@@                  @@@           @@@             
     @@@@@@@@     @@@@@                @@@            @@@            
     @@@@@   @@@@@@  @@@@              @@@     @@@@@@@@@@@@@@@@@     
     @@@@  @@@@@@@@@@  @@@             @@@    @@@             @@@    
     @@@  @@@     @@@@ @@@             @@@  @@@@@@@@@@@@@@@@@@@@@@@  
     @@@  @@@      @@@ @@@            @@@  @@@                   @@@ 
      @@@ @@@@   @@@@  @@@@@@@@@@@@@@@@@   @@@@@@@@@@@@@@@@@@@@@@@@@ 
       @@&  @@@@@@@   @@@                     @@@@@@@@@@@@@@@@@@@    
        @@@@@      @@@@                                              
          @@@@@@@@@@@                                                
";

            Console.WriteLine(logo);
        }
    }
}