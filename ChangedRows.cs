namespace SunamoSqlite;
public class ChangedRows
{
    public int quantity = 0;
    public DataTable beforeChanges = null;
    public DataTable afterChanges = null;
    public string error = null;

    public ChangedRows()
    {
    }

    public ChangedRows(string Chyba)
    {
        error = Chyba;
    }
}
