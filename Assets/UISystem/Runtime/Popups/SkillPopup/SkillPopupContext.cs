using System;
using Cysharp.Threading.Tasks;
using UISystem.Popup;
using UniRx;
using UnityEngine;

namespace UISystem.Runtime.Popups.SkillPopup
{
    [Serializable]
    public class SkillPopupModel : UIModel
    {
        [field: SerializeField]
        public ReactiveProperty<int> SkillLevel { get; private set; } = new ReactiveProperty<int>();
    }

    public class SkillPopupContext : PopupContext<SkillPopupModel, SkillPopupView, SkillPopupViewModel>
    {
        protected override void OnShowing(SkillPopupModel model)
        {
            Model.SkillLevel.Subscribe(level => ViewModel.SkillLevel.Value = level).AddTo(compositeDisposable);
            ViewModel.OnLevelUpButtonClicked.Subscribe(OnLevelUpButtonClicked).AddTo(compositeDisposable);
            ViewModel.OnCloseButtonClicked.Subscribe(OnCloseButtonClicked).AddTo(compositeDisposable);
            Observable.EveryUpdate()
                      .Where(_ => Input.GetKeyDown(KeyCode.Escape))
                      .TakeWhile(_ => gameObject.activeSelf)
                      .Subscribe(_ => ViewModel.OnCloseButtonClicked.OnNext(Unit.Default))
                      .AddTo(compositeDisposable);
        }

        private void OnLevelUpButtonClicked(Unit _)
        {
            ViewModel.SkillLevel.Value++;
        }

        private void OnCloseButtonClicked(Unit _)
        {
            PopupBaseLayer.Main.BackAsync().Forget();
        }
    }
}