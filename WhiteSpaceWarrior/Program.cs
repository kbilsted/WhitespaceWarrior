using System;
using System.IO;
using System.Linq;
using System.Text;
using McMaster.Extensions.CommandLineUtils;

namespace WhiteSpaceWarrior
{
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

                int totalLinesReduced = 0;

                foreach (var path in Directory.EnumerateFiles(options.Path, "*.cs", SearchOption.AllDirectories))
                {
                    var linesReduced = PrintAndCompress(path);

                    totalLinesReduced += linesReduced;
                }

                Console.WriteLine();
                Console.WriteLine($"Lines reduced: {totalLinesReduced}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType());
                Console.WriteLine(e.Message);
            }
        }

        private int PrintAndCompress(string path)
        {
            if (options.Verbosity==VerbosityLevel.ShowAllFiles)
                Console.Write($"{">>>",5} {path}      ");

            var linesReduced = CompressFile(path);

            Console.SetCursorPosition(0, Console.CursorTop);

            if (options.Verbosity == VerbosityLevel.ShowAllFiles 
                || (linesReduced != 0 && options.Verbosity==VerbosityLevel.ShowOnlyChangedFiles))
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

            var newFile = new CSharpCompressors(options).Compress(file);

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