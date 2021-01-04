namespace RPN_Locale
{
    public static class RegularExpression
    {
        public const string Rpn = @"^(\d*\.?\d*)(( (\d*\.?\d*))+( ([\+\-\*\/^%]|root|log))+)+";
        public const string HistoryString = @"^History{.*}$";
        public const string ReportString = @"^Report{.*}$";
        public const string ApplicationString = @"^(\d*),(.*),(\d{1,2}.\d{1,2}.\d{4} \d{1,2}:\d{1,2}:\d{1,2})$";
        public const string ReportWithGroup = @"^report\s(?<message>.*)";
        public const string Report = @"^report\s.*";
        public const string ReportGroup = "message";
    }
}