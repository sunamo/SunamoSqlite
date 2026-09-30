namespace SunamoSqlite.Interfaces;

    public interface IStoredProcedures
    {
        SQLiteCommand InsertRowTypeEnumIfNotExists(string tabulka, string nazev);
        SQLiteCommand DeleteTableIfExists(string nazevTabulky);
    }
