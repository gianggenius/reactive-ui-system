using System;
using UISystem.ReactiveComponents;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UISystem.Runtime.Components
{
    [Serializable]
    public class SkillInfoWidgetViewModel : UIViewModel
    {
        [field: SerializeField] public int                      Index                    { get; private set; }
        [field: SerializeField] public ReactiveProperty<string> SkillName                { get; private set; } = new();
        [field: SerializeField] public ReactiveProperty<string> SkillDescription         { get; private set; } = new();
        [field: SerializeField] public ReactiveProperty<bool>   IsLocked                 { get; private set; } = new();
        [field: SerializeField] public ReactiveProperty<int>    LevelToUnlock            { get; private set; } = new();
        [field: SerializeField] public ReactiveProperty<int>    CurrentLevel             { get; private set; } = new();
        public                         Subject<int>             OnSkillInfoWidgetClicked { get; }              = new();
    }

    [Serializable]
    public class SkillInfoWidget : UIView<SkillInfoWidgetViewModel>
    {
        [field: SerializeField] public TextMeshProReactive<string> SkillNameText        { get; private set; }
        [field: SerializeField] public TextMeshProReactive<string> SkillDescriptionText { get; private set; }
        [field: SerializeField] public TextMeshProReactive<string> LockText             { get; private set; }

        protected override void OnSetViewModel(SkillInfoWidgetViewModel vModel)
        {
            SkillNameText.SetViewModel(vModel.SkillName);
            SkillDescriptionText.SetViewModel(vModel.SkillDescription);
            vModel.IsLocked.Subscribe(isLocked =>
            {
                LockText.SetText(isLocked ? "Locked" : "Unlocked");
                LockText.Text.color = isLocked ? Color.red : Color.green;
            }).AddTo(compositeDisposable);
            vModel.CurrentLevel
                  .Subscribe(level => { viewModel.IsLocked.Value = level < viewModel.LevelToUnlock.Value; })
                  .AddTo(compositeDisposable);
            this.OnPointerClickAsObservable()
                .Subscribe(_ => vModel.OnSkillInfoWidgetClicked.OnNext(viewModel.Index))
                .AddTo(compositeDisposable);
        }
    }
}