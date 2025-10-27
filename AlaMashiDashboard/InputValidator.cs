public static class InputValidator
{
    private static readonly string _emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    private static readonly string _specialChars = "!@#$%^&*(),.?\":{}|<>";

    public static string? ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) 
            return "Email is required";

        return System.Text.RegularExpressions.Regex.IsMatch(email, _emailPattern) ? null: "Invalid email format";
    }

    public static string? ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) return "Password is required";
        if (password.Length < 8) return "Password must be at least 8 characters";
        if (!password.Any(char.IsDigit)) return "Password must contain at least one number";
        if (!password.Any(c => _specialChars.Contains(c))) return "Password must contain at least one special character";
        return null;
    }
}
