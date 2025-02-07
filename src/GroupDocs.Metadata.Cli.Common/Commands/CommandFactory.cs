using GroupDocs.Metadata.Cli.Common.Commands.Interfaces;
using GroupDocs.Metadata.Cli.Common.Enums;

namespace GroupDocs.Metadata.Cli.Common.Commands
{
    /// <summary>
    /// Command factory - get command by CommandType enum
    /// </summary>
    public static class CommandFactory
    {
        /// <summary>
        /// Create command
        /// </summary>
        /// <param name="commandType">Command Type</param>
        /// <returns>Requested command by ICommand interface</returns>
        public static ICommand Create(CommandType commandType)
        {
            if (commandType == CommandType.Export)
            {
                return new ExportCommand();
            }
            else if (commandType == CommandType.Find)
            {
                return new FindCommand();
            }
            else if (commandType == CommandType.Copy)
            {
                return new CopyCommand();
            }
            else if (commandType == CommandType.Remove)
            {
                return new RemoveCommand();
            }

            return null;
        }
    }
}
