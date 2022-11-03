namespace StringFormatter.Core.Tokens
{
    internal sealed class ReplaceToken : BaseToken
    {
        public string ClearFieldName => TokenText[1..^1];

        public string Replacement = null;

        public override string ToString() => Replacement ?? TokenText; 
    }
}
