public class Temple
{
    public readonly float baseHealth;
    public int   level;
    public float health;
    public bool  isUpgradeAvailable;

    public Temple()
    {
        baseHealth = 500;
        level = 1;
        health = 0;
        isUpgradeAvailable = false;
    }
}