using System;
using System.IO;
using System.Linq;
using GroupDocs.Metadata.Cli.Common.Commands.Interfaces;
using GroupDocs.Metadata.Cli.Common.Enums;
using GroupDocs.Metadata.Cli.Common.Parameters;
using GroupDocs.Metadata.Cli.Common.Parameters.Implementations;
using GroupDocs.Metadata.Cli.Common.Parameters.ParseResults;
using GroupDocs.Metadata.Cli.Common.Utils;
using GroupDocs.Metadata.Cli.Utils;
using GroupDocs.Metadata.Common;
using GroupDocs.Metadata.Logging;
using GroupDocs.Metadata.Options;

namespace GroupDocs.Metadata.Cli.Common.Commands
{
    /// <summary>
    /// Copy metadata between two file with same type
    /// </summary>
    internal class CopyCommand : ICommand
    {
        /// <summary>
        /// Source file name for export.
        /// </summary>
        private string SourceFileName { get; set; }

        /// <summary>
        /// File type for document load.
        /// </summary>
        private FileType FileType { get; set; } = FileType.Unknown;

        /// <summary>
        /// License path.
        /// </summary>
        private string LicensePath { get; set; }

        /// <summary>
        /// Destination file name.
        /// </summary>
        private string DestinationFileName { get; set; }

        /// <summary>
        /// Password for password protected documents.
        /// </summary>
        private string Password { get; set; }

        /// <summary>
        /// Parameter parsers container.
        /// </summary>
        ParameterParsersContainer ParameterParsersContainer { get; set; }

        /// <summary>
        /// Metadata command type
        /// </summary>
        public CommandType CommandType { get => CommandType.Copy; }

        /// <summary>
        /// Verbose logging enabled or not.
        /// </summary>
        private bool Verbose { get; set; }

        /// <summary>
        /// Execute command implementation.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public void Execute(string[] args = null)
        {
            // If license path set - set license.
            if (!string.IsNullOrEmpty(LicensePath))
            {
                License license = new License();
                license.SetLicense(LicensePath);
                Reporter.Output.WriteLine("License set.");
            }
            else
            {
                Reporter.Output.WriteLine("You are using the application in limited mode. Some functions may not be available. The full version can be purchased at the link - https://products.groupdocs.com/metadata/");
            }

            LoadOptions loadOptions = new LoadOptions(FileType.FileFormat)
            {
                Password = Password
            };

            // Initializing metadata with load options.
            Metadata metadataSource = new Metadata(SourceFileName, loadOptions);
            Metadata metadataDest = new Metadata(DestinationFileName, loadOptions);
            if (CommandContext.IsVerbose())
            {
                ConsoleLogger logger = new ConsoleLogger();
                GroupDocs.Metadata.Logging.Logging.Start(logger);
            }

            try
            {
                metadataSource.CopyTo(metadataDest.GetRootPackage());
                metadataDest.Save();

                var properties = metadataDest.FindProperties(p => p.Name != "").ToList();

                foreach (var property in properties)
                {
                    Reporter.Output.WriteLine($"Property Name: {property.Name}");
                    Reporter.Output.WriteLine($"Property Value: {property.Value.RawValue}");
                    if (!string.IsNullOrEmpty(property.InterpretedValue?.ToString()))
                        Reporter.Output.WriteLine($"Property InterpretedValue: {property.InterpretedValue}");
                }

                metadataSource.Dispose();
                metadataDest.Dispose();
            }
            catch (Exception e)
            {
                Reporter.Error.WriteLine(e.Message);
                metadataSource.Dispose();
                metadataDest.Dispose();
                throw;
            }
           

            if (CommandContext.IsVerbose())
            {
                GroupDocs.Metadata.Logging.Logging.Stop();
            }
            
        }

        /// <summary>
        /// Parse command-line arguments.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <returns>Parse result.</returns>
        public CommandLineParseResult Parse(string[] args)
        {
            string[] thisCommandArguments = args.Skip(1).ToArray();

            // expect: view type, source, destination
            if (thisCommandArguments.Length < 2)
            {
                return CommandLineParseResult.Failure(
                    $"{MainCommands.FindCommand} command should have at least 2 argument");
            }

            // Take source file name and creating parameters container.
            SourceFileName = thisCommandArguments[0];
            ParameterParsersContainer = CreateParameterContainer();

            CommandLineParseResult parameterParsersValidationResult = ParameterParsersContainer.ValidateAllParametersAndCheckIsValid(args);

            if (!parameterParsersValidationResult.Success)
            {
                return parameterParsersValidationResult;
            }

            // Take parsed and validated parameters values.
            SetParametersValues(ParameterParsersContainer);

            CommandContext.SetVerbose(Verbose);

            string fileType = ParameterParsersContainer.GetByParameterType<FileTypeParameter, string>().ResultValue;

            if (!string.IsNullOrEmpty(fileType))
            {
                FileType = FileType.FromExtension(fileType);
            }

            if (string.IsNullOrEmpty(DestinationFileName))
            {
                return CommandLineParseResult.Failure(
                    $"{MainCommands.CopyCommand} command failure: need destination file path.");
            }

            if (FileType.FromExtension(Path.GetExtension(SourceFileName)) != FileType.FromExtension(Path.GetExtension(DestinationFileName)))
            {
                return CommandLineParseResult.Failure(
                    $"{MainCommands.CopyCommand} command failure: source and destination files must be of the same type.");
            }

            return CommandLineParseResult.Successful();
        }

        /// <summary>
        /// Set parsed parameters values.
        /// </summary>
        /// <param name="parameterParsersContainer">Parameter parses container.</param>
        private void SetParametersValues(ParameterParsersContainer parameterParsersContainer)
        {
            LicensePath = parameterParsersContainer.GetByParameterType<LicensePathParameter, string>().ResultValue;
            Password = parameterParsersContainer.GetByParameterType<PasswordParameter, string>().ResultValue;
            Verbose = parameterParsersContainer.GetByParameterType<VerboseParameter, bool>().ResultValue;
            DestinationFileName = parameterParsersContainer.GetByParameterType<OutputPathParameter, string>().ResultValue;
        }

        /// <summary>
        /// Create and fill parameters container for register, parse and validation.
        /// </summary>
        /// <returns>Parameter parsers container.</returns>
        private static ParameterParsersContainer CreateParameterContainer()
        {
            ParameterParsersContainer parameterParsersContainer = new ParameterParsersContainer();
            parameterParsersContainer.RegisterParameter(new LicensePathParameter());
            parameterParsersContainer.RegisterParameter(new FileTypeParameter());
            parameterParsersContainer.RegisterParameter(new PasswordParameter());
            parameterParsersContainer.RegisterParameter(new VerboseParameter());

            parameterParsersContainer.RegisterParameter(new AllParameter());
            parameterParsersContainer.RegisterParameter(new PropertyNameParameter());
            parameterParsersContainer.RegisterParameter(new OutputPathParameter());


            return parameterParsersContainer;
        }

        /// <summary>
        /// Display help for view command.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public void DisplayHelp(string[] args)
        {
            DisplayHelpHelper.DisplayHelp(args, CreateParameterContainer());
        }
    }
}
