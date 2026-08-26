using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DojoStudentManagement
{
    /// <summary>
    /// Builds RFC 4180 style CSV. Student names, dojo names and notes routinely contain commas
    /// and quotes, so fields are escaped rather than concatenated.
    /// </summary>
    public static class CsvWriter
    {
        private static readonly char[] CharactersRequiringQuotes = { ',', '"', '\r', '\n' };

        /// <summary>
        /// Quotes a field when it contains a comma, a quote or a line break, doubling any quotes
        /// inside it. Null becomes an empty field.
        /// </summary>
        public static string EscapeField(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (value.IndexOfAny(CharactersRequiringQuotes) < 0)
                return value;

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        public static string BuildLine(IEnumerable<string> fields)
        {
            if (fields == null)
                return string.Empty;

            return string.Join(",", fields.Select(EscapeField));
        }

        /// <summary>
        /// Assembles a full CSV document. Lines are separated with CRLF, which is what Excel
        /// expects on Windows.
        /// </summary>
        public static string BuildCsv(IEnumerable<string> headers, IEnumerable<IEnumerable<string>> rows)
        {
            var builder = new StringBuilder();

            if (headers != null)
                builder.Append(BuildLine(headers)).Append("\r\n");

            if (rows != null)
            {
                foreach (IEnumerable<string> row in rows)
                    builder.Append(BuildLine(row)).Append("\r\n");
            }

            return builder.ToString();
        }

        /// <summary>
        /// UTF-8 with a byte order mark: without the BOM Excel misreads non-ASCII names in the
        /// roster as mojibake.
        /// </summary>
        public static void WriteToFile(string filePath, string csvContent)
        {
            System.IO.File.WriteAllText(filePath, csvContent, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
        }
    }
}
