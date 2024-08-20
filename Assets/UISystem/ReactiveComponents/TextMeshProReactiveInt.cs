using System.Globalization;
using UnityEngine;

namespace UISystem.ReactiveComponents
{
    public class TextMeshProReactiveInt : TextMeshProReactiveNumber<int>
    {
        protected override string GetTextValue(int value)
        {
            return useRange ? Mathf.Clamp(value, minValue, maxValue).ToString() : value.ToString();
        }
    }
}