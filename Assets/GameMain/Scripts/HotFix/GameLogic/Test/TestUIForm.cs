using GameBase;
using GameFramework;
using UnityGameFramework.Runtime;

namespace GameLogic.Test
{
    public class TestUIForm : UIFormLogic
    {
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        
            Log.Info("TestUIForm Open");
        }
        
        public void OnConnectButtonClick()
        {
            Log.Info("Test Connect");
            // TestNetworkMgr.Instance.SendLogin();
            TestNetwork.Instance.OnEnter();
        }
        
        public void OnDisconnetButtonClick()
        {
            Log.Info("Test Disconnect");
            // TestNetworkMgr.Instance.SendLogin();
            // TestNetwork.Instance.OnEnter();
        }
        
        public void OnHeartBeatButtonClick()
        {
            Log.Info("Test HeartBeat");
            // TestNetworkMgr.Instance.SendLogin();
            NetworkMgr.Instance.SendHeartBeat();
        }
        
    }
}