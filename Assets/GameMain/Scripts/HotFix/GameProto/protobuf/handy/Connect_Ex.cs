/*----------------------------------------------------------------
// Author：隐叶
// Copyright © 2023-2030 YinYe. All rights reserved.
//===============================================================
// 功能描述：
//
//----------------------------------------------------------------*/


using GameBase;
using GameFramework.Network;
using GameMain;

namespace GameProto.proto
{
    //===================== c2s (开始)  =========================================
    public partial class C2SPing : PacketC2S
    {
        public override int Id => PbC2SId.C2SPing_c2s; 
        
        public override void Clear()
        {
            this.client_time = 0;
        }
    }
    //===================== c2s (结束)  =========================================
    
    
    //===================== s2c (开始)  =========================================
    
    public partial class S2CPong : PacketS2C
    {
        public override int Id => PbS2CId.S2CPong_s2c; 
        public override void Clear()
        {
            this.client_time = 0;
            this.server_time = 0;
        }
    }
    
    public partial class S2CHello : PacketS2C
    {
        public override int Id => PbS2CId.S2CHello_s2c; 
        public override void Clear()
        {
            this.result = 0;
        }
    }
    //===================== s2c (结束)  =========================================
}