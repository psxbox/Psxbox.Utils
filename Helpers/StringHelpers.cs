namespace Psxbox.Utils.Helpers;

public static class StringHelpers
{
    private static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var randomString = new string([.. Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)])]);
        return randomString;
    }
}
