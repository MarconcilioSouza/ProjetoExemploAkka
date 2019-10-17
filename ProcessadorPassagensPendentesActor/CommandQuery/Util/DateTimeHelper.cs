using System;

namespace ProcessadorPassagensActors.CommandQuery.Util
{
    public class DateTimeHelper : IDateTimeHelper
    {
        public DateTime Today()
        {
            return DateTime.Today;
        }

        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}
