namespace SunamoSqlite.Interfaces;

public interface IStoredProceduresI : IStoredProcedures
    {
        int FindOutID(string tabulka, string nazevSloupce, object hodnotaSloupce);
        int FindOutNumberOfRows(string tabulka);
    }
