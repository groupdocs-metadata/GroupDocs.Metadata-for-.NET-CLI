using GroupDocs.Metadata.Cli.Common.Parameters.ParseResults;

namespace GroupDocs.Metadata.Cli.Common.Parameters.Base
{
    /// <summary>
    /// Simple string parameter template.
    /// </summary>
    public abstract class StringParameter : Parameter<string>
    {
        protected StringParameter() : base()
        {
        }

        /// <summary>
        /// Parse string parameter implementation
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>Typed parameter parse result</returns>
        public override ParameterParseResult<string> Parse(string[] args)
        {
            string rawValue = GetStringValue(args);

            if (string.IsNullOrEmpty(rawValue))
            {
                return ParameterParseResult<string>.CreateSuccessResult(this, DefaultValue);
            }
            else
            {
                return ParameterParseResult<string>.CreateSuccessResult(this, rawValue);
            }
        }
    }
}
