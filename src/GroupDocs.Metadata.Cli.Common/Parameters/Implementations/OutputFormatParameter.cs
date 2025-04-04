﻿using System;
using System.Linq;
using GroupDocs.Metadata.Cli.Common.Enums;
using GroupDocs.Metadata.Cli.Common.Parameters.Base;
using GroupDocs.Metadata.Cli.Common.Parameters.ParseResults;
using GroupDocs.Metadata.Cli.Utils;

namespace GroupDocs.Metadata.Cli.Common.Parameters.Implementations
{
    /// <summary>
    /// Output document format parameter.
    /// </summary>
    public class OutputFormatParameter : Parameter<OutputFormat>
    {
        public override string ParameterName => "output-format";

        public override string ShortParameterName => "f";

        public override string Description => "Output format, supported values are `Xml`, `Csv`, `Json`, `Xls`, and `Xlsx`.";

        protected override OutputFormat DefaultValue => OutputFormat.Xml;

        public override void FillValidValues()
        {
            foreach (OutputFormat value in Enum.GetValues(typeof(OutputFormat)))
            {
                ValidValues.Add(value.ToString());
            }
        }

        /// <summary>
        /// Allow case insensetive values here
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <returns>Valid or not</returns>
        protected override bool CheckValueIsValid(string value)
        {
            return ValidValues.Any(x => x.Equals(value, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Parse value from command-line and parse it with enum.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <returns>Parameter parse result.</returns>
        public override ParameterParseResult<OutputFormat> Parse(string[] args)
        {
            string rawValue = GetStringValue(args);

            if (!string.IsNullOrEmpty(rawValue))
            {
                try
                {
                    return ParameterParseResult<OutputFormat>.CreateSuccessResult(this, Enum.Parse<OutputFormat>(rawValue, true));
                }
                catch (Exception ex)
                {
                    Reporter.Error.WriteLine(ex.Message);
                    return ParameterParseResult<OutputFormat>.CreateFailedResult(this, ex.Message);
                }
            }
            else
            {
                // Use HTML with internal resources rendering - by default.
                return ParameterParseResult<OutputFormat>.CreateSuccessResult(this, OutputFormat.Xml);
            }
        }
    }
}
