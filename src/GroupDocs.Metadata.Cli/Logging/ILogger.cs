namespace GroupDocs.Metadata.Cli.Logging
{
    internal interface ILogger
    {
        void WriteLine(string message);

        void WriteLine();

        void Write(string message);
    }
}