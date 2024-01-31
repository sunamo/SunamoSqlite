namespace SunamoSqlite;
public static class GeneratorSqLite
{
    private static Type type = typeof(GeneratorSqLite);

    /// <summary>
    /// Může vrátit null když tabulka bude existovat
    /// Výchozí pro A3 je true
    /// A3 pokud nechci aby se mi vytvářeli reference na ostatní tabulky. Vhodné při testování tabulek a programů, kdy je pak ještě budu mazat.
    /// </summary>
    /// <param name="table"></param>
    /// <param name="sloupce"></param>
    /// <param name="p"></param>
    public static string CreateTable(string table, ColumnsDB sloupce, bool dynamicTables, SQLiteConnection conn)
    {
        StringBuilder sb = new StringBuilder();
        bool exists = StoredProceduresSqliteI.ci.SelectExistsTable(table, conn);
        if (!exists)
        {
            sb.AppendFormat("CREATE TABLE {0}(", table);
            foreach (SloupecDB var in sloupce)
            {
                sb.Append(Column(var, table, dynamicTables) + AllStringsSE.comma);
            }
            string dd = sb.ToString();
            dd = dd.TrimEnd(AllCharsSE.comma);
            string vr = dd + AllStringsSE.rb;
            //vr);
            return vr;
        }
        return null;
    }

    private static string Column(SloupecDB var, string inTable, bool dynamicTables)
    {
        InstantSB sb = new InstantSB(AllStringsSE.space);

        sb.AddItem(var.Name);
        sb.AddItem((var.Type + var.Delka));
        var t = var.Type;



        if (!var.CanBeNull)
        {
            sb.AddItem("NOT NULL");
        }
        if (var.PrimaryKey || var.MustBeUnique)
        {
            if (var.PrimaryKey)
            {
                sb.AddItem("PRIMARY KEY");
            }
            else
            {
                sb.AddItem("UNIQUE");
            }
        }
        if (var.IsNewId)
        {
            sb.AddItem("DEFAULT(newid())");
            //sb.AddItem("DEFAULT newsequentialid()");
        }
        if (!dynamicTables)
        {
            if (var.referencesTable != null)
            {
                ThrowEx.Custom("In SQLite must all columns reference the same table https://www.techonthenet.com/sqlite/foreign_keys/foreign_keys.php");

                //sb.AddItem("CONSTRAINT");
                //sb.AddItem(("fk_" + var.Name + AllStrings.lowbar + inTable + AllStrings.lowbar + var.referencesTable + AllStrings.lowbar + var.referencesColumn));
                //sb.AddItem("FOREIGN KEY REFERENCES");
                //sb.AddItem((var.referencesTable + AllStrings.lb + var.referencesColumn + AllStrings.rb));
            }
        }

        return sb.ToString();
    }

    public static object CombinedWhere(ABC aB)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(" " + "WHERE" + " ");
        bool první = true;
        foreach (AB var in aB)
        {
            if (první)
            {
                první = false;
            }
            else
            {
                sb.Append(" AND ");
            }
            sb.Append(SHFormat.Format2(" {0} = {1} ", var.A, StoredProceduresSqlite.ci.VratHodnotuJednu(var.B)));
        }
        return sb.ToString();
    }
}
