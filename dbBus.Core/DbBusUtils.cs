namespace dbBus.Core
{
    using System;

    public static class DbBusUtils
    {
        public static Exception GetMostInnerException(Exception e)
        {
            return e.InnerException != null ? 
                       GetMostInnerException(e.InnerException) : e;
        }
    }
}