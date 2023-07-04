namespace DebtTracker.BL.Hashers;

public interface IUserPasswordHasher
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string providedPassword, string hashedPassword);
}