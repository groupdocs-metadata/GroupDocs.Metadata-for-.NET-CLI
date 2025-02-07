using GroupDocs.Metadata.Cli.Common.Parameters.Base;
using GroupDocs.Metadata.Cli.Common.Parameters.ParseResults;

namespace GroupDocs.Metadata.Cli.Common.Parameters.Implementations
{
    /// <summary>
    /// Enable detailed (verbose) logging.
    /// </summary>
    public class AllParameter : Parameter<bool>
    {
        public override string ParameterName => "all";

        public override string ShortParameterName => "a";

        public override string Description => "Work with all properties.";

        public override void FillValidValues() { }

        public override string GetHelpText()
        {
            return "The flag indicates that all properties should be processed.";
        }

        public override ParameterParseResult<bool> Parse(string[] args)
        {
            return ParameterParseResult<bool>.CreateSuccessResult(this, ThisParameterExistInCommandLine(args));
        }
    }
}
