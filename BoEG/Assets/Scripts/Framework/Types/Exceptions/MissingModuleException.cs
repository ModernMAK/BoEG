using System;

namespace Framework.Types.Exceptions
{
    public class MissingModuleException : NullReferenceException
    {
        public MissingModuleException() : base()
        {
        }
    }
}