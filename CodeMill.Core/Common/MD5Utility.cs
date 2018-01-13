using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CodeMill.Core.Common
{
    /// <summary>
    /// 提供 MD5 Hash 计算的静态方法
    /// </summary>
    public static class MD5Utility
    {
        private static MD5CryptoServiceProvider csp = new MD5CryptoServiceProvider();

        /// <summary>
        /// 计算字节流的Hash值
        /// </summary>
        /// <param name="inputStream">待计算Hash值的Stream对象</param>
        /// <returns></returns>
        public static byte[] ComputeHash(Stream inputStream)
        {
            return csp.ComputeHash(inputStream);
        }

        /// <summary>
        /// 计算二进制字节数组指定区域的Hash值
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset">数组位置偏移量</param>
        /// <param name="count">字节数量</param>
        /// <returns></returns>
        public static byte[] ComputeHash(byte[] buffer, int offset, int count)
        {
            return csp.ComputeHash(buffer, offset, count);
        }

        /// <summary>
        /// 计算二进制字节数组的Hash值
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(byte[] buffer)
        {
            return csp.ComputeHash(buffer);
        }

        /// <summary>
        /// 计算输入字符串的Hash值，并转换为不带符号的HexString
        /// </summary>
        /// <param name="input">待计算的字符串</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string HashString(string input, Encoding encoding)
        {
            var hash = csp.ComputeHash(encoding.GetBytes(input));
            return BitConverter.ToString(hash).Replace("-", "");
        }

        /// <summary>
        /// 用UTF8编码计算输入字符串的Hash值，并转换为不带符号的HexString
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string HashString(string input)
        {
            return HashString(input, Encoding.UTF8);
        }

        /// <summary>
        /// 用自定的编码计算字符串的Hash
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="charset">编码字符集</param>
        /// <returns>16进制字符串</returns>
        public static string HashString(string input, string charset)
        {
            return HashString(input, Encoding.GetEncoding(charset));
        }
    }
}
