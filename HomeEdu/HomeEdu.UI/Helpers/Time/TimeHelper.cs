namespace HomeEdu.UI.Helpers.Time
{
    public class TimeHelper
    {
        private int _days;
        private int _months;

        public int Days
        {
            get => _days;
            set
            {
                _days = value;
                AdjustDuration();
            }
        }

        public int Months
        {
            get => _months;
            set
            {
                _months = value;
                AdjustDuration();
            }
        }

        public int TotalMonths { get; private set; }
        public int TotalDays { get; private set; }

        private void AdjustDuration()
        {
            TotalMonths = Months + (Days / 31);
            TotalDays = Days % 31;
        }
    }
}