using System.Diagnostics;
using System.Reflection;

namespace GroupDocs.Metadata.Cli
{
    public class Product
    {
        public static readonly string GroupDocsMetadataVersion = GetGroupDocsMetadataProductVersion();

        private static string GetGroupDocsMetadataProductVersion()
        {
            Assembly metadataAssembly = Assembly.GetAssembly(typeof(GroupDocs.Metadata.License));
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(metadataAssembly?.Location ?? string.Empty);

            return fileVersionInfo.FileVersion;
        }

        public static readonly string CLIVersion = GetCliProductVersion();

        private static string GetCliProductVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        }
    }
}