
namespace SunamoSqlite;
using SunamoSqlite._sunamo;

public static class RHSQLite
{
    public static string chyba = "";
    public static InsertedRows chybaInsertedRows = null;
    public static ChangedRows chybaChangedRows = null;

    public static bool IsNullOrWhiteSpaceField(Type t, string vstup, string nazev)
    {
        // t musí být třIda ve které je A1, ne A1 samotné!!
        FieldInfo fi = t.GetField(nazev);
        // Zde musI být null
        string s = fi.GetValue(null).ToString();
        bool vr = string.IsNullOrWhiteSpace(s);
        if (vr)
        {
            chyba = "PolLOko" + " " + nazev + " " + "nem\u016F\u017Ee b\u00FDt pr\u00E1zdn\u00E1";
        }
        return vr;
    }

    public static bool IsNullOrWhiteSpaceFieldInsertedRows(object o, string vstup, string nazev)
    {
        Type t = o.GetType();
        FieldInfo fi = t.GetField(nazev);
        string s = fi.GetValue(o).ToString();
        bool vr = string.IsNullOrWhiteSpace(s);
        if (vr)
        {
            chybaInsertedRows = new InsertedRows("PolOZko" + " " + nazev + " " + "nem\u016FZe b\u00FDt prPzdn\u00E1" + ". ");
        }
        return vr;
    }

    public static bool IsNullOrWhiteSpaceFieldChangedRows(object o, string vstup, string nazev)
    {
        Type t = o.GetType();
        FieldInfo fi = t.GetField(nazev);
        string s = fi.GetValue(o).ToString();
        bool vr = string.IsNullOrWhiteSpace(s);
        if (vr)
        {
            chybaChangedRows = new ChangedRows("Pol\u00A2Zko" + " " + nazev + " " + "nemUZe b\u00FDt pr\u00E1zdn\u00E1" + ". ");
        }
        return vr;
    }
}
