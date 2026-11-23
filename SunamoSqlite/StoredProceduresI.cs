namespace SunamoSqlite;

/// <summary>
/// Stored procedures that run at once over the current connection to the SQLite database.
/// </summary>
public class StoredProceduresI : IStoredProceduresI
{
    private SQLiteConnection _conn => DatabaseLayer.conn;

    /// <summary>
    /// The single instance.
    /// </summary>
    public static StoredProceduresI ci = new StoredProceduresI();

    /// <summary>
    /// Returns the count of rows in the table.
    /// </summary>
    public int SelectFindOutNumberOfRows(object tableName)
    {
        return FindOutNumberOfRows(tableName.ToString());
    }

    /// <summary>
    /// Inserts a row into the table without column names and returns the current count of rows.
    /// </summary>
    public int InsertToTable(object tableName, object p, int iD, params object[] p2)
    {
        List<object> hodnoty = new List<object> { iD };
        hodnoty.AddRange(p2);
        string sql = string.Format("INSERT INTO {0} VALUES {1}", tableName, StoredProcedures.ci.GetValues(hodnoty.ToArray()));
        ExecuteNonQuery(sql);
        return FindOutNumberOfRows(tableName.ToString());
    }

    /// <summary>
    /// Returns the names of the columns of the table.
    /// </summary>
    public List<string> VratNazvySloupcuTabulky(string p)
    {
        List<string> vr = new List<string>();
        DataTable dt = GetDataTable(string.Format("PRAGMA table_info({0})", p));
        foreach (DataRow row in dt.Rows)
        {
            vr.Add(row["name"].ToString());
        }
        return vr;
    }

    /// <summary>
    /// Returns the names of the columns of the table.
    /// </summary>
    public List<string> SelectColumnsNamesOfTable(string table)
    {
        return VratNazvySloupcuTabulky(table);
    }

    /// <summary>
    /// Finds out whether the table exists in the database.
    /// </summary>
    public bool SelectExistsTable(string table)
    {
        return GetDataTable("SELECT name FROM sqlite_master WHERE type='table' AND name='" + table + "'").Rows.Count != 0;
    }

    /// <summary>
    /// Returns the names of all tables in the database.
    /// </summary>
    public List<string> SelectGetAllTablesInDB()
    {
        return GetValuesAllRowsString("SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%'");
    }

    /// <summary>
    /// Runs the SQL query and returns the result as a table.
    /// </summary>
    public DataTable GetDataTable(string sql)
    {
        SQLiteCommand comm = new SQLiteCommand(sql, _conn);
        return GetDataTable(comm);
    }

    /// <summary>
    /// Runs the SQL query and returns the result as a table.
    /// </summary>
    public DataTable SelectDataTable(string sql)
    {
        return GetDataTable(sql);
    }

    /// <summary>
    /// Runs the command and returns the result as a table.
    /// </summary>
    public DataTable GetDataTable(SQLiteCommand comm)
    {
        DataTable dt = new DataTable();
        SQLiteDataAdapter adapter = new SQLiteDataAdapter(comm);
        adapter.Fill(dt);
        return dt;
    }

    /// <summary>
    /// Returns the ID of the row that has the value in the column.
    /// </summary>
    public int SelectID(string tabulka, string nazevSloupce, object hodnotaSloupce)
    {
        return FindOutID(tabulka, nazevSloupce, hodnotaSloupce);
    }

    /// <summary>
    /// Returns the ID of the row that has the value in the column. When there is no such row, returns the number of the next row (count of rows + 1).
    /// </summary>
    public int SelectIDOrNextIndex(string tabulka, string nazevSloupce, object hodnotaSloupce)
    {
        int id = FindOutID(tabulka, nazevSloupce, hodnotaSloupce);
        return id == 0 ? FindOutNumberOfRows(tabulka) + 1 : id;
    }

    /// <summary>
    /// Returns all rows of the table.
    /// </summary>
    public DataTable GetDataTableAllRows(string p)
    {
        return GetDataTable("SELECT * FROM" + " " + p);
    }

    /// <summary>
    /// Returns all rows of the table.
    /// </summary>
    public DataTable SelectDataTableAllRows(string p)
    {
        return GetDataTableAllRows(p);
    }

    /// <summary>
    /// Whether the table contains a row that has the value in the column.
    /// </summary>
    public bool ContainsRowsInTableValue(string tabulka, string sloupec, object hodnota)
    {
        DataTable dt = GetDataTableSelective(tabulka, sloupec, hodnota);
        return dt.Rows.Count != 0;
    }

    /// <summary>
    /// Whether the table contains a row that has the value in the column.
    /// </summary>
    public bool SelectContainsRowsInTableValue(string tabulka, string sloupec, object hodnota)
    {
        return ContainsRowsInTableValue(tabulka, sloupec, hodnota);
    }

    /// <summary>
    /// Whether the table contains a row that has the value in the column.
    /// </summary>
    public bool SelectExists(string tabulka, string sloupec, object hodnota)
    {
        return ContainsRowsInTableValue(tabulka, sloupec, hodnota);
    }

    /// <summary>
    /// Deletes the rows of the table that have the value in the column.
    /// </summary>
    public int Delete(string tabulka, string sloupec, object hodnota)
    {
        return ExecuteNonQuery(string.Format("DELETE FROM {0} WHERE {1} = {2}", tabulka, sloupec, StoredProcedures.ci.ReplaceValueOnlyOne(hodnota)));
    }

    /// <summary>
    /// Inserts a row into the table and returns the current count of rows.
    /// Column names are given without quotation marks.
    /// </summary>
    public int InsertToTable(string tabulka, string nazvySloupcu, params object[] sloupce)
    {
        SQLiteCommand comm = new SQLiteCommand(string.Format("INSERT INTO {0} {1} VALUES {2}", tabulka, nazvySloupcu, StoredProcedures.ci.GetValues(sloupce)), _conn);
        comm.ExecuteNonQuery();
        return FindOutNumberOfRows(tabulka);
    }

    /// <summary>
    /// Returns all rows of the table that have the value in the column.
    /// </summary>
    public DataTable GetDataTableSelective(string tabulka, string sloupec, object hodnota)
    {
        return GetDataTable("SELECT * FROM" + " " + tabulka + " " + "WHERE" + " " + sloupec + " = " + StoredProcedures.ci.ReplaceValueOnlyOne(hodnota));
    }

    private DataTable GetDataTableSelective(string tabulka, string sloupecID, int id, string hledanySloupec)
    {
        return GetDataTable("SELECT" + " " + hledanySloupec + " " + "FROM" + " " + tabulka + " " + "WHERE" + " " + sloupecID + " = " + StoredProcedures.ci.ReplaceValueOnlyOne(id));
    }

    /// <summary>
    /// Returns the values of column A2 of the rows that have the value A4 in column A3.
    /// </summary>
    public List<int> GetValueColumnInt(string tabulka, string sloupecHledane, string sloupecVeKteremHledat, object hodnota)
    {
        string sql = string.Format("SELECT {0} FROM {1} WHERE {2} = {3}", sloupecHledane, tabulka, sloupecVeKteremHledat, StoredProcedures.ci.ReplaceValueOnlyOne(hodnota));
        return GetValuesAllRowsInt(sql);
    }

    /// <summary>
    /// Returns the values of column A2 of the rows that have the value A4 in column A3.
    /// </summary>
    public List<int> SelectValuesOfColumnInt(string tabulka, string sloupecHledane, string sloupecVeKteremHledat, object hodnota)
    {
        return GetValueColumnInt(tabulka, sloupecHledane, sloupecVeKteremHledat, hodnota);
    }

    /// <summary>
    /// Whether the table has a row that matches all the column-value pairs.
    /// </summary>
    public bool SelectExistsCombination(string tabulka, params AB[] aB)
    {
        return ExistsCombination(tabulka, aB);
    }

    private List<int> GetValuesAllRowsInt(string sql)
    {
        List<int> vr = new List<int>();
        DataTable dt = GetDataTable(sql);
        foreach (DataRow var in dt.Rows)
        {
            vr.Add(int.Parse(var.ItemArray[0].ToString()));
        }
        return vr;
    }

    private List<string> GetValuesAllRowsString(string sql)
    {
        List<string> vr = new List<string>();
        DataTable dt = GetDataTable(sql);
        foreach (DataRow var in dt.Rows)
        {
            vr.Add(var.ItemArray[0].ToString());
        }
        return vr;
    }

    /// <summary>
    /// Returns the values of the first column of all rows that have the value A3 in column A2.
    /// </summary>
    public List<object> GetValuesAllRows(string tabulka, string nazevSloupce, object hodnotaSloupce)
    {
        List<object> vr = new List<object>();
        DataTable dt = GetDataTableSelective(tabulka, nazevSloupce, hodnotaSloupce);
        foreach (DataRow var in dt.Rows)
        {
            vr.Add(var.ItemArray[0]);
        }
        return vr;
    }

    /// <summary>
    /// Returns the values of the first column of all rows that have the value A3 in column A2.
    /// </summary>
    public List<object> SelectValuesAllRowsOfFirstColumnObject(string tabulka, string nazevSloupce, object hodnotaSloupce)
    {
        return GetValuesAllRows(tabulka, nazevSloupce, hodnotaSloupce);
    }

    /// <summary>
    /// Returns the values (names) of the rows with the IDs.
    /// </summary>
    public List<string> FindValuesOfIDs(string tabulka, List<int> idFces)
    {
        List<string> vr = new List<string>();
        foreach (int var in idFces)
        {
            vr.Add(FindValueOfID(tabulka, var));
        }
        return vr;
    }

    /// <summary>
    /// Returns the values (names) of the rows with the IDs.
    /// </summary>
    public List<string> SelectValuesOfIDs(string tabulka, List<int> idFces)
    {
        return FindValuesOfIDs(tabulka, idFces);
    }

    /// <summary>
    /// Returns the values of the column from all rows of the table.
    /// </summary>
    public List<string> VratVsechnyHodnotySloupce(string tabulka, string sloupec)
    {
        return GetValuesAllRowsString(string.Format("SELECT {0} FROM {1}", sloupec, tabulka));
    }

    /// <summary>
    /// Returns the values of the column from all rows of the table.
    /// </summary>
    public List<string> SelectValuesOfColumnAllRowsString(string tabulka, string sloupec)
    {
        return VratVsechnyHodnotySloupce(tabulka, sloupec);
    }

    /// <summary>
    /// Whether the table has a row that matches all the column-value pairs.
    /// </summary>
    public bool ExistsCombination(string p, params AB[] aB)
    {
        string sql = string.Format("SELECT {0} FROM {1} {2}", aB[0].A, p, GeneratorSqLite.CombinedWhere(new ABC(aB)));
        DataTable dt = GetDataTable(sql);
        return dt.Rows.Count != 0;
    }

    /// <summary>
    /// Works only when the name column has index 1.
    /// </summary>
    public string FindValueOfID(string tabulka, int id)
    {
        return GetElementDataTable(GetDataTableSelective(tabulka, "ID", id), 0, 1);
    }

    /// <summary>
    /// Works only when the name column has index 1.
    /// </summary>
    public string SelectValueOfID(string tabulka, int id)
    {
        return FindValueOfID(tabulka, id);
    }

    /// <summary>
    /// Returns the value of the row with the ID, or an empty string when there is no such row.
    /// </summary>
    public string SelectValueOfIDOrSE(string tabulka, int id)
    {
        DataTable dt = GetDataTableSelective(tabulka, "ID", id);
        return dt.Rows.Count == 0 ? "" : GetElementDataTable(dt, 0, 1);
    }

    /// <summary>
    /// Returns the item in row A2 and column A3 of the table.
    /// Throws when the row or the column is not found.
    /// </summary>
    private string GetElementDataTable(DataTable dataTable, int radek, int sloupec)
    {
        if (dataTable.Rows.Count > radek)
        {
            object[] o = dataTable.Rows[radek].ItemArray;

            if (o.Length > sloupec)
            {
                if (o[sloupec] == null)
                {
                    return null;
                }
                return o[sloupec].ToString();
            }
        }
        throw new Exception("The cell was not found");
    }

    static Type type = typeof(StoredProceduresI);

    /// <summary>
    /// Runs a command that returns no data.
    /// </summary>
    public int ExecuteNonQuery(string p)
    {
        SQLiteCommand comm = new SQLiteCommand(p, _conn);
        return comm.ExecuteNonQuery();
    }

    /// <summary>
    /// Adds the value to the real number in the row (the average when a value is already there).
    /// </summary>
    public double UpdatePlusRealValue(string table, string sloupecKUpdate, double pridej, string sloupecID, int id)
    {
        double d = double.Parse(GetElementDataTable(table, sloupecID, id, sloupecKUpdate));
        double n = pridej;
        if (d != 0)
        {
            n = (d + pridej) / 2;
        }
        Update(table, sloupecID, id, sloupecKUpdate, n);
        return n;
    }

    private int Update(string table, string sloupecID, int id, string sloupecKUpdate, double n)
    {
        string sql = string.Format("UPDATE {0} SET {1}={2} WHERE {3} = {4}", table, sloupecKUpdate, StoredProcedures.ci.ReplaceValueOnlyOne(n), sloupecID, StoredProcedures.ci.ReplaceValueOnlyOne(id));
        SQLiteCommand comm = new SQLiteCommand(sql, _conn);
        return comm.ExecuteNonQuery();
    }

    /// <summary>
    /// Returns the value of column A4 of the row with ID A3.
    /// </summary>
    public string GetElementDataTable(string table, string sloupecID, int id, string hledanySloupec)
    {
        DataTable dt = GetDataTableSelective(table, sloupecID, id, hledanySloupec);
        return GetElementDataTable(dt, 0, 0);
    }

    /// <summary>
    /// Returns the chosen columns of all rows of the table.
    /// </summary>
    public DataTable GetDataTable(string p, params string[] selectSloupce)
    {
        return GetDataTable(string.Format("SELECT {0} FROM {1}", StoredProcedures.ci.GetColumnsWithoutBracets(selectSloupce.ToList()), p));
    }

    /// <summary>
    /// Returns the ID of the first row that has the value in the column, or 0 when there is no such row.
    /// </summary>
    public int FindOutID(string tabulka, string nazevSloupce, object hodnotaSloupce)
    {
        DataTable dt = GetDataTableSelective(tabulka, nazevSloupce, hodnotaSloupce);
        if (dt.Rows.Count == 0)
        {
            return 0;
        }
        return int.Parse(dt.Rows[0].ItemArray[0].ToString());
    }

    /// <summary>
    /// Returns the count of rows in the table.
    /// </summary>
    public int FindOutNumberOfRows(string tabulka)
    {
        DataTable dt = GetDataTable("SELECT COUNT(*) FROM " + tabulka);
        return int.Parse(dt.Rows[0].ItemArray[0].ToString());
    }

    /// <summary>
    /// Inserts a row with the name into the lookup table when it is not there yet.
    /// </summary>
    public SQLiteCommand InsertRowTypeEnumIfNotExists(string tabulka, string nazev)
    {
        if (!ContainsRowsInTableValue(tabulka, "Name", nazev))
        {
            SQLiteCommand comm = StoredProcedures.ci.InsertRowTypeEnumIfNotExists(tabulka, nazev);
            comm.ExecuteNonQuery();
            return comm;
        }
        return null;
    }

    /// <summary>
    /// Drops the table when it exists.
    /// </summary>
    public SQLiteCommand DeleteTableIfExists(string nazevTabulky)
    {
        SQLiteCommand comm = StoredProcedures.ci.DeleteTableIfExists(nazevTabulky);
        comm.ExecuteNonQuery();
        return comm;
    }

    /// <summary>
    /// Drops the table when it exists.
    /// </summary>
    public SQLiteCommand DropTableIfExists(string nazevTabulky)
    {
        return DeleteTableIfExists(nazevTabulky);
    }
}
