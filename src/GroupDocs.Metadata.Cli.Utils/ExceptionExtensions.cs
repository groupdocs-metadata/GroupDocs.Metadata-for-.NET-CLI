﻿using System;

namespace GroupDocs.Metadata.Cli.Utils
{
    internal static class ExceptionExtensions
    {
        public static TException DisplayAsError<TException>(this TException exception)
        where TException : Exception
        {
            exception.Data.Add(CLI_User_Displayed_Exception, true);
            return exception;
        }

        public static void ReportAsWarning(this Exception e)
        {
            //Reporter.Verbose.WriteLine($"Warning: Ignoring exception: {e.ToString().Yellow()}");
        }

        public static bool ShouldBeDisplayedAsError(this Exception e) =>
            e.Data.Contains(CLI_User_Displayed_Exception);

        internal const string CLI_User_Displayed_Exception = "CLI_User_Displayed_Exception";
    }
}
