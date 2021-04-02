namespace Utilities.Helpers
{
    public static class General
    {
        public static string ToPluralString(this int num)
        {
            return $"{(num > 1 ? "s" : "")}";
        }
        
        public static string ToPluralString(this float num)
        {
            return $"{(num > 1 ? "s" : "")}";
        }
        
        public static string ToPluralString(this int num, string unit)
        {
            return $"{num}{unit}{(num > 1 ? "s" : "")}";
        }
        
        public static string ToPluralString(this float num, string unit)
        {
            return $"{num}{unit}{(num > 1 ? "s" : "")}";
        }
    }
}