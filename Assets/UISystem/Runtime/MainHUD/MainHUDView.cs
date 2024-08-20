using System;
using UISystem.ReactiveComponents;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    [Serializable]
    public class MainHUDViewModel : UIViewModel
    {
        [field: SerializeField] public ReactiveProperty<int> FPS                        { get; set; } = new();
        [field: SerializeField] public PlayerInfoViewModel   PlayerInfo                 { get; set; } = new();
        public                         Subject<Unit>         OnShowWindowAButtonClicked { get; }      = new();
    }

    [Serializable]
    public class MainHUDView : UIView<MainHUDViewModel>
    {
        [field: SerializeField] public PlayerInfoView         PlayerInfoView    { get; private set; }
        [field: SerializeField] public TextMeshProReactiveInt FPSText           { get; private set; }
        [field: SerializeField] public Button                 ShowWindowAButton { get; private set; }

        protected override void OnSetViewModel(MainHUDViewModel vModel)
        {
            FPSText.SetViewModel(vModel.FPS);
            PlayerInfoView.SetViewModel(vModel.PlayerInfo);
            ShowWindowAButton.OnClickAsObservable()
                             .Subscribe(_ => vModel.OnShowWindowAButtonClicked.OnNext(Unit.Default))
                             .AddTo(compositeDisposable);
        }
    }
}