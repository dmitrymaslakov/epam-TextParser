using TextParser.DAL.Interfaces;

namespace TextParser.DAL.Entities
{
    public class Symbol : ISymbol
    {
        public char Value { get; }
        public bool IsUpper { get; }
        public bool IsLower { get; }


        public Symbol(char value)
        {
            Value = value;

            IsUpper = char.IsUpper(Value);

            IsLower = char.IsLower(Value);
        }

        public void ToLower()
        {
            char.ToLower(Value);
        }

        public void ToUpper()
        {
            char.ToUpper(Value);
        }
    }
}
