using System;
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
    /// Find metadata properties in file.
    /// </summary>
    internal class FindCommand : ICommand
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
        /// Password for password protected documents.
        /// </summary>
        private string Password { get; set; }

        /// <summary>
        /// Find all properties.
        /// </summary>
        private bool All { get; set; }

        /// <summary>
        /// Property name.
        /// </summary>
        private string PropertyName { get; set; }

        /// <summary>
        /// Parameter parsers container.
        /// </summary>
        ParameterParsersContainer ParameterParsersContainer { get; set; }

        /// <summary>
        /// Metadata command type
        /// </summary>
        public CommandType CommandType { get => CommandType.Find; }

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
            Metadata metadata = new Metadata(SourceFileName, loadOptions);
            if (CommandContext.IsVerbose())
            {
                ConsoleLogger logger = new ConsoleLogger();
                GroupDocs.Metadata.Logging.Logging.Start(logger);
            }

            try
            {
                var properties = All ? metadata.FindProperties(p => p.Name != "").ToList() : metadata.FindProperties(p => p.Name == PropertyName).ToList();

                if (properties.Count == 0)
                {
                    Reporter.Output.WriteLine($"Not find property: {PropertyName}");
                }
                else
                {
                    foreach (var property in properties)
                    {
                        Reporter.Output.WriteLine($"Property Name: {property.Name}");
                        Reporter.Output.WriteLine($"Property Value: {property.Value.RawValue}");
                        if (!string.IsNullOrEmpty(property.InterpretedValue?.ToString()))
                            Reporter.Output.WriteLine($"Property InterpretedValue: {property.InterpretedValue}");
                    }
                }
                

                metadata.Dispose();
            }
            catch (Exception e)
            {
                Reporter.Error.WriteLine(e.Message);
                metadata.Dispose();
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
            if (thisCommandArguments.Length < 1)
            {
                return CommandLineParseResult.Failure(
                    $"{MainCommands.FindCommand} command should have at least 1 argument");
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

            if(All == false && string.IsNullOrEmpty(PropertyName))
                return CommandLineParseResult.Failure(
                    $"{MainCommands.FindCommand} command should have a -all flag or -name");

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
            All = ParameterParsersContainer.GetByParameterType<AllParameter, bool>().ResultValue;
            PropertyName = ParameterParsersContainer.GetByParameterType<PropertyNameParameter, string>().ResultValue;
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
