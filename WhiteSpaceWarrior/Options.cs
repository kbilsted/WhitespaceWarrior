using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;

namespace WhiteSpaceWarrior
{
    [Command(
        FullName = @"NAME
    WhiteSpaceWarrior ",
        Description = @"DESCRIPTION
    This command strips unnecessary cruft from c# files.

LICENSE
    Freeware - (c) Kasper B. Graversen 2019"

    )]
    [VersionOption("1.0.0")]
    public class Options
    {
        [Required]
        [Argument(0, Description = "The path from which to recursively compress cs files")]
        public string Path { get; set; }

        [Option(ShortName = "v", Description = "Set verbosity level")]
        public VerbosityLevel Verbosity { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "rpn", Description = "Remove <param name=\"..\"> descriptions up to N words long. A low number such as \"2\" captures many useless comments.")]
        public int RemoveParamNameUptoNWords { get; set; } = 0;

        [Option(CommandOptionType.SingleValue, ShortName = "rsn", Description = "Remove <summary> descriptions up to N words long. A low number such as \"2\" captures many useless comments.")]
        public int RemoveSummaryUptoNWords { get; set; } = 0;

        [Option(CommandOptionType.NoValue, ShortName = "rr", Description = "Remove #region")]
        public bool RemoveRegions { get; set; }

        [Option(CommandOptionType.MultipleValue, ShortName = "rt", Description = "Remove <tag> in /// sections. Can be specified multiple times")]
        public string[] RemoveTags { get; set; } = new string[0];

        private void OnExecute()
        {
            new Program(this).Execute();
        }
    }
}
