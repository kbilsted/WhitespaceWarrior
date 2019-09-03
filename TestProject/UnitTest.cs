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

            Assert.AreEqual(@"#region GetName

        static foo", Compress(code));

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

#endregion

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

    }
}