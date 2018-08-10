using System;

namespace Old.Entity.Core
{
    public class MissingModuleException : NullReferenceException
    {
        public MissingModuleException() : base()
        {
        }
    }
}