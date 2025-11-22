using static System.Console;

namespace Compiler.IO
{
    /// <summary>
    /// An object for reporting errors in the compilation process
    /// </summary>
    public class ErrorReporter
    {
        /// <summary>
        /// The number of errors encountered so far
        /// </summary>
        public int ErrorCount { get; private set; } = 0;

        /// <summary>
        /// Whether or not any errors have been encounter
        /// </summary>
        public bool HasErrors { get { return ErrorCount > 0; } }

        /// <summary>
        /// Reports an error (legacy single-argument overload)
        /// </summary>
        /// <param name="message">The message to display</param>
        public void ReportError(string message)
        {
            // Delegate to the two-argument overload using a built-in position
            ReportError(message, Position.BuiltIn);
        }

        /// <summary>
        /// Reports an error with an associated source position
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="position">The source position where the error occurred</param>
        public void ReportError(string message, Position position)
        {
            ErrorCount += 1;
            WriteLine($"ERROR: {message} at {position}");
        }
    }
}