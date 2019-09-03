namespace WhiteSpaceWarrior
{
    public class CompressOptions
    {
        public bool SkipRegions { get; }

        public CompressOptions(bool skipRegions)
        {
            this.SkipRegions = skipRegions;
        }
    }
}