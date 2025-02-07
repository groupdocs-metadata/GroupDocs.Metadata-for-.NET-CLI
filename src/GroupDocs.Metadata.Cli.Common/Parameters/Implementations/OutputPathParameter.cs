using GroupDocs.Metadata.Cli.Common.Enums;
using GroupDocs.Metadata.Cli.Common.Parameters.Base;
using GroupDocs.Metadata.Cli.Common.Parameters.ParseResults;

namespace GroupDocs.Metadata.Cli.Common.Parameters.Implementations
{
    /// <summary>
    /// Output path parameter.
    /// </summary>
    public class OutputPathParameter : StringParameter
    {
        public override string ParameterName => "output";

        public override string ShortParameterName => "o";

        public override string Description => "Output file path.";

        public override void FillValidValues() { }

        public override ParameterParseResult<string> Parse(string[] args)
        {
            ParameterParseResult<string> result = base.Parse(args);

            // Make default file name if it empty
            if (result.Success && (string.IsNullOrEmpty(result.ResultValue) || string.IsNullOrWhiteSpace(result.ResultValue)))
            {
                OutputFormatParameter outputFormatParameter = new OutputFormatParameter();

                // Make default file name depend on output format parameter (-f, --format)
                if (outputFormatParameter.ParseAndValidate(args))
                {
                    OutputFormat outputFormat = outputFormatParameter.LastValidationResult.ResultValue;
                    switch (outputFormat)
                    {
                        case OutputFormat.Xml:
                            result = ParameterParseResult<string>.CreateSuccessResult(this, "output.xml");
                            break;
                        case OutputFormat.Csv:
                            result = ParameterParseResult<string>.CreateSuccessResult(this, "output.csv");
                            break;
                        case OutputFormat.Json:
                            result = ParameterParseResult<string>.CreateSuccessResult(this, "output.json");
                            break;
                        case OutputFormat.Xls:
                            result = ParameterParseResult<string>.CreateSuccessResult(this, "output.xls");
                            break;
                        case OutputFormat.Xlsx:
                            result = ParameterParseResult<string>.CreateSuccessResult(this, "output.xlsx");
                            break;
                    }
                }
                else
                {
                    // If it not valid validation error for output format parameter will be catched at general validation.
                }
            }

            return result;
        }
    }
}
