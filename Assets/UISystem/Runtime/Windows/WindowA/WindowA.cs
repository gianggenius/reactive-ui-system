using Cysharp.Threading.Tasks;
using UISystem.Window;
using UISystem.Windows.WindowB;
using UnityEngine;
using UniRx;

namespace UISystem.Windows.WindowA
{
    public class WindowAModel : UIModel
    {
    }

    public class WindowA : WindowContext<WindowAModel, WindowAView, WindowAViewModel>
    {
        protected override void OnShowing(WindowAModel model)
        {
            ViewModel.OnShowWindowBButtonClicked.Subscribe(OnWindowBButtonClicked).AddTo(compositeDisposable);
            ViewModel.OnCloseButtonClicked.Subscribe(OnCloseButtonClicked).AddTo(compositeDisposable);
        }

        private void OnWindowBButtonClicked(Unit _)
        {
            WindowBaseLayer.Main.ShowAsync<WindowB.WindowB, WindowBModel>(new WindowBModel()).Forget();
        }
        private void OnCloseButtonClicked(Unit _)
        {
            WindowBaseLayer.Main.BackAsync().Forget();
        }
    }
}