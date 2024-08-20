using System;
using UnityEngine;

namespace UISystem.HUD
{
    public interface IHUDContext
    {
        
    }
    
    public abstract class HUDContext<TModel, TView, TViewModel> : BaseUIContext<TModel, TView, TViewModel>,
                                                                  IHUDContext
        where TModel : UIModel
        where TView : UIView<TViewModel>
        where TViewModel : UIViewModel
    {

    }
}