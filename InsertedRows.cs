namespace SunamoSqlite;
public class InsertedRows
{
    public int quantity = 0;
    public DataTable insertedRows = null;
    public string error = null;

    public InsertedRows()
    {
    }

    public InsertedRows(string Chyba)
    {
        error = Chyba;
    }
}
