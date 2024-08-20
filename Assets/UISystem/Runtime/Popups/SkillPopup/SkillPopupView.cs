using System;
using System.Collections.Generic;
using UISystem.ReactiveComponents;
using UISystem.Runtime.Components;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem.Runtime.Popups.SkillPopup
{
    [Serializable]
    public class SkillPopupViewModel : UIViewModel
    {
        [field: SerializeField] public ReactiveProperty<int>          SkillLevel             { get; set; } = new();
        [field: SerializeField] public List<SkillInfoWidgetViewModel> SkillInfoComponents    { get; set; } = new();
        public                         Subject<Unit>                  OnLevelUpButtonClicked { get; }      = new();
        public                         Subject<Unit>                  OnCloseButtonClicked   { get; }      = new();
    }

    [Serializable]
    public class SkillPopupView : UIView<SkillPopupViewModel>
    {
        [field: SerializeField] public SkillInfoWidget[]        SkillInfoComponents { get; private set; }
        [field: SerializeField] public SkillDetailWidget        SkillDetailWidget   { get; private set; }
        [field: SerializeField] public TextMeshProReactive<int> SkillLevelText      { get; private set; }
        [field: SerializeField] public Button                   LevelUpButton       { get; private set; }

        protected override void OnSetViewModel(SkillPopupViewModel vModel)
        {
            for (int i = 0; i < SkillInfoComponents.Length; i++)
            {
                SkillInfoComponents[i].SetViewModel(vModel.SkillInfoComponents[i]);
                vModel.SkillInfoComponents[i].OnSkillInfoWidgetClicked
                      .Subscribe(index => SkillDetailWidget.SetViewModel(vModel.SkillInfoComponents[index]))
                      .AddTo(compositeDisposable);
            }

            SkillLevelText.SetViewModel(vModel.SkillLevel);
            LevelUpButton.OnClickAsObservable()
                         .Subscribe(_ => vModel.OnLevelUpButtonClicked.OnNext(Unit.Default))
                         .AddTo(compositeDisposable);

            ViewModel.SkillLevel.Subscribe(level =>
            {
                foreach (var skillInfoComponent in SkillInfoComponents)
                {
                    skillInfoComponent.ViewModel.CurrentLevel.Value = level;
                }
            }).AddTo(compositeDisposable);
            
            // Set first skill as default
            vModel.SkillInfoComponents[0].OnSkillInfoWidgetClicked.OnNext(0);
        }
    }
}