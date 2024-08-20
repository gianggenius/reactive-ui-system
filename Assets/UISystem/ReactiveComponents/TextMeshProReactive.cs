using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem.ReactiveComponents
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public abstract class TextMeshProReactive<T> : UIView<IReadOnlyReactiveProperty<T>>
    {
        [field: SerializeField] public TextMeshProUGUI Text       { get; private set; }
        [field: SerializeField] public string          Format     { get; private set; } = "{0}";
        [field: SerializeField] public bool            AutoResize { get; private set; }

        protected abstract string GetTextValue(T value);
        
        public void SetUIModel(T enumValue)
        {
            SetViewModel(new ReactiveProperty<T>(enumValue));
        }

        protected override void OnSetViewModel(IReadOnlyReactiveProperty<T> vModel)
        {
            vModel.Subscribe(SetText)
                   .AddTo(compositeDisposable);
        }

        public void SetText(T text)
        {
            Text.text = string.Format(Format, GetTextValue(text));
            if (AutoResize)
                LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        }

        protected override void Reset()
        {
            base.Reset();
            Text = GetComponent<TextMeshProUGUI>();
        }
    }
}