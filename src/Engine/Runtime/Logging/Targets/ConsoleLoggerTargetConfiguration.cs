using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace Pandora.Runtime.Logging.Targets
{
    public sealed class ConsoleLoggerTargetConfiguration : LoggerTargetConfiguration<ConsoleLoggerTarget>
    {
        internal ConsoleLoggerTargetConfiguration(ConsoleLoggerTarget target)
            : base(target)
        { }

        public ConsoleLoggerTargetConfiguration SetColorTrace(ConsoleColor color)
        {
            Target.TraceColor = color;
            return this;
        }

        public ConsoleLoggerTargetConfiguration SetColorDebug(ConsoleColor color)
        {
            Target.DebugColor = color;
            return this;
        }

        public ConsoleLoggerTargetConfiguration SetColorNormal(ConsoleColor color)
        {
            Target.NormalColor = color;
            return this;
        }

        public ConsoleLoggerTargetConfiguration SetColorWarning(ConsoleColor color)
        {
            Target.WarningColor = color;
            return this;
        }

        public ConsoleLoggerTargetConfiguration SetColorError(ConsoleColor color)
        {
            Target.ErrorColor = color;
            return this;
        }

        public ConsoleLoggerTargetConfiguration SetColorException(ConsoleColor color)
        {
            Target.ExceptionColor = color;
            return this;
        }
    }
}
