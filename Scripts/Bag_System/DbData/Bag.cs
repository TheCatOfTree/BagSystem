using Unity.VisualScripting.Dependencies.Sqlite;

public class Bag
{
    [PrimaryKey, AutoIncrement] 
    public int    Id     { get; set; }
    public int    Count   { get; set; }

    public Bag()
    {
        
    }
   public Bag(int id,int count)
    {
        Id = id;
        Count = count;
    }
    /// <summary>
    /// 重写ToString函数，方便控制台打印
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"[Person: Id={Id},  Count={Count}]";
    }
}