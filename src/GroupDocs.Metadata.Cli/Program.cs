using GroupDocs.Metadata.Cli.Common.Commands;
using GroupDocs.Metadata.Cli.Common.Commands.Interfaces;
using GroupDocs.Metadata.Cli.Common.Exceptions;
using GroupDocs.Metadata.Cli.Common.Parameters;
using GroupDocs.Metadata.Cli.Common.Parameters.Implementations;
using GroupDocs.Metadata.Cli.Utils;
using System;
using GroupDocs.Metadata.Cli.Common.Parameters.ParseResults;

namespace GroupDocs.Metadata.Cli
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                return ProcessArgs(args);
            }
            catch (CliApplicationException e)
            {
                Reporter.Error.WriteLine(e.Message);
                return 0;
            }
            catch (Exception e)
            {
                Reporter.Error.WriteLine("Unexpected exception:" + e.Message);
                return 0;
            }
        }

        private static int ProcessArgs(string[] args)
        {
            var lastArg = 0;
            CommandLineParseResult result = null;

            if (args.Length == 0)
            {
                DisplayHelp(args);
            }

            for (; lastArg < args.Length; lastArg++)
            {
                if (IsArg(args[lastArg], "version"))
                {
                    PrintVersion();
                    return 0;
                }
                else if (IsArg(args[lastArg], "h", "help") ||
                         args[lastArg] == "-?" ||
                         args[lastArg] == "/?")
                {
                    DisplayHelp(args);

                    return 0;
                }
            }

            if (args.Length == 0)
                return 0;

            var command = args[0];

            VerboseParameter verboseParameter = new VerboseParameter();

            if (!verboseParameter.ParseAndValidate(args))
            {
                Reporter.Error.WriteLine("Invalid value for parameter verbose(v): " + verboseParameter.LastValidationResult.ValidationMessage);
                return 1;
            }

            CommandContext.SetVerbose(verboseParameter.LastValidationResult.ResultValue);

            if (string.IsNullOrEmpty(command))
            {
                DisplayHelp(args);
                return 0;
            }
            ICommand commandObj = null;

            if (command == MainCommands.ExportCommand)
            {
                commandObj = CommandFactory.Create(Common.Enums.CommandType.Export);
                result = commandObj.Parse(args);
            }
            else if (command == MainCommands.FindCommand)
            {
                commandObj = CommandFactory.Create(Common.Enums.CommandType.Find);
                result = commandObj.Parse(args);
            }
            else if (command == MainCommands.CopyCommand)
            {
                commandObj = CommandFactory.Create(Common.Enums.CommandType.Copy);
                result = commandObj.Parse(args);
            }
            else if (command == MainCommands.RemoveCommand)
            {
                commandObj = CommandFactory.Create(Common.Enums.CommandType.Remove);
                result = commandObj.Parse(args);
            }

            if (result != null && result.Success)
            {
                commandObj.Execute();
            }
            else if (result == null)
            {
                result = CommandLineParseResult.Failure(
                    $"Unknown command {command} should be one of: {MainCommands.ExportCommand}, {MainCommands.FindCommand}, {MainCommands.CopyCommand}, {MainCommands.RemoveCommand}");
            }

            if (!result.Success)
            {
                Reporter.Error.WriteLine(result.Message);
            }

            return 0;
        }

        private static void DisplayHelp(string[] args)
        {
            Reporter.Output.WriteLine("Usage: groupdocs-metadata [command] [source-file] [options]");
            Reporter.Output.WriteLine();
            Reporter.Output.WriteLine("Arguments:");
            Reporter.Output.WriteLine(" [command] can be: ");
            Reporter.Output.WriteLine($" {MainCommands.ExportCommand} - export file metadata to a specified 'output-format'. The default value is 'XML'.");
            Reporter.Output.WriteLine($" {MainCommands.RemoveCommand} - remove metadata from a file.");
            Reporter.Output.WriteLine($" {MainCommands.CopyCommand} - copy metadata between two files of the same type.");
            Reporter.Output.WriteLine($" {MainCommands.FindCommand} - find metadata properties in a file.");
            Reporter.Output.WriteLine("");
            Reporter.Output.WriteLine("Options:");
            Reporter.Output.WriteLine(" -h|--help         Display help.");
            Reporter.Output.WriteLine(" --version         Display CLI version in use.");

            string command = string.Empty;
            ICommand commandObj = null;

            if (args.Length != 0)
            {
                command = args[0];
            }

            if (command == MainCommands.ExportCommand)
            {
                commandObj = CommandFactory.Create(Common.Enums.CommandType.Export);

            }
            /*else if (command == MainCommands.GetViewInfoCommand)
            {
                commandObj = CommandFactory.Create(Common.Enums.CommandType.GetViewInfo);
            }*/

            if (!string.IsNullOrEmpty(command) && commandObj == null)
            {
                //Reporter.Error.WriteLine($"Unknown command {command} should be {MainCommands.ViewCommand} or {MainCommands.GetViewInfoCommand}");
                Reporter.Error.WriteLine($"Unknown command {command} should be {MainCommands.ExportCommand}");
            }

            if (commandObj != null)
            {
                commandObj.DisplayHelp(args);
            }
        }

        private static bool IsArg(string candidate, string longName)
        {
            return IsArg(candidate, shortName: null, longName: longName);
        }

        private static bool IsArg(string candidate, string shortName, string longName)
        {
            return (shortName != null && candidate.Equals("-" + shortName, StringComparison.OrdinalIgnoreCase)) ||
                   (longName != null && candidate.Equals("--" + longName, StringComparison.OrdinalIgnoreCase));
        }

        private static void PrintVersion()
        {
            Reporter.Output.WriteLine("CLI: " + Product.CLIVersion);
            Reporter.Output.WriteLine("GroupDocs.Metadata: " + Product.GroupDocsMetadataVersion);
        }
    }
}