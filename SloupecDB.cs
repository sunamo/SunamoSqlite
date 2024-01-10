namespace SunamoSqlite;

public class SloupecDB : SloupecDBBase<SloupecDB, TypeAffinity>
{
    static SloupecDB()
    {
        SloupecDBBase<MSSloupecDB, TypeAffinity>.databaseLayer = DatabaseLayer.ci;
    }
}
