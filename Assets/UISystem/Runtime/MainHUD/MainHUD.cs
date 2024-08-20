using System;
using Cysharp.Threading.Tasks;
using UISystem.HUD;
using UISystem.Window;
using UISystem.Windows.WindowA;
using UnityEngine;
using UniRx;

namespace UISystem
{
    public class MainHUDModel : UIModel
    {
        public string PlayerName { get; set; }
        public int    PlayerLevel { get; set; }
        public int    PlayerCombatPower { get; set; }
    }

    public class MainHUD : HUDContext<MainHUDModel, MainHUDView, MainHUDViewModel>
    {
        
        private void Update()
        {
            CalculateFPS();
        }
        
        protected override void OnShowing(MainHUDModel model)
        {
            SetPlayerInfoModel(model);
            ViewModel.OnShowWindowAButtonClicked.Subscribe(OnShowWindowAButtonClicked).AddTo(this);
        }

        private void SetPlayerInfoModel(MainHUDModel model)
        {
            ViewModel.PlayerInfo.PlayerName.Value        = model.PlayerName;
            ViewModel.PlayerInfo.PlayerLevel.Value       = model.PlayerLevel;
            ViewModel.PlayerInfo.PlayerCombatPower.Value = model.PlayerCombatPower;
        }

        private void CalculateFPS()
        {
            ViewModel.FPS.Value = (int)(1f / Time.deltaTime);
        }
        
        private void OnShowWindowAButtonClicked(Unit _)
        {
            WindowBaseLayer.Main.ShowAsync<WindowA, WindowAModel>(new WindowAModel()).Forget();
        }
        
    }
}