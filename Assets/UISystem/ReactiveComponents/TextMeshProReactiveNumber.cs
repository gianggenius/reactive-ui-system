using MoreMountains.Tools;
using UnityEngine;

namespace UISystem.ReactiveComponents
{
    public abstract class TextMeshProReactiveNumber<T> : TextMeshProReactive<T>
    {
        [SerializeField] protected bool useRange = false;

        [MMCondition("useRange", true)] 
        [SerializeField] protected T minValue;

        [MMCondition("useRange", true)] 
        [SerializeField] protected T maxValue;
    }
}