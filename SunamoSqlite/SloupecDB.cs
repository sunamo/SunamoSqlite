namespace SunamoSqlite;

public class SloupecDB : SloupecDBBase<SloupecDB, TypeAffinity>
{
    public SloupecDB()
    {
    }

    public SloupecDB(TypeAffinity typ, string name)
    {
        this.typ = typ;
        Name = name;
    }

    static SloupecDB()
    {
        SloupecDBBase<SloupecDB, TypeAffinity>.databaseLayer = DatabaseLayer.ci;
    }
}

public class MSSloupecDB : SloupecDBBase<MSSloupecDB, TypeAffinity>
{
    public MSSloupecDB() : base()
    {
        //databaseLayer = MSDatabaseLayer.ci;
    }

    static MSSloupecDB()
    {
        //SloupecDBBase<MSSloupecDB, SqlDbType2>.databaseLayer = MSDatabaseLayer.ci;
        // could set up here also factory column DB
    }

}


public class SloupecDBBase<MSSloupecDB, SqlDbType2>
{


    /// <summary>
    /// Must after call insert to static sloupecDb and databaseLayer
    /// </summary>
    /// <param name="sloupecDb"></param>
    static SloupecDBBase()
    {
        //MSDatabaseLayer.SetFactoryColumnDb();
        // cant set up because its abstract
        //factoryColumnDB = MSFactoryColumnDB.Instance;
    }

    #region MyRegion
    public SqlDbType2 typ = default(SqlDbType2);
    string _nazev = "";
    public Signed _signed = Signed.Other;
    public bool _canBeNull = false;
    public bool canBeNull
    {
        get
        {
            return _canBeNull;
        }
        set
        {
            // 10-12-2019 origianlly there canBeNull return false and set was empty. But then I cant ActivateCs - trying save null to SessionID
            _canBeNull = value;
        }
    }
    public bool mustBeUnique = false;
    public bool primaryKey = false;
    public string referencesTable = null;
    public string referencesColumn = null;

    public SqlDbType Type2
    {
        get
        {
            return (SqlDbType)Enum.Parse(typeof(SqlDbType), Type.ToString());
        }
    }

    public SqlDbType2 Type
    {
        get
        {
            return typ;
        }
        set
        {
            typ = value;
        }
    }

    string delka = "";
    public string Delka
    {
        get
        {
            return delka;
        }
    }

    public string LengthWithoutBraces
    {
        get
        {
            return delka.TrimStart(AllChars.lb).TrimEnd(AllChars.rb);//, AllStrings.lb, AllStrings.rb);
        }
    }
    public bool identityIncrementBy1 = false;
    /// <summary>
    /// Is without lenght
    /// </summary>
    public string Name
    {
        get
        {
            return _nazev;
        }
        set
        {
            int dex = value.IndexOf(AllChars.lb);
            if (dex != -1)
            {
                _nazev = value.Substring(0, dex);
                // The length is given here including the brackets
                delka = value.Substring(dex);//, value.Length - _nazev.Length - 2);
            }
            else
            {
                _nazev = value;
            }
        }
    }

    /// <summary>
    /// IUN, SQL Server to nepodporuje
    /// </summary>
    public Signed IsSigned
    {
        get
        {
            return _signed;
        }
    }

    public bool CanBeNull
    {
        get
        {
            return canBeNull;
        }
    }

    public bool MustBeUnique
    {
        get
        {
            return mustBeUnique;
        }
    }

    public bool PrimaryKey
    {
        get
        {
            return primaryKey;
        }
    }
    public bool isNewId = false;
    public bool IsNewId
    {
        get
        {
            return isNewId;
        }
    }
    #endregion



    #region d
    public string ReferencesTo()
    {
        return string.Format("{0}[{1}]", referencesTable, referencesColumn);
    }

    public string InfoToTextBox()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Data type" + ": " + databaseLayer.UsedTables[typ]);
        sb.AppendLine("Name" + ": " + _nazev);
        //sb.AppendLine("Is primary key" + ": " + BTS.BoolToStringEn(primaryKey));
        //sb.AppendLine("Can be null" + ": " + BTS.BoolToStringEn(canBeNull));
        //sb.AppendLine("Must be unique" + ": " + BTS.BoolToStringEn(mustBeUnique));
        sb.AppendLine();
        if (referencesTable != null)
        {
            sb.AppendLine("Odkazuje na tabulku[sloupec" + "]:");
            sb.AppendLine(ReferencesTo());
        }
        return sb.ToString();
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(databaseLayer.UsedTables[typ] + AllStrings.space + _nazev);
        if (referencesTable != null)
        {
            sb.Append(" " + "odkazuje na" + " " + ReferencesTo());
        }
        return sb.ToString();
    }

    #endregion

    public static IDatabaseLayer<SqlDbType2> databaseLayer = null;

    public static MSSloupecDB CI(SqlDbType2 typ, string nazev, bool primaryKey)
    {

        return factoryColumnDB.CreateInstance(typ, nazev, Signed.Other, false, false, null, null, primaryKey);
    }

    public bool IsUnicode = false;

    public static MSSloupecDB CI(SqlDbType2 typ, string nazev)
    {
        return factoryColumnDB.CreateInstance(typ, nazev, Signed.Other, false, false, null, null, false);
    }

    public static MSSloupecDB CI(SqlDbType2 typ, string nazev, bool primaryKey, string referencesTable, string referencesColumn)
    {
        return factoryColumnDB.CreateInstance(typ, nazev, Signed.Other, false, false, referencesTable, referencesColumn, false);
    }

    /// <summary>
    /// The overload without A3 and A4 passes false for both
    /// </summary>
    /// <param name="typ"></param>
    /// <param name="nazev"></param>
    /// <param name="canBeNull"></param>
    /// <param name="mustBeUnique"></param>
    public static MSSloupecDB CI(SqlDbType2 typ, string nazev, bool canBeNull, bool mustBeUnique)
    {
        MSSloupecDB db = factoryColumnDB.CreateInstance(typ, nazev, Signed.Other, canBeNull, mustBeUnique, null, null, false);
        return db;
    }

    public static MSSloupecDB CI(SqlDbType2 typ, string name, bool canBeNull, bool mustBeUnique, string referencesTable, string referencesColumn)
    {
        return factoryColumnDB.CreateInstance(typ, name, Signed.Other, canBeNull, mustBeUnique, referencesTable, referencesColumn, false);
    }

    public static MSSloupecDB CI(SqlDbType2 typ, string nazev, bool canBeNull, bool mustBeUnique, string referencesTable, string referencesColumn, bool primaryKey)
    {
        return factoryColumnDB.CreateInstance(typ, nazev, Signed.Other, canBeNull, mustBeUnique, referencesTable, referencesColumn, primaryKey);
    }

    public static MSSloupecDB CI(SqlDbType2 typ, MSSloupecDBArgs a)
    {
        return factoryColumnDB.CreateInstance(typ, a);
    }

    public static IFactoryColumnDB<MSSloupecDB, SqlDbType2> factoryColumnDB = null;



}
