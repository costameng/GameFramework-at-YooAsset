/*----------------------------------------------------------------
// Author：隐叶
// Copyright © 2023-2030 YinYe. All rights reserved.
//===============================================================
// 功能描述：
//
//----------------------------------------------------------------*/


using System;
using GameProto.proto;

namespace GameProto
{
    public class PbC2SId
    {
        //Connect
        public const UInt16 C2SPing_c2s = 104;

    }
    public class PbS2CId
    {
        //Connect
        public const UInt16 S2CPong_s2c = 105;
        public const UInt16 S2CHello_s2c = 103;

        public static Type GetTypeById(UInt16 s2cId)
        {
            Type protoType = s2cId switch
            {
                S2CPong_s2c => typeof(S2CPong),
                S2CHello_s2c => typeof(S2CHello),
                _ => null,
            };
            return protoType;
        }
    }
    
    
}