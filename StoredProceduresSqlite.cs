namespace SunamoSqlite;
public class StoredProceduresSqlite : IStoredProcedures
{
    public static StoredProceduresSqlite ci = new StoredProceduresSqlite();

    public SQLiteCommand InsertRowTypeEnumIfNotExists(string tabulka, string nazev)
    {
        return GetCmdInFormat("INSERT INTO {0} (ID,Nazev) VALUES (NULL,{1})", tabulka, nazev);
    }

    public SQLiteCommand GetCmdInFormat(string f, params object[] p)
    {
        return GetCmdInFormat(f, new List<int>(), p);
    }

    public SQLiteCommand GetCmdInFormat(string f, List<int> nenahrazovat, params object[] p)
    {
        for (int i = 0; i < p.Length; i++)
        {
            if (!nenahrazovat.Contains(i))
            {
                //string nc = "";
                f = ReplaceValueOnlyOne(f, p[i], i);
            }
        }
        return new SQLiteCommand(f, DatabaseLayer.conn);
        //return new SQLiteCommand(string.Format(f, p), DatabaseLayerSqlite.conn);
    }

    public object VratHodnotuJednu(object b)
    {
        return b;
    }

    public string ReplaceValueOnlyOne(string f, object p, int i)
    {
        if (p != null)
        {
            string nahraditCim = ReplaceValueOnlyOne(p);

            f = f.Replace(AllStringsSE.lcub + i.ToString() + AllStringsSE.rcub, nahraditCim);
        }
        else
        {
            f = f.Replace(AllStringsSE.lcub + i.ToString() + AllStringsSE.rcub, "NULL");
        }
        return f;
    }

    public string ReplaceValueOnlyOne(object p)
    {
        if (p != null)
        {
            string nahraditCim = p.ToString();
            if (p.GetType() == typeof(string))
            {
                // Musím vrátit hned protoZe na konci mi to replacuje uvozovky
                return "'" + nahraditCim.Replace(AllCharsSE.bs, AllCharsSE.space) + "'";
            }
            else if (p.GetType() == typeof(bool))
            {
                bool b = (bool)p;
                if (b)
                {
                    return "1";
                }
                return "0";
            }
            else if (p.GetType() == typeof(byte[]))
            {
                nahraditCim = DatabaseLayer.ToBlob((byte[])p);
            }
            else if (p.GetType() == typeof(DateTime))
            {
                DateTime dt = DateTime.Parse(nahraditCim);
                if (dt == DateTime.MinValue)
                {
                    nahraditCim = "0";
                }
                else
                {
                    nahraditCim = dt.Ticks.ToString();
                }
            }
            else if (p.GetType() == typeof(double))
            {
                nahraditCim = nahraditCim.Replace(AllStringsSE.comma, AllStringsSE.dot);
            }
            return nahraditCim.Replace("'", "");
        }

        return "NULL";
    }




    public string GetValues(params object[] sloupce)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(AllStringsSE.lb);
        foreach (object var in sloupce)
        {
            sb.Append(ReplaceValueOnlyOne(var) + AllStringsSE.comma);
        }
        string vr = sb.ToString().TrimEnd(AllCharsSE.comma) + AllStringsSE.rb;
        return vr;
    }

    public string GetColumns(List<string> sloupce)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(AllStringsSE.lb);
        foreach (string var in sloupce)
        {
            sb.Append(var + AllStringsSE.comma);
        }
        string vr = sb.ToString().TrimEnd(AllCharsSE.comma) + AllStringsSE.rb;
        return vr;
    }

    public string GetColumns(string tabulka)
    {
        List<string> sloupce = StoredProceduresSqliteI.ci.VratNazvySloupcuTabulky(tabulka);
        StringBuilder sb = new StringBuilder();
        sb.Append(AllStringsSE.lb);
        foreach (string var in sloupce)
        {
            sb.Append(var + AllStringsSE.comma);
        }
        string vr = sb.ToString().TrimEnd(AllCharsSE.comma) + AllStringsSE.rb;
        return vr;
    }

    public string GetColumnsWithoutBracets(List<string> sloupce)
    {
        StringBuilder sb = new StringBuilder();
        //sb.Append(AllStrings.lb);
        foreach (string var in sloupce)
        {
            sb.Append(var + AllStringsSE.comma);
        }
        string vr = sb.ToString().TrimEnd(AllCharsSE.comma);// +AllStrings.rb;
        return vr;
    }

    public SQLiteCommand DeleteTableIfExists(string nazevTabulky)
    {
        return null;
    }
}
