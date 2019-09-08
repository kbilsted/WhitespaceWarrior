namespace WhiteSpaceWarrior
{
    public class CompressOptions
    {
        public string[] RemoveCustomTags { get; }
        public bool RemoveRegions { get; }

        public CompressOptions(bool removeRegions, string[] removeCustomTags)
        {
            RemoveCustomTags = removeCustomTags;
            RemoveRegions = removeRegions;
        }
    }
}