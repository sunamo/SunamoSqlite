namespace SunamoSqlite;
public class DatabaseLayerInstance : IDatabaseLayer<TypeAffinity>
{
    public Dictionary<TypeAffinity, string> usedTa { get => DatabaseLayer.usedTa; set => DatabaseLayer.usedTa = value; }
    public Dictionary<TypeAffinity, string> hiddenTa { get => DatabaseLayer.hiddenTa; set => DatabaseLayer.hiddenTa = value; }
}
