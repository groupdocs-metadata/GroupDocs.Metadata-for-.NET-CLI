using GroupDocs.Metadata.Cli.Common.Parameters.Base;
using GroupDocs.Metadata.Cli.Common.Parameters.ParseResults;

namespace GroupDocs.Metadata.Cli.Common.Parameters.Implementations
{
    /// <summary>
    /// Output path parameter.
    /// </summary>
    public class PropertyNameParameter : StringParameter
    {
        public override string ParameterName => "property";

        public override string ShortParameterName => "p";

        public override string Description => "Property name.";

        public override string GetHelpText()
        {
            return "Specifies a specific property name to work with.";
        }

        public override void FillValidValues() { }

        public override ParameterParseResult<string> Parse(string[] args)
        {
            return ParameterParseResult<string>.CreateSuccessResult(this, GetStringValue(args));
        }
    }
}
