using NUnit.Framework;
using System.IO;
using GroupDocs.Metadata.Cli.Tests.Utils;

namespace GroupDocs.Metadata.Cli.Tests
{
    /// <summary>
    /// Command-line tests.
    /// </summary>
    public class CommandLineTests : BaseConsoleTests
    {
        [Test]
        public void HelpIsDisplayed()
        {
            string result = CallCliApplication(new string[] { });

            Assert.IsTrue(result.Contains("Usage: groupdocs-metadata [command] [source-file]"));
        }

        [Test]
        public void ExportDocxToXmlTest()
        {
            var output = "out.xml";
            CallCliApplication(new string[] { "export", "Resources/test.docx", "-o", output });

            Assert.IsTrue(File.Exists(output));
        }

        [Test]
        public void ExportDocxToCsvTest()
        {
            var output = "out.csv";
            CallCliApplication(new string[] { "export", "Resources/test.docx", "-o", output });

            Assert.IsTrue(File.Exists(output));
        }

        [Test]
        public void ExportDocxToJsonTest()
        {
            //var output = "out.json";
            CallCliApplication(new string[] { "export", "Resources/test.docx", "--output-format", "JSON" });

            //Assert.IsTrue(File.Exists(output));
        }

        [Test]
        public void FindAllTest()
        {
            string result = CallCliApplication(new string[] { "find", "Resources/test.docx", "-a"});

            Assert.IsTrue(!string.IsNullOrEmpty(result));
            Assert.IsTrue(result.Contains("https://products.groupdocs.com/metadata/"));
        }

        [Test]
        public void FindByNameTest()
        {
            string result = CallCliApplication(new string[] { "find", "Resources/test.docx", "-p", "CreateTime" });

            Assert.IsTrue(!string.IsNullOrEmpty(result));
        }

        [Test]
        public void FindByNameFailTest()
        {
            string result = CallCliApplication(new string[] { "find", "Resources/test.docx", "-p", "CreateTime111" });

            Assert.IsTrue(result.Contains("Not find property"));
        }

        [Test]
        public void CopyToLicenseFailTest()
        {
            string result = CallCliApplication(new string[] { "copy", "Resources/test.docx", "-o", "Resources/test1.docx" });

            Assert.IsTrue(result.Contains("Evaluation only."));
        }

        [Test]
        public void CopyToTest()
        {
            if (File.Exists("Resources/test1.docx"))
            {
                File.Delete("Resources/test1.docx");
            }
            File.Copy("Resources/sample.docx", "Resources/test1.docx");

            string result = CallCliApplication(new string[] { "copy", "Resources/test.docx", "-o", "Resources/test1.docx", "--license-path", @"C:\Work\License\GroupDocs.Total.Product.Family.lic" });

            Assert.IsTrue(result.Contains("dc:creator"));
        }

        [Test]
        public void CopyToFailTest()
        {
            string result = CallCliApplication(new string[] { "copy", "Resources/test.docx", "-o", "Resources/sample.msg" });

            Assert.IsTrue(result.Contains("source and destination files must be of the same type"));
        }

        [Test]
        public void RemoveTest()
        {
            if (File.Exists("Resources/sample.pdf"))
            {
                File.Delete("Resources/remove_test.pdf");
            }
            File.Copy("Resources/sample.pdf", "Resources/remove_test.pdf");

            string result = CallCliApplication(new string[] { "find", "Resources/remove_test.pdf", "-a", "--license-path", @"C:\Work\License\GroupDocs.Total.Product.Family.lic" });

            Assert.IsTrue(result.Contains("Property Name: author"));
            Assert.IsTrue(result.Contains("Property Value: Windows User"));


            result = CallCliApplication(new string[] { "remove", "Resources/remove_test.pdf", "-a", "--license-path", @"C:\Work\License\GroupDocs.Total.Product.Family.lic" });

            Assert.IsTrue(result.Contains("Remove 7 properties."));
        }
    }
}