/*----------------------------------------------------------------
// Author：隐叶
// Copyright © 2023-2030 YinYe. All rights reserved.
//===============================================================
// 功能描述：
//
//----------------------------------------------------------------*/


using System;
using GameMain;
using UnityGameFramework.Runtime;
using GameProto;
using GameProto.proto;

namespace GameBase
{
    public class NetworkAgent:INetworkAgent
    {
        public static NetworkAgent NewInstance()
        {
            return new NetworkAgent();
        }

        public void RouteS2C(PacketS2C packetS2C)
        {
            UInt16 s2dId = (UInt16)packetS2C.Id;
            switch (s2dId)
            {
                case PbS2CId.S2CPong_s2c:
                    HandlePongS2C((S2CPong)packetS2C);
                    break;
                default:
                    break;
            }
            
        }
        
        private void HandlePongS2C(S2CPong s2cPong)
        {
            Log.Info("HandlePongS2C");
        }
    }
}