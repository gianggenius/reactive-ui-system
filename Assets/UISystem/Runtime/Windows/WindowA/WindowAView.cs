using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem.Windows.WindowA
{
    [Serializable]
    public class WindowAViewModel: UIViewModel
    {
        public Subject<Unit> OnShowWindowBButtonClicked = new();
        public Subject<Unit> OnCloseButtonClicked       = new();
    }

    public class WindowAView : UIView<WindowAViewModel>
    {
        [field: SerializeField] public Button ShowWindowBButton { get; private set; }
        [field: SerializeField] public Button CloseButton       { get; private set; }

        protected override void OnSetViewModel(WindowAViewModel vModel)
        {
            ShowWindowBButton.OnClickAsObservable()
                             .Subscribe(_ => viewModel.OnShowWindowBButtonClicked.OnNext(Unit.Default))
                             .AddTo(compositeDisposable);
            
            CloseButton.OnClickAsObservable()
                       .Subscribe(_ => viewModel.OnCloseButtonClicked.OnNext(Unit.Default))
                       .AddTo(compositeDisposable);
            
        }
    }
}