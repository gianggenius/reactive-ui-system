using Cysharp.Threading.Tasks;
using UISystem.Popup;
using UISystem.Runtime.Popups.SkillPopup;
using UISystem.Window;
using UISystem.Windows.WindowA;
using UniRx;
using UnityEngine;

namespace UISystem.Windows.WindowB
{
    public class WindowBModel : UIModel
    {
    }

    public class WindowB : WindowContext<WindowBModel, WindowBView, WindowBViewModel>
    {
        [SerializeField] private SkillPopupModel _skillPopupModel;

        protected override void OnShowing(WindowBModel model)
        {
            ViewModel.OnOpenPopupButtonClicked.Subscribe(OpenPopup).AddTo(compositeDisposable);
            ViewModel.OnCloseButtonClicked.Subscribe(OnCloseButtonClicked).AddTo(compositeDisposable);
            ViewModel.OnCloseAllButtonClicked.Subscribe(OnCloseAllButtonClicked).AddTo(compositeDisposable);
        }

        private void OnCloseButtonClicked(Unit _)
        {
            WindowBaseLayer.Main.BackAsync().Forget();
        }

        private void OpenPopup(Unit _)
        {
            PopupBaseLayer.Main.ShowAsync<SkillPopupContext, SkillPopupModel>(_skillPopupModel).Forget();
        }

        private void OnCloseAllButtonClicked(Unit _)
        {
            WindowBaseLayer.Main.BackAsync(true).Forget();
        }
    }
}