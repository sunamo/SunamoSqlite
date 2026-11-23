namespace SunamoSqlite;

/// <summary>
/// Conversions of the values of a database table row to .NET types.
/// </summary>
public class TableRowParse
{
    private static bool IsNull(object[] o, int p)
    {
        return o[p] == null || o[p] == DBNull.Value;
    }

    /// <summary>
    /// DBNull gives -1.
    /// </summary>
    public static int GetInt(object[] o, int v)
    {
        return IsNull(o, v) ? -1 : int.Parse(o[v].ToString());
    }

    /// <summary>
    /// The date is stored as a count of ticks (0 = DateTime.MinValue). DBNull gives DateTime.MinValue.
    /// </summary>
    public static DateTime GetDateTime(object[] o, int v)
    {
        if (IsNull(o, v))
        {
            return DateTime.MinValue;
        }
        string s = o[v].ToString();
        if (long.TryParse(s, out long ticks))
        {
            return ticks <= 0 ? DateTime.MinValue : new DateTime(ticks);
        }
        return DateTime.Parse(s);
    }

    /// <summary>
    /// DBNull gives an empty string.
    /// </summary>
    public static string GetString(object[] o, int v)
    {
        return IsNull(o, v) ? string.Empty : o[v].ToString();
    }

    /// <summary>
    /// DBNull gives -1.
    /// </summary>
    public static float GetFloat(object[] o, int p)
    {
        return IsNull(o, p) ? -1 : float.Parse(o[p].ToString());
    }

    /// <summary>
    /// DBNull gives -1.
    /// </summary>
    public static long GetLong(object[] o, int p)
    {
        return IsNull(o, p) ? -1 : long.Parse(o[p].ToString());
    }

    /// <summary>
    /// Understands 0/1 and true/false. DBNull gives false.
    /// </summary>
    public static bool GetBool(object[] o, int p)
    {
        if (IsNull(o, p))
        {
            return false;
        }
        string s = o[p].ToString();
        return s == "1" || string.Equals(s, "true", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Returns "Ano"/"Ne" (yes/no). DBNull gives "Ne".
    /// </summary>
    public static string GetBoolS(object[] o, int p)
    {
        return GetBool(o, p) ? "Ano" : "Ne";
    }

    /// <summary>
    /// Returns the date as text, or null for DBNull.
    /// </summary>
    public static string GetDateTimeS(object[] o, int p)
    {
        return IsNull(o, p) ? null : GetDateTime(o, p).ToString();
    }

    /// <summary>
    /// Returns the content of a BLOB column.
    /// </summary>
    public static byte[] GetImage(object[] o, int dex)
    {
        return IsNull(o, dex) ? null : (byte[])o[dex];
    }

    /// <summary>
    /// DBNull gives -1.
    /// </summary>
    public static decimal GetDecimal(object[] o, int p)
    {
        return IsNull(o, p) ? -1 : decimal.Parse(o[p].ToString());
    }

    /// <summary>
    /// DBNull gives -1.
    /// </summary>
    public static double GetDouble(object[] o, int p)
    {
        return IsNull(o, p) ? -1 : double.Parse(o[p].ToString());
    }

    /// <summary>
    /// DBNull gives -1.
    /// </summary>
    public static short GetShort(object[] o, int p)
    {
        return IsNull(o, p) ? (short)-1 : short.Parse(o[p].ToString());
    }

    /// <summary>
    /// DBNull gives 0.
    /// </summary>
    public static byte GetByte(object[] o, int p)
    {
        return IsNull(o, p) ? (byte)0 : byte.Parse(o[p].ToString());
    }

    /// <summary>
    /// DBNull gives Guid.Empty.
    /// </summary>
    public static Guid GetGuid(object[] o, int p)
    {
        return IsNull(o, p) ? Guid.Empty : Guid.Parse(o[p].ToString());
    }
}
