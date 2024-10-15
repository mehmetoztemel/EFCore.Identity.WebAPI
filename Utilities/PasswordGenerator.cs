namespace EFCore.Identity.WebAPI.Utilities
{
    public class PasswordGenerator
    {
        public static string GenerateRandomPassword(int length)
        {
            Random random = new Random();
            string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            char[] password = new char[length];

            for (int i = 0; i < password.Length; i++)
            {
                password[i] = characters[random.Next(characters.Length)];
            }
            return new string(password);
        }
    }
}
