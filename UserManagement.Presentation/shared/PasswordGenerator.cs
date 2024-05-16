namespace UserManagement.Presentation.shared
{
    public class PasswordGenerator
    {
        public static string GenerateRandomPassword()
        {
            const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
            const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digitChars = "0123456789";

            string allChars = lowercaseChars + uppercaseChars + digitChars;

            Random random = new Random();

          
            string password = $"{GetRandomChar(lowercaseChars, random)}" +
                              $"{GetRandomChar(uppercaseChars, random)}" +
                              $"{GetRandomChar(digitChars, random)}";

            
            for (int i = 4; i < 8; i++)
            {
                password += GetRandomChar(allChars, random);
            }
           
            password = new string(password.ToCharArray().OrderBy(c => Guid.NewGuid()).ToArray());

            return password;
        }

        private static char GetRandomChar(string charSet, Random random)
        {
            int index = random.Next(charSet.Length);
            return charSet[index];
        }
    }
}
