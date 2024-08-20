using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UISystem.Animation
{
    public abstract class BaseViewAnimation : ScriptableObject
    {
        public abstract UniTask AnimateAsync(RectTransform rectTransform, CanvasGroup canvasGroup);
    }
}