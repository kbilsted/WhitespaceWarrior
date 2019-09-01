using NUnit.Framework;
using WhiteSpaceWarrior;

namespace Tests
{
    public class Tests
    {
        static string Compress(string content)
        {
            var compressed = new Compressors().Compress(content);
            return compressed.Trim();
        }

        [Test]
        public void Summary_0_space()
        {
            var code = @"
                /// <summary>
                /// </summary>
                public static string CompressProperties(string file) {";

            Assert.AreEqual("public static string CompressProperties(string file) {", Compress(code));
        }

        [Test]
        public void Summary_1_space()
        {
            var code = @"
                /// <summary>
                /// 
                /// </summary>
                public static string CompressProperties(string file) {";

            Assert.AreEqual("public static string CompressProperties(string file) {", Compress(code));
        }


        [Test]
        public void Summary_mult_space()
        {
            var code = @"
                /// <summary>
                /// 
                /// 
                /// </summary>
                public static string CompressProperties(string file) {";

            Assert.AreEqual("public static string CompressProperties(string file) {", Compress(code));
        }
        [Test]
        public void Summary_multiline_comment_are_ignored()
        {
            var code = @"
                /// <summary>
                /// foo boo 
                /// bar baz
                /// </summary>
                public static string CompressProperties(string file) {";

            Assert.AreEqual(@"/// <summary>
                /// foo boo 
                /// bar baz
                /// </summary>
                public static string CompressProperties(string file) {", Compress(code));
        }

        [Test]
        public void Summary_singleline_comment_is_inlined()
        {
            var code = @"
                /// <summary>
                /// foo boo
                /// </summary>
                public static string CompressProperties(string file) {";

            Assert.AreEqual(@"/// <summary> foo boo </summary>
                public static string CompressProperties(string file) {", Compress(code));
        }

    }
}