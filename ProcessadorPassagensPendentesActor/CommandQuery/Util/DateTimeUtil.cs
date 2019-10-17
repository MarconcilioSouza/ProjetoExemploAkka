using System;

namespace ProcessadorPassagensActors.CommandQuery.Util
{
    public class DateTimeUtil
    {
        public static IDateTimeHelper Helper
        {
            get => _helper;
            set => _helper = value;
        }

        private static DateTimeUtil _util = new DateTimeUtil();
        private static IDateTimeHelper _helper = new DateTimeHelper();

        public static DateTimeUtil Util
        {
            get => _util ?? (_util = new DateTimeUtil());
            set => _util = value;
        }

        public static DateTime Today => Util.TodayHelper();

        public static DateTime Now => Util.NowHelper();

        public virtual DateTime TodayHelper()
        {
            return Helper.Today();
        }

        public virtual DateTime NowHelper()
        {
            return Helper.Now();
        }
    }
}
