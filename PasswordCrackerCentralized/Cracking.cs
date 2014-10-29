using System.Diagnostics;
using PasswordCrackerCentralized.model;
using PasswordCrackerCentralized.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace PasswordCrackerCentralized
{
    public class Cracking
    {
       

        /// <summary>
        /// The algorithm used for encryption.
        /// Must be exactly the same algorithm that was used to encrypt the passwords in the password file
        /// </summary>
        

        public Cracking()
        {
            //_messageDigest = new MD5CryptoServiceProvider();
            // seems to be same speed¨
           
        }
        
        /// <summary>
        /// Runs the password cracking algorithm
        /// </summary>
        public void RunCracking(string DictionaryFile)
        {
            HashAlgorithm _messageDigest;
            _messageDigest = new SHA1CryptoServiceProvider();

            Stopwatch stopwatch = Stopwatch.StartNew();

            List<UserInfo> userInfos = PasswordFileHandler.ReadPasswordFile("passwords.txt");
            List<UserInfoClearText> result = new List<UserInfoClearText>();
            using (FileStream fs = new FileStream(DictionaryFile, FileMode.Open, FileAccess.Read))
            using (StreamReader dictionary = new StreamReader(fs))
            {
                
                   while (!dictionary.EndOfStream)
                    {
                        String dictionaryEntry = dictionary.ReadLine();
                        
                        IEnumerable<UserInfoClearText> partialResult = CheckWordWithVariations(dictionaryEntry,
                            userInfos, _messageDigest);
                        result.AddRange(partialResult);
                    }
                
                
            }
            stopwatch.Stop();
            Console.WriteLine(string.Join(", ", result));
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
        }

        /// <summary>
        /// Generates a lot of variations, encrypts each of the and compares it to all entries in the password file
        /// </summary>
        /// <param name="dictionaryEntry">A single word from the dictionary</param>
        /// <param name="userInfos">List of (username, encrypted password) pairs from the password file</param>
        /// <param name="_messageDigest">List of (username, encrypted password) pairs from the password file</param>
        /// <returns>A list of (username, readable password) pairs. The list might be empty</returns>
        private IEnumerable<UserInfoClearText> CheckSingleWord(IEnumerable<UserInfo> userInfos, String possiblePassword, HashAlgorithm _messageDigest)
        {
            char[] charArray = possiblePassword.ToCharArray();
            byte[] passwordAsBytes = Array.ConvertAll(charArray, PasswordFileHandler.GetConverter());
            byte[] encryptedPassword = _messageDigest.ComputeHash(passwordAsBytes);
            //string encryptedPasswordBase64 = System.Convert.ToBase64String(encryptedPassword);

            List<UserInfoClearText> results = new List<UserInfoClearText>();
            foreach (UserInfo userInfo in userInfos)
            {
                if (CompareBytes(userInfo.EntryptedPassword, encryptedPassword))
                {
                    results.Add(new UserInfoClearText(userInfo.Username, possiblePassword));
                    Console.WriteLine(userInfo.Username + " " + possiblePassword);
                }
            }
            return results;
        }
         private IEnumerable<UserInfoClearText> CheckWordWithVariations(string dictionaryEntry, List<UserInfo> userInfos, HashAlgorithm _messageDigest)
        {
            List<UserInfoClearText> result = new List<UserInfoClearText>();

            String possiblePassword = dictionaryEntry;
            IEnumerable<UserInfoClearText> partialResult = CheckSingleWord(userInfos, possiblePassword, _messageDigest);
            result.AddRange(partialResult);

            String possiblePasswordUpperCase = dictionaryEntry.ToUpper();
            IEnumerable<UserInfoClearText> partialResultUpperCase = CheckSingleWord(userInfos, possiblePasswordUpperCase, _messageDigest);
            result.AddRange(partialResultUpperCase);

            String possiblePasswordCapitalized = StringUtilities.Capitalize(dictionaryEntry);
            IEnumerable<UserInfoClearText> partialResultCapitalized = CheckSingleWord(userInfos, possiblePasswordCapitalized, _messageDigest);
            result.AddRange(partialResultCapitalized);

            String possiblePasswordReverse = StringUtilities.Reverse(dictionaryEntry);
            IEnumerable<UserInfoClearText> partialResultReverse = CheckSingleWord(userInfos, possiblePasswordReverse, _messageDigest);
            result.AddRange(partialResultReverse);

            for (int i = 0; i < 100; i++)
            {
                String possiblePasswordEndDigit = dictionaryEntry + i;
                IEnumerable<UserInfoClearText> partialResultEndDigit = CheckSingleWord(userInfos, possiblePasswordEndDigit, _messageDigest);
                result.AddRange(partialResultEndDigit);
            }

            for (int i = 0; i < 100; i++)
            {
                String possiblePasswordStartDigit = i + dictionaryEntry;
                IEnumerable<UserInfoClearText> partialResultStartDigit = CheckSingleWord(userInfos, possiblePasswordStartDigit, _messageDigest);
                result.AddRange(partialResultStartDigit);
            }

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    String possiblePasswordStartEndDigit = i + dictionaryEntry + j;
                    IEnumerable<UserInfoClearText> partialResultStartEndDigit = CheckSingleWord(userInfos, possiblePasswordStartEndDigit, _messageDigest);
                    result.AddRange(partialResultStartEndDigit);
                }
            }

            return result;
        }

        /// <summary>
        /// Checks a single word (or rather a variation of a word): Encrypts and compares to all entries in the password file
        /// </summary>
        /// <param name="userInfos"></param>
        /// <param name="possiblePassword">List of (username, encrypted password) pairs from the password file</param>
        /// <returns>A list of (username, readable password) pairs. The list might be empty</returns>


        /// <summary>
        /// Compares to byte arrays. Encrypted words are byte arrays
        /// </summary>
        /// <param name="firstArray"></param>
        /// <param name="secondArray"></param>
        /// <returns></returns>
        private static bool CompareBytes(IList<byte> firstArray, IList<byte> secondArray)
        {
            //if (secondArray == null)
            //{
            //    throw new ArgumentNullException("firstArray");
            //}
            //if (secondArray == null)
            //{
            //    throw new ArgumentNullException("secondArray");
            //}
            if (firstArray.Count != secondArray.Count)
            {
                return false;
            }
            for (int i = 0; i < firstArray.Count; i++)
            {
                if (firstArray[i] != secondArray[i])
                    return false;
            }
            return true;
        }

    }
}
