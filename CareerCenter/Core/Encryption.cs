using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace CareerCenter
{

    public static class Encryption
    {
        static string is_DefaultKey = "Exper1$";  //KTC: For overloaded reversible encryption only (Should always provide a separate key for increased security)
        static int ii_FailLimit = 5; //Number of times a user can attempt to login without being locked out.
        static int ii_Timeout = 30; //Number of minutes before a token will timeout.

        #region Encryption Entry Points

        public static bool CreateUser(string as_UserName, string as_Password, string as_Email, bool ab_Available)
        {
            bool lb_Return = false;
            string ls_Salt = Guid.NewGuid().ToString().Replace("-", "");
            string ls_Hash = EncodePassword(as_Password, ls_Salt);
            SqlCommand lo_Command = new SqlCommand();

            try
            {
                if (DataHandler.InitializeCommandFromProcedure(ref lo_Command, "CreateUser"))
                {
                    int li_Result_ID;
                    string ls_Result_Msg;

                    DataHandler.SetVal(ref lo_Command, "@appuser_name", as_UserName);
                    DataHandler.SetVal(ref lo_Command, "@appuser_salt", ls_Salt);
                    DataHandler.SetVal(ref lo_Command, "@appuser_hash", ls_Hash);
                    DataHandler.SetVal(ref lo_Command, "@appuser_email", as_Email);
                    DataHandler.SetVal(ref lo_Command, "@appuser_available", ab_Available);

                    lo_Command.ExecuteNonQuery();

                    li_Result_ID = int.Parse(lo_Command.Parameters["@Result_ID"].Value.ToString());
                    ls_Result_Msg = lo_Command.Parameters["@Result_Msg"].Value.ToString();

                    if (li_Result_ID < 0)
                    {
                        DataHandler.HandleError("Encryption.CreateUser()", ls_Result_Msg, "User = " + as_UserName);
                    }
                    else
                    {
                        lb_Return = true;
                    }

                    lo_Command.Dispose();

                }
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
                lb_Return = false; //For good measure :)
            }

            return lb_Return;
        }


        public static string ValidateUser(string as_User, string as_Password)
        {
            // Accepts user credentials and if valid, returns token
            string ls_Return = "";
            string ls_SQL = "Select * from appuser where appuser_name='" + as_User + "' and appuser_available=1";
            string ls_Update = "";
            
            DataSet lo_Check = new DataSet();

            try
            {
                if (DataHandler.GetDatasetFromQuery(ref lo_Check, ls_SQL))
                {
                    //User Found.  Encrypt to see if the hashes match
                    string ls_Hash = lo_Check.Tables[0].Rows[0]["appuser_hash"].ToString();
                    string ls_Salt = lo_Check.Tables[0].Rows[0]["appuser_hash"].ToString();

                    if (ls_Hash == EncodePassword(as_Password, ls_Salt))
                    {
                        //We have a match!
                        ls_Return = Guid.NewGuid().ToString().Replace("-", "");
                        ls_Update = "update appuser set appuser_token ='" + ls_Return + "', appuser_lastlogin=getdate(), appuser_fails = 0 where appuser_name='" + as_User +"'";
                    }
                    else
                    {
                        int li_Fail = int.Parse(lo_Check.Tables[0].Rows[0]["appuser_fails"].ToString());
                        if (li_Fail >= ii_FailLimit)
                        {
                            ls_Update = "update appuser set appuser_available = 0 where appuser_name='" + as_User + "'";
                        }
                        else
                        {
                            ls_Update = "update appuser set appuser_fails = appuser_fails + 1 where appuser_name='" + as_User + "'";
                        }
                        ls_Return = ""; //None shall pass....
                    }

                    //Perform cleanup:
                    SqlCommand lo_Update = new SqlCommand();
                    DataHandler.InitializeCommandFromQuery(ref lo_Update, ls_Update);
                    lo_Update.ExecuteNonQuery();
                    lo_Update.Dispose();
                }
                else
                {
                    //Something went wrong.  No records were returned or worse...  Send back nada.
                    ls_Return = ""; //Keeping the code honest :)
                }
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
                ls_Return = ""; //Also, to keep the code honest :)
            }

            lo_Check.Dispose();

            return ls_Return;
        }

        public static string ValidateToken(string as_Token)
        {
            //Checks token and if valid, returns username
            string ls_Return = "";
            string ls_SQL = "Select * from appuser where appuser_token='" + as_Token + "' and appuser_lastlogin > getdate()-(" + ii_Timeout.ToString()+".00/1440.00)"; //Note: 1440 minutes in a day.

            DataSet lo_Check = new DataSet();

            try
            {

                if (DataHandler.GetDatasetFromQuery(ref lo_Check, ls_SQL))
                {
                    //We found a record.  There can be only one (Highlander reference)...  Anyway, return User Name associated with the token.
                    ls_Return = lo_Check.Tables[0].Rows[0]["appuser_name"].ToString();
                }
                else
                {
                    //Something went wrong.  No records were returned or worse...  Send back nada.
                    ls_Return = ""; //Keeping the code honest :)
                }
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
                ls_Return = ""; //Also, to keep the code honest :)
            }

            lo_Check.Dispose();

            return ls_Return;
        }

        public static string EncodePassword(string as_Password, string as_Salt)
        {
            // KTC: 06/12/2015 - Adding Encryption
            // Note that this generates a one-way hash for supplied password using MD5.  Can't decode even if we wanted to.
            // Also note that with HMAC MD5, salt and the item to be encrypted must be provided as byte arrays.
            // Response is a byte array too, so convert to string before returning value.

            Byte[] lbt_OriginalBytes;
            Byte[] lbt_EncodedBytes;
            Byte[] lbt_SaltBytes;

            // Convert the original password to bytes; then create the hash
            lbt_OriginalBytes = ASCIIEncoding.Default.GetBytes(as_Password);
            lbt_SaltBytes = ASCIIEncoding.Default.GetBytes(as_Password);
            HMACMD5 lo_Crypto = new HMACMD5(lbt_SaltBytes);
            lbt_EncodedBytes = lo_Crypto.ComputeHash(lbt_OriginalBytes);


            // Return after converting bytes to string
            return System.Text.RegularExpressions.Regex.Replace(BitConverter.ToString(lbt_EncodedBytes), "-", "").ToLower();
        }

        public static string Encode(string as_Input)
        {
            string ls_Return = "";
            ls_Return = Encrypt(as_Input, is_DefaultKey);

            return ls_Return;
        }

        public static string Encode(string as_Input, string as_Key)
        {
            string ls_Return = "";
            ls_Return = Encrypt(as_Input, as_Key);

            return ls_Return;
        }

        public static string Decode(string as_Input)
        {
            string ls_Return = "";
            ls_Return = Decrypt(as_Input, is_DefaultKey);
            return ls_Return;
        }

        public static string Decode(string as_Input, string as_Key)
        {
            string ls_Return = "";
            ls_Return = Decrypt(as_Input, as_Key);
            return ls_Return;
        }

        #endregion

        #region Encryption Algorithms

        // Encrypt a byte array into a byte array using a key and an IV 
        public static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            // Create a MemoryStream to accept the encrypted bytes 

            MemoryStream ms = new MemoryStream();

            // Create a symmetric algorithm. 
            // We are going to use Rijndael because it is strong and
            // available on all platforms. 
            // You can use other algorithms, to do so substitute the
            // next line with something like 
            //      TripleDES alg = TripleDES.Create(); 

            Rijndael alg = Rijndael.Create();

            // Now set the key and the IV. 
            // We need the IV (Initialization Vector) because
            // the algorithm is operating in its default 
            // mode called CBC (Cipher Block Chaining).
            // The IV is XORed with the first block (8 byte) 
            // of the data before it is encrypted, and then each
            // encrypted block is XORed with the 
            // following block of plaintext.
            // This is done to make encryption more secure. 

            // There is also a mode called ECB which does not need an IV,
            // but it is much less secure. 

            alg.Key = Key;
            alg.IV = IV;

            // Create a CryptoStream through which we are going to be
            // pumping our data. 
            // CryptoStreamMode.Write means that we are going to be
            // writing data to the stream and the output will be written
            // in the MemoryStream we have provided. 

            CryptoStream cs = new CryptoStream(ms,
               alg.CreateEncryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the encryption 
            cs.Write(clearData, 0, clearData.Length);

            // Close the crypto stream (or do FlushFinalBlock). 
            // This will tell it that we have done our encryption and
            // there is no more data coming in, 
            // and it is now a good time to apply the padding and
            // finalize the encryption process. 
            cs.Close();

            // Now get the encrypted data from the MemoryStream.
            // Some people make a mistake of using GetBuffer() here,
            // which is not the right way. 
            byte[] encryptedData = ms.ToArray();

            return encryptedData;
        }

        public static string Encrypt(string clearText, string Password)
        {
            // First we need to turn the input string into a byte array. 

            byte[] clearBytes =
              System.Text.Encoding.Unicode.GetBytes(clearText);

            // Then, we need to turn the password into Key and IV 
            // We are using salt to make it harder to guess our key
            // using a dictionary attack - 
            // trying to guess a password by enumerating all possible words. 

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            // Now get the key/IV and do the encryption using the
            // function that accepts byte arrays. 
            // Using PasswordDeriveBytes object we are first getting
            // 32 bytes for the Key 
            // (the default Rijndael key length is 256bit = 32bytes)
            // and then 16 bytes for the IV. 
            // IV should always be the block size, which is by default
            // 16 bytes (128 bit) for Rijndael. 
            // If you are using DES/TripleDES/RC2 the block size is
            // 8 bytes and so should be the IV size. 
            // You can also read KeySize/BlockSize properties off
            // the algorithm to find out the sizes. 

            byte[] encryptedData = Encrypt(clearBytes,
                     pdb.GetBytes(32), pdb.GetBytes(16));

            // Now we need to turn the resulting byte array into a string. 
            // A common mistake would be to use an Encoding class for that.
            //It does not work because not all byte values can be
            // represented by characters. 
            // We are going to be using Base64 encoding that is designed
            //exactly for what we are trying to do. 

            return Convert.ToBase64String(encryptedData);

        }

        // Encrypt bytes into bytes using a password 
        //    Uses Encrypt(byte[], byte[], byte[]) 

        public static byte[] Encrypt(byte[] clearData, string Password)
        {
            // We need to turn the password into Key and IV. 
            // We are using salt to make it harder to guess our key
            // using a dictionary attack - 
            // trying to guess a password by enumerating all possible words. 

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            // Now get the key/IV and do the encryption using the function
            // that accepts byte arrays. 
            // Using PasswordDeriveBytes object we are first getting
            // 32 bytes for the Key 
            // (the default Rijndael key length is 256bit = 32bytes)
            // and then 16 bytes for the IV. 
            // IV should always be the block size, which is by default
            // 16 bytes (128 bit) for Rijndael. 
            // If you are using DES/TripleDES/RC2 the block size is 8
            // bytes and so should be the IV size. 
            // You can also read KeySize/BlockSize properties off the
            // algorithm to find out the sizes. 

            return Encrypt(clearData, pdb.GetBytes(32), pdb.GetBytes(16));

        }


        // Decrypt a byte array into a byte array using a key and an IV 

        public static byte[] Decrypt(byte[] cipherData,
                                    byte[] Key, byte[] IV)
        {
            // Create a MemoryStream that is going to accept the
            // decrypted bytes 

            MemoryStream ms = new MemoryStream();

            // Create a symmetric algorithm. 
            // We are going to use Rijndael because it is strong and
            // available on all platforms. 
            // You can use other algorithms, to do so substitute the next
            // line with something like 
            //     TripleDES alg = TripleDES.Create(); 

            Rijndael alg = Rijndael.Create();

            // Now set the key and the IV. 
            // We need the IV (Initialization Vector) because the algorithm
            // is operating in its default 
            // mode called CBC (Cipher Block Chaining). The IV is XORed with
            // the first block (8 byte) 
            // of the data after it is decrypted, and then each decrypted
            // block is XORed with the previous 
            // cipher block. This is done to make encryption more secure. 
            // There is also a mode called ECB which does not need an IV,
            // but it is much less secure. 

            alg.Key = Key;
            alg.IV = IV;

            // Create a CryptoStream through which we are going to be
            // pumping our data. 
            // CryptoStreamMode.Write means that we are going to be
            // writing data to the stream 
            // and the output will be written in the MemoryStream
            // we have provided. 

            CryptoStream cs = new CryptoStream(ms,
                alg.CreateDecryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the decryption 

            cs.Write(cipherData, 0, cipherData.Length);

            // Close the crypto stream (or do FlushFinalBlock). 
            // This will tell it that we have done our decryption
            // and there is no more data coming in, 
            // and it is now a good time to remove the padding
            // and finalize the decryption process. 

            cs.Close();

            // Now get the decrypted data from the MemoryStream. 
            // Some people make a mistake of using GetBuffer() here,
            // which is not the right way. 

            byte[] decryptedData = ms.ToArray();

            return decryptedData;
        }

        // Decrypt a string into a string using a password 
        //    Uses Decrypt(byte[], byte[], byte[]) 

        public static string Decrypt(string cipherText, string Password)
        {

            try
            {

                // First we need to turn the input string into a byte array. 
                // We presume that Base64 encoding was used 

                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                // Then, we need to turn the password into Key and IV 
                // We are using salt to make it harder to guess our key
                // using a dictionary attack - 
                // trying to guess a password by enumerating all possible words. 

                PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                    new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 
            0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

                // Now get the key/IV and do the decryption using
                // the function that accepts byte arrays. 
                // Using PasswordDeriveBytes object we are first
                // getting 32 bytes for the Key 
                // (the default Rijndael key length is 256bit = 32bytes)
                // and then 16 bytes for the IV. 
                // IV should always be the block size, which is by
                // default 16 bytes (128 bit) for Rijndael. 
                // If you are using DES/TripleDES/RC2 the block size is
                // 8 bytes and so should be the IV size. 
                // You can also read KeySize/BlockSize properties off
                // the algorithm to find out the sizes. 

                byte[] decryptedData = Decrypt(cipherBytes,
                    pdb.GetBytes(32), pdb.GetBytes(16));

                // Now we need to turn the resulting byte array into a string. 
                // A common mistake would be to use an Encoding class for that.
                // It does not work 
                // because not all byte values can be represented by characters. 
                // We are going to be using Base64 encoding that is 
                // designed exactly for what we are trying to do. 

                return System.Text.Encoding.Unicode.GetString(decryptedData);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        // Decrypt bytes into bytes using a password 
        //    Uses Decrypt(byte[], byte[], byte[]) 
        public static byte[] Decrypt(byte[] cipherData, string Password)
        {
            // We need to turn the password into Key and IV. 
            // We are using salt to make it harder to guess our key
            // using a dictionary attack - 
            // trying to guess a password by enumerating all possible words. 

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            // Now get the key/IV and do the Decryption using the 
            //function that accepts byte arrays. 
            // Using PasswordDeriveBytes object we are first getting
            // 32 bytes for the Key 
            // (the default Rijndael key length is 256bit = 32bytes)
            // and then 16 bytes for the IV. 
            // IV should always be the block size, which is by default
            // 16 bytes (128 bit) for Rijndael. 
            // If you are using DES/TripleDES/RC2 the block size is
            // 8 bytes and so should be the IV size. 

            // You can also read KeySize/BlockSize properties off the
            // algorithm to find out the sizes. 
            return Decrypt(cipherData, pdb.GetBytes(32), pdb.GetBytes(16));
        }

        #endregion
    }

}