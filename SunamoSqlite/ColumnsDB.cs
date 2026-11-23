namespace SunamoSqlite;

public class ColumnsDB : List<SloupecDB>
{
    private bool _signed = false;
    private string _derived = null;

    /// <summary>
    /// A1 are the interfaces and classes the TableRow class derives from
    /// </summary>
    /// <param name="derived"></param>
    /// <param name="signed"></param>
    /// <param name="p"></param>
    public ColumnsDB(string derived, bool signed, params SloupecDB[] p)
    {
        _derived = derived;
        _signed = signed;
        this.AddRange(p);
    }

    public ColumnsDB(bool signed, params SloupecDB[] p)
    {
        _signed = signed;
        this.AddRange(p);
    }

    public ColumnsDB(params SloupecDB[] p)
    {
        this.AddRange(p);
    }

    public object GetTROfColumns()
    {
        return false;
    }

    /// <summary>
    /// Can return null in Command property
    /// A2 skips references to other tables. Useful when tables are tested, dropped and filled again.
    /// </summary>
    public SQLiteCommand GetSqlCreateTable(string table, bool dynamicTables, SQLiteConnection conn)
    {
        string sql = GeneratorSqLite.CreateTable(table, this, dynamicTables, conn);
        SQLiteCommand comm = new SQLiteCommand(sql, conn);
        return comm;
    }
    /// <summary>
    /// Can return null in Command property
    /// 
    /// </summary>
    /// <param name="tableName"></param>
    public SQLiteCommand GetSqlCreateTable(string tableName)
    {
        return GetSqlCreateTable(tableName, false);
    }

    /// <summary>
    /// Can return null in Command property
    /// A2 skips references to other tables. Useful when tables are tested, dropped and filled again.
    /// </summary>
    /// <param name="table"></param>
    /// <param name="dynamicTables"></param>
    public SQLiteCommand GetSqlCreateTable(string table, bool dynamicTables)
    {
        return GetSqlCreateTable(table, dynamicTables, DatabaseLayer.conn);
    }

    //public ColumnsDB(bool signed, string replaceinStoredProceduresI, params SloupecDB[] p)
    //{
    //    this.signed = signed;
    //    this.replaceinStoredProceduresI = replaceinStoredProceduresI;
    //    this.AddRange(p);
    //}
}
