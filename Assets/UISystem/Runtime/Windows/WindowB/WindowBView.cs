using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem.Windows.WindowA
{
    [Serializable]
    public class WindowBViewModel : UIViewModel
    {
        public Subject<Unit> OnOpenPopupButtonClicked = new();
        public Subject<Unit> OnCloseButtonClicked     = new();
        public Subject<Unit> OnCloseAllButtonClicked  = new();
    }

    public class WindowBView : UIView<WindowBViewModel>
    {
        [field: SerializeField] public Button OpenPopupButton { get; private set; }
        [field: SerializeField] public Button CloseButton     { get; private set; }
        [field: SerializeField] public Button CloseAllButton  { get; private set; }

        protected override void OnSetViewModel(WindowBViewModel vModel)
        {
            OpenPopupButton.OnClickAsObservable()
                           .Subscribe(_ => vModel.OnOpenPopupButtonClicked.OnNext(Unit.Default))
                           .AddTo(compositeDisposable);
            CloseButton.OnClickAsObservable()
                       .Subscribe(_ => viewModel.OnCloseButtonClicked.OnNext(Unit.Default))
                       .AddTo(compositeDisposable);
            CloseAllButton.OnClickAsObservable()
                          .Subscribe(_ => viewModel.OnCloseAllButtonClicked.OnNext(Unit.Default))
                          .AddTo(compositeDisposable);
        }
    }
}