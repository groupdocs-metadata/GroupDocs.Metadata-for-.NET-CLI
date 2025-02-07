namespace GroupDocs.Metadata.Cli.Common.Enums
{
    /// <summary>
    /// Metadata command type.
    /// </summary>
    public enum CommandType
    {
        /// <summary>
        /// Export document metadata to selected output format (CSV/JSON/XML/XLS/XLSX).
        /// </summary>
        Export,

        /// <summary>
        /// Find document metadata.
        /// </summary>
        Find,

        /// <summary>
        /// Remove metadata property from document.
        /// </summary>
        Remove,

        /// <summary>
        /// Copy metadata properties between document.
        /// </summary>
        Copy,
    }
}
