namespace UISystem.ReactiveComponents
{
    public class TextMeshProReactiveString : TextMeshProReactive<string>
    {
        protected override string GetTextValue(string value)
        {
            return value;
        }
    }
}