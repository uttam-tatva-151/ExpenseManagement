namespace CashCanvas.Common.ExceptionHandler;

using System.Text;

public static class DataProtectionHelper
{
    /// <summary>
    /// Encodes the given plain text into a Base64 string.
    /// </summary>
    /// <param name="plainText">The plain text to encode.</param>
    /// <returns>The Base64 encoded string.</returns>
    public static string Encode(string plainText)
    {
        if (string.IsNullOrWhiteSpace(plainText))
            throw new ArgumentException("Input cannot be null or empty.", nameof(plainText));

        return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
    }

    /// <summary>
    /// Decodes the given Base64 string back into plain text.
    /// </summary>
    /// <param name="base64Text">The Base64 encoded string to decode.</param>
    /// <returns>The decoded plain text.</returns>
    public static string Decode(string base64Text)
    {
        if (string.IsNullOrWhiteSpace(base64Text))
            throw new ArgumentException("Input cannot be null or empty.", nameof(base64Text));

        return Encoding.UTF8.GetString(Convert.FromBase64String(base64Text));
    }
}