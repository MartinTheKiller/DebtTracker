namespace DebtTracker.BL.Hashers;

public class UserPasswordHasher : IUserPasswordHasher
{
    public string HashPassword(string password)
    {
        string salt = BCrypt.Net.BCrypt.GenerateSalt(10);
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

        return hashedPassword;
    }

    public bool VerifyHashedPassword(string providedPassword, string hashedPassword) =>
        BCrypt.Net.BCrypt.Verify(providedPassword,hashedPassword);
}