using JetBrains.Annotations;
using UISystem.ReactiveComponents;
using UniRx;
using UnityEngine;

namespace UISystem.Runtime.Components
{
    public class SkillDetailWidget : UIView<SkillInfoWidgetViewModel>
    {
        [field: SerializeField] public TextMeshProReactive<string> SkillNameText        { get; private set; }
        [field: SerializeField] public TextMeshProReactive<string> SkillDescriptionText { get; private set; }
        [field: SerializeField] public GameObject                  LockPanel            { get; private set; }
        [field: SerializeField] public TextMeshProReactive<string> LockConditionText    { get; private set; }

        protected override void OnSetViewModel(SkillInfoWidgetViewModel vModel)
        {
            SkillNameText.SetViewModel(vModel.SkillName);
            SkillDescriptionText.SetViewModel(vModel.SkillDescription);
            vModel.IsLocked.Subscribe(isLocked =>
            {
                LockPanel.gameObject.SetActive(isLocked);
            }).AddTo(compositeDisposable);
            
            vModel.CurrentLevel.Subscribe(level =>
            {
                viewModel.IsLocked.Value = level < viewModel.LevelToUnlock.Value;
                LockConditionText
                   .SetText($"You need {vModel.LevelToUnlock.Value - vModel.CurrentLevel.Value} level to unlock this skill.");
            }).AddTo(compositeDisposable);
        }
    }
}