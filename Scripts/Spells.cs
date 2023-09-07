using Object = UnityEngine.Object;

public enum SpellType
{
    Exterminate = 1, // First high power move
}
public abstract class Spells: Object
{
    public abstract float Damage { get; set; }
    protected abstract float Cooldown { get; set; }

    internal abstract void Execute(ref float cooldown);

}