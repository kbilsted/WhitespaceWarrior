using System;
using System.Text.RegularExpressions;

namespace WhiteSpaceWarrior
{
    public class Compressors
    {
        private static RegexOptions options = RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant;
        private static RegexOptions optionsIgnoreCase = options | RegexOptions.IgnoreCase;

        static readonly Regex GetSetRegEx = new Regex(@"\s*\{\s+get;\s+(?<modifier>protected |private )?set;\s+\}", options);


        public string Compress(string content)
        {
            return CompressSummmary(content);
        }

        public static string CompressProperties(string file)
        {
            var newFile = GetSetRegEx.Replace(file, @" { get; ${modifier}set; }");
            return newFile;
        }

        static readonly Regex VersionHistoryRegEx = new Regex(@"( |\t)*#region Version History( |\t)*((?>\s*//[^\n]*))*\s*#endregion( |\t)*(\r\n?|\n)+", optionsIgnoreCase);

        public static string CompressRegionVersionHistory(string file)
        {
            var newFile = VersionHistoryRegEx.Replace(file, "");
            return newFile;
        }

        static Regex singleLineSummary = new Regex(@"(?<indent>[ \t]+)/// <summary>[ \t]*\r?\n[ \t]+///[ \t]*(?<comment>[^\r\n]*)\r?\n[ \t]+/// </summary>[^\n]*\n", options);
        static Regex singleLineEmptySummary = new Regex(@"[ \t]+/// <summary>\s*</summary>[^\n]*\n", options);
        static Regex multilineEmptySummary = new Regex(@"[ \t]+/// <summary>[ \t]*\r?\n([ \t]+///[ \t]*\r?\n)*[ \t]+/// </summary>[^\n]*\n", options);
        string CompressSummmary(string content)
        {
            var content1 = singleLineSummary.Replace(content, "${indent}/// <summary> ${comment} </summary>" + Environment.NewLine);
            var content2 = singleLineEmptySummary.Replace(content1, "");
            var content3 = multilineEmptySummary.Replace(content2, "");

            var content4 = cleanup.Replace(content3, "${elmname}" + Environment.NewLine);

            return content4;
        }


        static Regex emptyParamRE = new Regex(@"\s+/// <param name=""[^""]*"">\s*</param>[^\n]*\n", options);
        static Regex emptyTypeparamRE = new Regex(@"\s+/// <typeparam name=""[^""]*"">\s*</typeparam>[^\n]*\n", options);
        static Regex emptyReturnsRE = new Regex(@"\s+/// <returns>\s*</returns>[^\n]*\n", options);
        static Regex cleanup = new Regex(@"(?<elmname>(</summary>|</param>))\r\n(\r\n)+", options);

        //$content = $emptyParamRE.Replace($content, "`r`n")

        //$content = $emptyTypeparamRE.Replace($content, "`r`n")

        //$content = $emptyReturnsRE.Replace($content, "`r`n")



        //# cleanup must be last, ensuring eg. that the space left from remo

        //$content = $cleanup.Replace($content, '${elmname}'+"`r`n");
    }
}
