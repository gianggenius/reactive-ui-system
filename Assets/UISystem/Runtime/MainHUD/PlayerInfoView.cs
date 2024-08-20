using System;
using TMPro;
using UISystem.ReactiveComponents;
using UniRx;
using UnityEngine;

namespace UISystem
{
    [Serializable]
    public class PlayerInfoViewModel
    {
        [field: SerializeField] public ReactiveProperty<string> PlayerName        { get; private set; } = new();
        [field: SerializeField] public ReactiveProperty<int>    PlayerLevel       { get; private set; } = new();
        [field: SerializeField] public ReactiveProperty<int>    PlayerCombatPower { get; private set; } = new();
    }

    public class PlayerInfoView : UIView<PlayerInfoViewModel>
    {
        [field: SerializeField] public TextMeshProReactiveString PlayerNameText        { get; private set; }
        [field: SerializeField] public TextMeshProReactiveInt    PlayerLevelText       { get; private set; }
        [field: SerializeField] public TextMeshProReactiveInt    PlayerCombatPowerText { get; private set; }
        
        protected override void OnSetViewModel(PlayerInfoViewModel vModel)
        {
            PlayerNameText.SetViewModel(vModel.PlayerName);
            PlayerLevelText.SetViewModel(vModel.PlayerLevel);
            PlayerCombatPowerText.SetViewModel(vModel.PlayerCombatPower);
        }
    }
}