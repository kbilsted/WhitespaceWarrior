using System;
using System.Text.RegularExpressions;

namespace WhiteSpaceWarrior
{
    public class Compressors
    {
        public CompressOptions Options { get; }

        public Compressors(CompressOptions options)
        {
            Options = options;
        }

        static RegexOptions options = RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant;
        static RegexOptions optionsIgnoreCase = options | RegexOptions.IgnoreCase;

        public string Compress(string content)
        {
            content = CompressProperties(content);
            content = VersionHistoryRE.Replace(content, "");

            content = CompressParam(content);
            content = EmptyReturnsRE.Replace(content, "");
            content = OldStyleMethodSeparators(content);
            content = CompressSummmary(content);
            return content;
        }

        static readonly Regex OldStyleMethodSeparator = new Regex(@"(\r?\n){2,}[ \t]*/////+[ \t]*\r?\n(\r?\n)+", options);
        static readonly Regex OldStyleMethodSeparatorPreprocessorDirectives = new Regex(@"(?<=(#if|#region) \w*[ \t]*(\r?\n))[ \t]*/////+[ \t]*\r?\n(\r?\n)+", options);
        static readonly Regex OldStyleMethodSeparatorEndPreprocessorDirectives = new Regex(@"(\r?\n){2,}[ \t]*/////+[ \t]*\r?\n[ \t]*(?=(#endif|#endregion))", options);

        private static string OldStyleMethodSeparators(string content)
        {
            content = OldStyleMethodSeparator.Replace(content, Environment.NewLine + Environment.NewLine);
            content = OldStyleMethodSeparatorPreprocessorDirectives.Replace(content, Environment.NewLine);
            content = OldStyleMethodSeparatorEndPreprocessorDirectives.Replace(content, Environment.NewLine+Environment.NewLine);
            return content;
        }

        static readonly Regex MultilineProperties_get_set = new Regex(@"\s*\{\s+get;\s+(?<modifier>protected |private )?set;\s+\}", options);
        static readonly Regex MultilineProperties_set_get = new Regex(@"\s*\{\s+(?<modifier>protected |private )?set;\s+get;\s+\}", options);

        static string CompressProperties(string content)
        {
            content = MultilineProperties_get_set.Replace(content, @" { get; ${modifier}set; }");
            content = MultilineProperties_set_get.Replace(content, @" { get; ${modifier}set; }");
            return content;
        }

        static readonly Regex VersionHistoryRE = new Regex(@"( |\t)*#region Version History( |\t)*((?>\s*//[^\n]*))*\s*#endregion( |\t)*(\r\n?|\n)+", optionsIgnoreCase);

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
        static readonly Regex emptyParamRE = new Regex(@"[ \t]+/// <param name\s*=\s*""[^""]*"">\s*</param>[^\n]*\n", options);
        static readonly Regex emptyTypeparamRE = new Regex(@"[ \t]+/// <typeparam name\s*=\s*""[^""]*"">\s*</typeparam>[^\n]*\n", options);
        static readonly Regex EmptyReturnsRE = new Regex(@"[ \t]+/// <returns>\s*</returns>[^\n]*\n", options);
    }
}
