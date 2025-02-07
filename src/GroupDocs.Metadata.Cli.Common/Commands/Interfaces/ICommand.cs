using GroupDocs.Metadata.Cli.Common.Enums;
using GroupDocs.Metadata.Cli.Common.Parameters;
using GroupDocs.Metadata.Cli.Common.Parameters.ParseResults;

namespace GroupDocs.Metadata.Cli.Common.Commands.Interfaces
{
    /// <summary>
    /// Metadata command interface.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Command type.
        /// </summary>
        CommandType CommandType { get; }

        /// <summary>
        /// Execute command
        /// </summary>
        /// <param name="args">Arguments.</param>
        void Execute(string[] args = null);

        /// <summary>
        /// Parse command-line arguments and return parse result.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <returns>Parse result.</returns>
        CommandLineParseResult Parse(string[] args);

        /// <summary>
        /// Display help for command.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        void DisplayHelp(string[] args);
    }
}
