/*----------------------------------------------------------------
// Author：隐叶
// Copyright © 2023-2030 YinYe. All rights reserved.
//===============================================================
// 功能描述：
//
//----------------------------------------------------------------*/


using GameBase;
using GameMain;
using GameProto;

namespace GameLogic.Test
{
    public class TestNetwork : Singleton<TestNetwork>
    {
        private bool _isLogin = false;
        
        public void OnEnter()
        {
            string ip = "127.0.0.1";
            int port = 12345;
            NetworkMgr.Instance.InitChannel(ip,port,TestNetworkAgent.NewInstance());
        }

        public void OnUpdate()
        {
            if (NetworkMgr.Instance.IsReady && !_isLogin)
            {
                TestNetworkMgr.Instance.SendLogin();
                _isLogin = true;
            }
        }

    }
}