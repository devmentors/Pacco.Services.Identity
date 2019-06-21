using System;

namespace Pacco.Services.Identity.Core.Exceptions
{
    public abstract class ExceptionBase : Exception
    {
        public abstract string Code { get; }

        public ExceptionBase(string message) : base(message)
        {
        }
    }
}