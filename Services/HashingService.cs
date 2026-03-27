using System.Security.Cryptography;
using System;
using System.Text;


namespace E_commerce_PetShop.Services
{
    public class HashingService
    {
        public static string HashPassword(string usersData)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(usersData);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder builder1 = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder1.Append(hashBytes[i].ToString("x2"));
                }
                return builder1.ToString();

            }
        }

    }
}
