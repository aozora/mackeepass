using System;
using System.Runtime.InteropServices;
using KeePassLib.Utility;
using System.Security;



namespace MacKeePass
{
   public static class StringExtensions
   {
      public static byte[] ToUtf8 (this string text)
      {
         //Debug.Assert(sizeof(char) == 2);
         SecureString m_secString = new SecureString();
         for (int index = 0; index < text.Length; index++)
         {
            m_secString.InsertAt(index, text[index]);
         }


         if (m_secString != null) {
            char[] vChars = new char[m_secString.Length];
            IntPtr p = Marshal.SecureStringToGlobalAllocUnicode (m_secString);
            for (int i = 0; i < m_secString.Length; ++i)
               vChars [i] = (char)Marshal.ReadInt16 (p, i * 2);
            Marshal.ZeroFreeGlobalAllocUnicode (p);

            byte[] pb = StrUtil.Utf8.GetBytes (vChars);
            Array.Clear (vChars, 0, vChars.Length);

            return pb;
         }
         else {
            return StrUtil.Utf8.GetBytes (string.Empty);
         }
      }

   }
}

