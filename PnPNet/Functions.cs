namespace PnPNet
{
    internal static class Functions
    {
        internal static string Trim(string input)
        {
            input = input.Split(':')[1];
            input = input.TrimStart();
            return input;
        }
    }
}