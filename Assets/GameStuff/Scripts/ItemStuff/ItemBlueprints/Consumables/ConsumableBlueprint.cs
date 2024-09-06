
public abstract class ConsumableBlueprint : ItemBlueprint
{
    public string[] effectDescription;

    public virtual string[] FullDescription()
    {
        return effectDescription;
    }
    public abstract void ConsumeItem(Player player);
}
