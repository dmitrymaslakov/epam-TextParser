namespace TextParser.DAL.Interfaces
{
    public interface ISymbol
    {
        char Value { get; }
        bool IsUpper { get; }
        bool IsLower { get; }

        void ToUpper();
        void ToLower();

    }
}
