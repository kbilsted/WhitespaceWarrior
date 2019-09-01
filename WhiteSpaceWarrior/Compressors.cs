using System;
using System.Text.RegularExpressions;

namespace WhiteSpaceWarrior
{
    public class Compressors
    {
        private static RegexOptions options = RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant;
        private static RegexOptions optionsIgnoreCase = options | RegexOptions.IgnoreCase;

        public string Compress(string content)
        {
            content = CompressProperties(content);
            content = CompressRegionVersionHistory(content);
            content = CompressParam(content);
            content = EmptyReturnsRE.Replace(content, "");
            content = CompressSummmary(content);
            return content;
        }

        static readonly Regex MultilineProperties = new Regex(@"\s*\{\s+get;\s+(?<modifier>protected |private )?set;\s+\}", options);

        static string CompressProperties(string content)
        {
            var newFile = MultilineProperties.Replace(content, @" { get; ${modifier}set; }");
            return newFile;
        }

        static readonly Regex VersionHistoryRE = new Regex(@"( |\t)*#region Version History( |\t)*((?>\s*//[^\n]*))*\s*#endregion( |\t)*(\r\n?|\n)+", optionsIgnoreCase);

        static string CompressRegionVersionHistory(string content)
        {
            var newFile = VersionHistoryRE.Replace(content, "");
            return newFile;
        }

        static readonly Regex singleLineSummaryRE = new Regex(@"(?<indent>[ \t]+)/// <summary>[ \t]*\r?\n[ \t]+///[ \t]*(?<comment>[^\r\n]*)\r?\n[ \t]+/// </summary>[^\n]*\n", options);
        static readonly Regex singleLineEmptySummaryRE = new Regex(@"[ \t]+/// <summary>\s*</summary>[^\n]*\n", options);
        static readonly Regex multilineEmptySummaryRE = new Regex(@"[ \t]+/// <summary>[ \t]*\r?\n([ \t]+///[ \t]*\r?\n)*[ \t]+/// </summary>[^\n]*\n", options);

        string CompressSummmary(string content)
        {
            content = singleLineSummaryRE.Replace(content, "${indent}/// <summary> ${comment} </summary>" + Environment.NewLine);
            content = singleLineEmptySummaryRE.Replace(content, "");
            content = multilineEmptySummaryRE.Replace(content, "");

            return content;
        }

        string CompressParam(string content)
        {
            content = emptyParamRE.Replace(content, "");
            content = emptyTypeparamRE.Replace(content, "");

            return content;
        }
        static readonly Regex emptyParamRE = new Regex(@"[ \t]+/// <param name=""[^""]*"">\s*</param>[^\n]*\n", options);
        static readonly Regex emptyTypeparamRE = new Regex(@"[ \t]+/// <typeparam name=""[^""]*"">\s*</typeparam>[^\n]*\n", options);
        static readonly Regex EmptyReturnsRE = new Regex(@"[ \t]+/// <returns>\s*</returns>[^\n]*\n", options);
    }
}
