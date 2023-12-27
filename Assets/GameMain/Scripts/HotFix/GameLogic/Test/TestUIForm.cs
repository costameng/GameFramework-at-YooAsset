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

        public void OnLoginButtonClick()
        {
            Log.Info("Test Login");
            // TestNetworkMgr.Instance.SendLogin();
            NetworkMgr.Instance.SendHeartBeat();
        }
    }
}