using System;

namespace Util
{
    public class MissingModuleException : NullReferenceException
    {
        public MissingModuleException() : base()
        {
        }
    }
}