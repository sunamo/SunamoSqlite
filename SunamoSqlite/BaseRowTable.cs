namespace SunamoSqlite;

/// <summary>
/// Base of the classes that represent one table row.
/// Contains helpers that read values of the row and a general implementation of the table operations.
/// The general implementation works with the public fields of the class (in declaration order, field name = column name)
/// and with the TableName property of the derived class. Classes whose fields do not match the columns one to one override it.
/// </summary>
public class BaseRowTable
{
    /// <summary>
    /// Values of the current row.
    /// </summary>
    protected object[] o = null;

    protected string GetString(int p) { return TableRowParse.GetString(o, p); }
    protected string GetString(object[] o, int p) { return TableRowParse.GetString(o, p); }

    /// <summary>
    /// DBNull gives -1.
    /// </summary>
    protected int GetInt(int p) { return TableRowParse.GetInt(o, p); }
    protected int GetInt(object[] o, int p) { return TableRowParse.GetInt(o, p); }

    /// <summary>
    /// DBNull gives -1.
    /// </summary>
    protected float GetFloat(int p) { return TableRowParse.GetFloat(o, p); }
    protected float GetFloat(object[] o, int p) { return TableRowParse.GetFloat(o, p); }

    /// <summary>
    /// DBNull gives -1.
    /// </summary>
    protected long GetLong(int p) { return TableRowParse.GetLong(o, p); }
    protected long GetLong(object[] o, int p) { return TableRowParse.GetLong(o, p); }

    /// <summary>
    /// Returns a logical value (0/1, true/false).
    /// </summary>
    protected bool GetBool(int p) { return TableRowParse.GetBool(o, p); }
    protected bool GetBool(object[] o, int p) { return TableRowParse.GetBool(o, p); }

    /// <summary>
    /// Returns Ano/Ne (yes/no).
    /// </summary>
    protected string GetBoolS(int p) { return TableRowParse.GetBoolS(o, p); }
    protected string GetBoolS(object[] o, int p) { return TableRowParse.GetBoolS(o, p); }

    /// <summary>
    /// DBNull gives DateTime.MinValue.
    /// </summary>
    protected DateTime GetDateTime(int p) { return TableRowParse.GetDateTime(o, p); }
    protected DateTime GetDateTime(object[] o, int p) { return TableRowParse.GetDateTime(o, p); }

    /// <summary>
    /// Can return null when the value is DBNull.
    /// </summary>
    protected string GetDateTimeS(int p) { return TableRowParse.GetDateTimeS(o, p); }
    protected string GetDateTimeS(object[] o, int p) { return TableRowParse.GetDateTimeS(o, p); }

    protected byte[] GetImage(int dex) { return TableRowParse.GetImage(o, dex); }
    protected byte[] GetImage(object[] o, int dex) { return TableRowParse.GetImage(o, dex); }

    protected decimal GetDecimal(int p) { return TableRowParse.GetDecimal(o, p); }
    protected decimal GetDecimal(object[] o, int p) { return TableRowParse.GetDecimal(o, p); }

    protected double GetDouble(int p) { return TableRowParse.GetDouble(o, p); }
    protected double GetDouble(object[] o, int p) { return TableRowParse.GetDouble(o, p); }

    protected short GetShort(int p) { return TableRowParse.GetShort(o, p); }
    protected short GetShort(object[] o, int p) { return TableRowParse.GetShort(o, p); }

    protected byte GetByte(int p) { return TableRowParse.GetByte(o, p); }
    protected byte GetByte(object[] o, int p) { return TableRowParse.GetByte(o, p); }

    protected object GetObject(int p) { return o[p]; }

    protected Guid GetGuid(int p) { return TableRowParse.GetGuid(o, p); }
    protected Guid GetGuid(object[] o, int p) { return TableRowParse.GetGuid(o, p); }

    private System.Reflection.FieldInfo[] Columns()
    {
        return GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
    }

    private string TableNameOfRow()
    {
        System.Reflection.PropertyInfo pi = GetType().GetProperty("TableName");
        if (pi == null)
        {
            throw new NotSupportedException("Class " + GetType().Name + " has no TableName property.");
        }
        return pi.GetValue(this).ToString();
    }

    private object FieldValue(System.Reflection.FieldInfo fi)
    {
        object v = fi.GetValue(this);
        if (v is DateTime dt)
        {
            return dt;
        }
        return v;
    }

    private string WhereById()
    {
        System.Reflection.FieldInfo id = GetType().GetField("ID");
        if (id == null)
        {
            throw new NotSupportedException("Class " + GetType().Name + " has no ID field.");
        }
        return " WHERE ID = " + StoredProcedures.ci.ReplaceValueOnlyOne(id.GetValue(this));
    }

    private string WhereByColumns(List<string> nazvySloupcu, List<object> hodnotaSloupcu)
    {
        List<string> casti = new List<string>();
        for (int i = 0; i < nazvySloupcu.Count; i++)
        {
            casti.Add(nazvySloupcu[i] + " = " + StoredProcedures.ci.ReplaceValueOnlyOne(hodnotaSloupcu[i]));
        }
        return " WHERE " + string.Join(" AND ", casti);
    }

    private void FillFromRow(DataRow dr)
    {
        object[] hodnoty = dr.ItemArray;
        System.Reflection.FieldInfo[] sloupce = Columns();
        for (int i = 0; i < sloupce.Length && i < hodnoty.Length; i++)
        {
            Type t = sloupce[i].FieldType;
            object v;
            if (t == typeof(int)) v = TableRowParse.GetInt(hodnoty, i);
            else if (t == typeof(bool)) v = TableRowParse.GetBool(hodnoty, i);
            else if (t == typeof(DateTime)) v = TableRowParse.GetDateTime(hodnoty, i);
            else if (t == typeof(string)) v = TableRowParse.GetString(hodnoty, i);
            else if (t == typeof(double)) v = TableRowParse.GetDouble(hodnoty, i);
            else if (t == typeof(byte[])) v = TableRowParse.GetImage(hodnoty, i);
            else continue;
            sloupce[i].SetValue(this, v);
        }
    }

    /// <summary>
    /// Inserts a row into the table. The ID field is inserted as NULL (automatic numbering) when it is negative or zero.
    /// Returns the current count of rows in the table.
    /// </summary>
    public virtual int InsertToTable()
    {
        System.Reflection.FieldInfo[] sloupce = Columns();
        List<string> nazvy = new List<string>();
        List<object> hodnoty = new List<object>();
        foreach (System.Reflection.FieldInfo fi in sloupce)
        {
            object v = FieldValue(fi);
            if (fi.Name == "ID" && v is int id && id <= 0)
            {
                v = null;
            }
            nazvy.Add(fi.Name);
            hodnoty.Add(v);
        }
        string tabulka = TableNameOfRow();
        StoredProceduresI.ci.ExecuteNonQuery(string.Format("INSERT INTO {0} {1} VALUES {2}", tabulka, StoredProcedures.ci.GetColumns(nazvy), StoredProcedures.ci.GetValues(hodnoty.ToArray())));
        return StoredProceduresI.ci.FindOutNumberOfRows(tabulka);
    }

    /// <summary>
    /// Inserts a row into the table and returns the count of rows, or the error.
    /// </summary>
    public virtual InsertedRows InsertToTableInsertedRows()
    {
        InsertedRows vr = new InsertedRows();
        try
        {
            vr.quantity = InsertToTable();
        }
        catch (Exception ex)
        {
            vr.error = ex.Message;
        }
        return vr;
    }

    /// <summary>
    /// Updates all columns of the row with the ID of this object.
    /// </summary>
    public virtual ChangedRows UpdateInTable()
    {
        List<string> sets = new List<string>();
        foreach (System.Reflection.FieldInfo fi in Columns())
        {
            sets.Add(fi.Name + " = " + StoredProcedures.ci.ReplaceValueOnlyOne(FieldValue(fi)));
        }
        return RunUpdate(string.Join(", ", sets), WhereById());
    }

    /// <summary>
    /// Updates the rows that have the values in the columns with the values of this object.
    /// </summary>
    public virtual ChangedRows UpdateInTable(List<string> nazvySloupcu, List<object> hodnotaSloupcu)
    {
        List<string> sets = new List<string>();
        foreach (System.Reflection.FieldInfo fi in Columns())
        {
            sets.Add(fi.Name + " = " + StoredProcedures.ci.ReplaceValueOnlyOne(FieldValue(fi)));
        }
        return RunUpdate(string.Join(", ", sets), WhereByColumns(nazvySloupcu, hodnotaSloupcu));
    }

    /// <summary>
    /// Updates the rows that have the value in the column with the values of this object.
    /// </summary>
    public virtual ChangedRows UpdateInTable(string nazevSloupce, object hodnotaSloupce)
    {
        return UpdateInTable(new List<string> { nazevSloupce }, new List<object> { hodnotaSloupce });
    }

    private ChangedRows RunUpdate(string sets, string where)
    {
        ChangedRows vr = new ChangedRows();
        try
        {
            vr.quantity = StoredProceduresI.ci.ExecuteNonQuery(string.Format("UPDATE {0} SET {1}{2}", TableNameOfRow(), sets, where));
        }
        catch (Exception ex)
        {
            vr.error = ex.Message;
        }
        return vr;
    }

    /// <summary>
    /// Checks whether the update of the row with the ID of this object would run (changes nothing).
    /// </summary>
    public virtual ChangedRows UpdateInTableTest()
    {
        return TestRows(WhereById());
    }

    /// <summary>
    /// Checks whether the update would run (changes nothing).
    /// </summary>
    public virtual ChangedRows UpdateInTableTest(List<string> nazvySloupcu, List<object> hodnotaSloupcu)
    {
        return TestRows(WhereByColumns(nazvySloupcu, hodnotaSloupcu));
    }

    /// <summary>
    /// Checks whether the update would run (changes nothing).
    /// </summary>
    public virtual ChangedRows UpdateInTableTest(string nazevSloupce, object hodnotaSloupce)
    {
        return UpdateInTableTest(new List<string> { nazevSloupce }, new List<object> { hodnotaSloupce });
    }

    private ChangedRows TestRows(string where)
    {
        ChangedRows vr = new ChangedRows();
        try
        {
            vr.beforeChanges = StoredProceduresI.ci.GetDataTable(string.Format("SELECT * FROM {0}{1}", TableNameOfRow(), where));
            vr.quantity = vr.beforeChanges.Rows.Count;
        }
        catch (Exception ex)
        {
            vr.error = ex.Message;
        }
        return vr;
    }

    /// <summary>
    /// Loads the row with the ID of this object into its fields.
    /// </summary>
    public virtual void SelectInTable()
    {
        SelectInTable(new List<string> { "ID" }, new List<object> { GetType().GetField("ID").GetValue(this) });
    }

    /// <summary>
    /// Loads the first row that has the values in the columns into the fields of this object.
    /// </summary>
    public virtual void SelectInTable(List<string> nazvySloupcu, List<object> hodnotaSloupcu)
    {
        DataTable dt = SelectInTableDataTable(nazvySloupcu, hodnotaSloupcu);
        if (dt.Rows.Count != 0)
        {
            FillFromRow(dt.Rows[0]);
        }
    }

    /// <summary>
    /// Loads the first row that has the value in the column into the fields of this object.
    /// </summary>
    public virtual void SelectInTable(string nazevSloupce, object hodnotaSloupce)
    {
        SelectInTable(new List<string> { nazevSloupce }, new List<object> { hodnotaSloupce });
    }

    /// <summary>
    /// Returns all rows that have the values in the columns.
    /// </summary>
    public virtual SelectedRows SelectInTableSelectedRows(List<string> nazvySloupcu, List<object> hodnotaSloupcu)
    {
        SelectedRows vr = new SelectedRows();
        try
        {
            vr.selectedRows = SelectInTableDataTable(nazvySloupcu, hodnotaSloupcu);
            vr.quantity = vr.selectedRows.Rows.Count;
        }
        catch (Exception ex)
        {
            vr.error = ex.Message;
        }
        return vr;
    }

    /// <summary>
    /// Returns all rows that have the value in the column.
    /// </summary>
    public virtual SelectedRows SelectInTableSelectedRows(string nazevSloupce, object hodnotaSloupce)
    {
        return SelectInTableSelectedRows(new List<string> { nazevSloupce }, new List<object> { hodnotaSloupce });
    }

    /// <summary>
    /// Returns all rows that have the values in the columns.
    /// </summary>
    public virtual DataTable SelectInTableDataTable(List<string> nazvySloupcu, List<object> hodnotaSloupcu)
    {
        return StoredProceduresI.ci.GetDataTable(string.Format("SELECT * FROM {0}{1}", TableNameOfRow(), WhereByColumns(nazvySloupcu, hodnotaSloupcu)));
    }

    /// <summary>
    /// Returns all rows that have the value in the column.
    /// </summary>
    public virtual DataTable SelectInTableDataTable(string nazevSloupce, object hodnotaSloupce)
    {
        return SelectInTableDataTable(new List<string> { nazevSloupce }, new List<object> { hodnotaSloupce });
    }

    /// <summary>
    /// Deletes the row with the ID of this object.
    /// </summary>
    public virtual DeletedRows DeleteFromTable()
    {
        return RunDelete(WhereById());
    }

    /// <summary>
    /// Deletes the rows that have the values in the columns.
    /// </summary>
    public virtual DeletedRows DeleteFromTable(List<string> nazvySloupcu, List<object> hodnotaSloupcu)
    {
        return RunDelete(WhereByColumns(nazvySloupcu, hodnotaSloupcu));
    }

    /// <summary>
    /// Deletes the rows that have the value in the column.
    /// </summary>
    public virtual DeletedRows DeleteFromTable(string nazevSloupce, object hodnotaSloupce)
    {
        return DeleteFromTable(new List<string> { nazevSloupce }, new List<object> { hodnotaSloupce });
    }

    private DeletedRows RunDelete(string where)
    {
        DeletedRows vr = new DeletedRows();
        try
        {
            string tabulka = TableNameOfRow();
            vr.deletedRows = StoredProceduresI.ci.GetDataTable(string.Format("SELECT * FROM {0}{1}", tabulka, where));
            vr.quantity = StoredProceduresI.ci.ExecuteNonQuery(string.Format("DELETE FROM {0}{1}", tabulka, where));
        }
        catch (Exception ex)
        {
            vr.chyba = ex.Message;
        }
        return vr;
    }

    /// <summary>
    /// Checks whether the delete of the row with the ID of this object would run (deletes nothing).
    /// </summary>
    public virtual DeletedRows DeleteFromTableTest()
    {
        return TestDelete(WhereById());
    }

    /// <summary>
    /// Checks whether the delete would run (deletes nothing).
    /// </summary>
    public virtual DeletedRows DeleteFromTableTest(List<string> nazvySloupcu, List<object> hodnotaSloupcu)
    {
        return TestDelete(WhereByColumns(nazvySloupcu, hodnotaSloupcu));
    }

    /// <summary>
    /// Checks whether the delete would run (deletes nothing).
    /// </summary>
    public virtual DeletedRows DeleteFromTableTest(string nazevSloupce, object hodnotaSloupce)
    {
        return DeleteFromTableTest(new List<string> { nazevSloupce }, new List<object> { hodnotaSloupce });
    }

    private DeletedRows TestDelete(string where)
    {
        DeletedRows vr = new DeletedRows();
        try
        {
            vr.deletedRows = StoredProceduresI.ci.GetDataTable(string.Format("SELECT * FROM {0}{1}", TableNameOfRow(), where));
            vr.quantity = vr.deletedRows.Rows.Count;
        }
        catch (Exception ex)
        {
            vr.chyba = ex.Message;
        }
        return vr;
    }
}
