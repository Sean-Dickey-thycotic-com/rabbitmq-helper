using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Thycotic.Logging
{
    /// <summary>
    /// Log context
    /// </summary>
    public sealed class LogContext : IDisposable
    {
        /// <summary>
        /// The context name
        /// </summary>
        public const string ContextName = "Context";

        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly IDisposable _context;

        [DebuggerStepThrough]
        private LogContext(string name)
        {
            _context = log4net.ThreadContext.Stacks[ContextName].Push(name);
            _stopwatch.Start();
        }

        /// <summary>
        /// Creates the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static LogContext Create(string name)
        {
            Contract.Ensures(Contract.Result<LogContext>() != null);

            return new LogContext(string.Format(CultureInfo.InvariantCulture, "> {0}", name));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [DebuggerStepThrough]
        public void Dispose()
        {
            _stopwatch.Stop();

            var log = Log.Get(typeof(Stopwatch));

            log.Debug(string.Format(CultureInfo.InvariantCulture, "[ElapsedMilliseconds: {0}]", _stopwatch.ElapsedMilliseconds));

            _context.Dispose();
        }
    }
}
