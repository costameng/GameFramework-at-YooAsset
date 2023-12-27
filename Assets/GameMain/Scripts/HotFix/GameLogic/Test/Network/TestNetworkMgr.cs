/*----------------------------------------------------------------
// Author：隐叶
// Copyright © 2023-2030 YinYe. All rights reserved.
//===============================================================
// 功能描述：
//
//----------------------------------------------------------------*/

using System;
using GameBase;
using GameMain;
using GameProto;
using GameProto.proto;
using UnityEngine;

namespace GameLogic.Test
{
    public class TestNetworkMgr:Singleton<TestNetworkMgr>
    {
        // public void SendLogin()
        // {
        //     role_login_c2s loginC2S = PacketPool.AcqC2SPacket<role_login_c2s>();
        //     DecadeRoleLoginC2S(loginC2S);
        //     NetworkMgr.Instance.Send(loginC2S);
        // }
        // private void DecadeRoleLoginC2S(role_login_c2s loginC2S )
        // {
        //     loginC2S.uid = 10001;
        //     loginC2S.uname = "jelly fish";
        //     loginC2S.plat = "tx";
        //     loginC2S.svrId = 1;
        //     loginC2S.game_id = "StartCraftWar";
        //     loginC2S.machine_info = null;
        // }
        //
        // public void SendCreate()
        // {
        //     create_role_c2s createC2S = PacketPool.AcqC2SPacket<create_role_c2s>();
        //     DecadeRoleLoginC2S(createC2S);
        //     NetworkMgr.Instance.Send(createC2S);
        // }
        //
        // private void DecadeRoleLoginC2S(create_role_c2s createRoleC2S )
        // {
        //     createRoleC2S.name = "jelly fish";
        //     createRoleC2S.gender = 1;
        // }
        
        public void SendPing()
        {
            C2SPing pingC2S = PacketPool.AcqC2SPacket<C2SPing>();
            DecadePingC2S(pingC2S);
            NetworkMgr.Instance.Send(pingC2S);
        }
        
        private void DecadePingC2S(C2SPing pingC2S )
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);;
            pingC2S.client_time = (uint)ts.TotalSeconds;
        }
        
        public void RouteS2C(PacketS2C packetS2C)
        {
            throw new System.NotImplementedException();
        }
    }
}