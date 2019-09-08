using NUnit.Framework;
using WhiteSpaceWarrior;

namespace Tests
{
    public class Tests
    {
        static string Compress(string content)
        {
            var compressed = new Compressors(new Options()
            {
                RemoveRegions = true,
                RemoveTags = new string[] {"revision"},
            }).Compress(content);
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

        [Test]
        public void PropertyCompress_get_set()
        {
            var code = @"
string MyProperty
{
    set;
    get;
}";

            Assert.AreEqual("string MyProperty { get; set; }", Compress(code));
        }

        [Test]
        public void PropertyCompress_set_get()
        {
            var code = @"
string MyProperty
{
    get;
    set;
}";
            Assert.AreEqual("string MyProperty { get; set; }", Compress(code));
        }


        [Test]
        public void OldStyleSeparator()
        {
            var code = @"        }

        //////////////////////////////////////////////////

        public void foo()";
            
            Assert.AreEqual(@"}

        public void foo()", Compress(code));

        }


        [Test]
        public void OldStyleSeparator_with_region()
        {
            var code = @"
        #region GetName
        /////////////////////////////////////////////////////////////////////////////////

        static foo";

            Assert.AreEqual(@"static foo", Compress(code));

        }


        [Test]
        public void OldStyleSeparator_with_if()
        {
            var code = @"}

        #if DEBUG
        /////////////////////////////////////////////////////////////////////////////////

        static foo";

            Assert.AreEqual(@"}

        #if DEBUG

        static foo", Compress(code));

        }

        [Test]
        public void OldStyleSeparator_with_endregion()
        {
            var code = @"}

        /////////////////////////////////////////////////////////////////////////////////
#endregion

        static foo";

            Assert.AreEqual(@"}


        static foo", Compress(code));

        }

        [Test]
        public void OldStyleSeparator_spaced_lines_are_not_matched()
        {
            var code = @"        }
                     
        //////////////////////////////////////////////////
                                
        public void foo()";

            Assert.AreEqual(code.Trim(), Compress(code));
        }


        [Test]
        public void Regions_are_removed_when_endregion_is_last_part_of_the_file()
        {
            var code = @"
#region private variables
    int i;
#endregion";

            Assert.AreEqual("int i;", Compress(code)); ;
        }

        [Test]
        public void Regions_are_removed_when_endregion_is_somewhere_in_the_file()
        {
            var code = @"
#region private variables
    int i;
#endregion
    int j;";

            Assert.AreEqual(@"int i;
    int j;", Compress(code)); ;
        }

        [Test]
        public void Remove_custom_tags_with_eager_matching()
        {
            var code = @"
            /// <revision version=""6.11.20"" date=""2015-09-19"" >
            /// implements nu interface 
            /// runs in singleinstance mode with dynamic recesions
            /// </revision>
            int i;
            /// fake do not match </revision>";

            Assert.AreEqual(@"int i;
            /// fake do not match </revision>", Compress(code)); ;
        }
    }
}