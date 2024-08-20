using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UISystem
{
    public class TestUI : MonoBehaviour
    {
        private void Start()
        {
            HUDBaseLayer.Main.ShowAsync<MainHUD, MainHUDModel>(new MainHUDModel()
            {
                PlayerName = "GiGi",
                PlayerLevel = 99,
                PlayerCombatPower = 9999
            }).Forget();
        }
    }
}