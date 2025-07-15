using CodeBind;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public partial class LoginForm : AHotUIForm
    {
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitBind(gameObject.GetComponent<CSCodeBindMono>());
            sureButton.onClick.AddListener(OnSureButtonClick);
        }

        private void OnSureButtonClick()
        {
            Log.Error(99999);
        }
    }
}