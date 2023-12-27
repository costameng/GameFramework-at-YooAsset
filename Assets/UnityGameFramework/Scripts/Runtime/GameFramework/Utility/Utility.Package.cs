using System;
using System.Collections.Generic;

namespace GameFramework
{
    /// <summary>
    /// 实用函数集。
    /// </summary>
    public static partial class Utility
    {
        /// <summary>
        /// 数据包工具类
        /// </summary>
        public static class Package
        {
            // 数据包：数据长度+协议ID+消息结构体Body
            //        byte[2] byte[2] byte[len-4]
    
            public static bool CheckPackage(byte[] message)
            {
                //最小长度
                if (message.Length < 4)
                {
                    return false;
                }
                //合法长度
                int packageLen = GetPackageLength(message);
                if (packageLen > message.Length)
                {
                    return false;
                }
                return true;
            }
    
            public static int GetPackageLength(byte[] message)
            {
                byte[] buffer = new byte[2];
                Array.Copy(message, 0, buffer, 0, buffer.Length);
                Array.Reverse(buffer); //转成大端字节序
                int packageLen = BitConverter.ToInt16(buffer, 0);
                return packageLen;
            }
    
            public static int GetProtoID(byte[] message)
            {
                byte[] buffer = new byte[2];
                Array.Copy(message, 2, buffer, 0, buffer.Length);
                Array.Reverse(buffer);
                int protoID = BitConverter.ToInt16(buffer, 0);
                return protoID;
            }
    
            public static byte[] GetProtoBody(byte[] message)
            {
                int packageLen = GetPackageLength(message);
                byte[] buffer = new byte[packageLen - 4];
                Array.Copy(message, 4, buffer, 0, buffer.Length);
                return buffer;
            }
    
            public static byte[] SetPackage(int protoID, byte[] body)
            {
                int totalLen = 4 + body.Length;
                byte[] lenBuffer = BitConverter.GetBytes(Convert.ToInt16(totalLen));
                byte[] protoBuffer = BitConverter.GetBytes(Convert.ToInt16(protoID));
                Array.Reverse(lenBuffer);
                Array.Reverse(protoBuffer);
                List<byte> package = new List<byte>();
                package.AddRange(lenBuffer);
                package.AddRange(protoBuffer);
                if (body.Length > 0)
                {
                    package.AddRange(body);
                }
                return package.ToArray();
            }
        }
    }
}
