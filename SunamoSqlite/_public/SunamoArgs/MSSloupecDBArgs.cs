namespace SunamoSqlite._public.SunamoArgs;

public class MSSloupecDBArgs
{
    public string nazev = null!;
    public bool canBeNull;
    public bool identityIncrementBy1;
    public bool mustBeUnique;
    public bool primaryKey;
    public string? referencesColumn;
    public string? referencesTable;
    public Signed signed;
}
