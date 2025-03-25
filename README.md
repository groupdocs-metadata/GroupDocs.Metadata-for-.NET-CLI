# CLI for GroupDocs.Metadata for .NET

![Nuget](https://img.shields.io/nuget/v/groupdocs.metadata-cli)
![Nuget](https://img.shields.io/nuget/dt/groupdocs.metadata-cli)


CLI - Command Line Interface for [GroupDocs.Metadata for .NET](https://products.groupdocs.com/metadata/net) document metadata and automation API.

## How to install

GroupDocs.Metadata CLI is a dotnet tool. To start using the CLI you'll need .NET runtime and GroupDocs.Metadata CLI.

1. Install .NET Core runtime following by the [instructions](https://docs.microsoft.com/en-us/dotnet/core/install/)
2. Install dotnet tool by running `dotnet tool install --global groupDocs.metadata-cli`
3. You can run GroupDocs.Metadata.CLI by using command `groupdocs-metadata`

## Example usage

Type `export` command and source filename to export metadata properties and place the output in the current directory:

```bash
groupdocs-metadata export source.docx
```

Set `output-format` parameter value to `JSON` to export properties to JSON file:

```bash
groupdocs-metadata export source.docx --output-format JSON
```

The `--help` or `export --help` option provides more detail about each parameter. \
The `--version` option provides information about CLI version in use.

## Commands

* `export` file metadata to a specified `output-format`. The default value is `XML`.

* `remove` metadata from a file.

* `copy` metadata between two files of the same type.

* `find` find metadata properties in a file.

## Parameters

### Parameters for "export" command

* `--output-format` [short `-f`]: Output format, supported values are `Xml`, `Csv`, `Json`, `Xls`, and `Xlsx`.

* `--output` [short `-o`]: Output path.

### Parameters for "remove" command

* `--all` [short `-a`]: The flag indicates that all properties should be processed.

* `--property` [short `-p`]: Specifies a specific property name to work with.

### Parameters for "copy" command

* `--all` [short `-a`]: The flag indicates that all properties should be processed.

* `--property` [short `-p`]: Specifies a specific property name to work with.

* `--output-format` [short `-f`]: Output format, supported values are `Xml`, `Csv`, `Json`, `Xls`, and `Xlsx`.

* `--output` [short `-o`]: Output path.

### Parameters for "find" command

* `--all` [short `-a`]: The flag indicates that all properties should be processed.

* `--property` [short `-p`]: Specifies a specific property name to work with.

### Parameters for all commands

* `--license-path`: Path to license file.

* `--file-type`: Source document file type e.g. `DOCX`.

* `--password` [short: `-pwd`]: Password to open password-protected file.

* `--output-format` [short `-f`]: Output format, supported values are `Xml`, `Csv`, `Json`, `Xls`, and `Xlsx`.

* `--verbose` [short `-v`]: Enable detailed logging to console.



## Setting the license

Without a license the tool will work in trial mode so you can convert only first two pages of a document see [Evaluation Limitations and Licensing of GroupDocs.Metadata](https://docs.groupdocs.com/metadata/net/evaluation-limitations-and-licensing-of-groupdocs-metadata/) for more details. A temporary license can be requested at [Get a Temporary License](https://purchase.groupdocs.com/temporary-license).

The license can be set with `--license-path` parameter:

```bash
groupdocs-metadata view source.docx --license-path c:\\licenses\\GroupDocs.Metadata.lic
```

Also, you can set path to the license file in `GROUPDOCS_METADATA_LICENSE_PATH` environment variable.