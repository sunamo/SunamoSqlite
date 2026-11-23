namespace SunamoSqlite._public.SunamoInterfaces.Interfaces;

/// <summary>
/// Must be in sunamo because is shared between MSSQL and SQL Server project.
/// </summary>
/// <typeparam name="SqlDbType">The type representing SQL database types.</typeparam>
public interface IDatabaseLayer<SqlDbType> where SqlDbType : notnull
{
    /// <summary>
    /// Dictionary of used tables.
    /// </summary>
    Dictionary<SqlDbType, string> UsedTables { get; set; }

    /// <summary>
    /// Dictionary of hidden tables.
    /// </summary>
    Dictionary<SqlDbType, string> HiddenTables { get; set; }
}