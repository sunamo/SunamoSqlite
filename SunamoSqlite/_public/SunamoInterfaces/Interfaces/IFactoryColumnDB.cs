namespace SunamoSqlite._public.SunamoInterfaces.Interfaces;

/// <summary>Factory of database table columns.</summary>
public interface IFactoryColumnDB<TColumn, TType>
{
    /// <summary>Creates a column from the single parameters.</summary>
    TColumn CreateInstance(TType typ, string nazev, Signed signed, bool canBeNull, bool mustBeUnique, string referencesTable, string referencesColumn, bool primaryKey);

    /// <summary>Creates a column from the arguments.</summary>
    TColumn CreateInstance(TType typ, MSSloupecDBArgs nazev);
}

/// <summary>Layer over a database that can create its tables.</summary>
public interface ISpecificLayer
{
    /// <summary>Drops the tables and creates them again.</summary>
    void ClearAndCreateTables() { }

    /// <summary>Creates the static tables.</summary>
    void CreateStaticTables() { }
}
