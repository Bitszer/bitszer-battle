public sealed class HeroUnitData
{
    public const int LevelMax = 5;

    public readonly int    id;
    public readonly string typeName;
    public readonly string description;

    public int   Level              { get; private set; }
    public float Health             { get; private set; }
    public float HealthBase         { get; private set; }
    public float Damage             { get; private set; }
    public float DamageBase         { get; private set; }
    public float Speed              { get; private set; }
    public float CoolDown           { get; private set; }
    public float SpawnCost          { get; private set; }
    public bool  IsUnlockAvailable  { get; private set; }
    public bool  IsUpgradeAvailable { get; private set; }

    public HeroController Prototype { get; private set; }

    public bool IsUnlocked => Level > 0;
    public bool IsLocked   => Level <= 0;
    public bool IsMaxLevel => Level >= LevelMax;

    public HeroUnitData(int id, string typeName, string description)
    {
        this.id = id;
        this.typeName = typeName;
        this.description = description;
    }

    public void SetLevel(int level)
    {
        Level = level;

        GetLevelStats(level, out var health, out var damage);
        Damage = damage;
        Health = health;
    }

    public void Update(float health,
                       float healthBase,
                       float damage,
                       float damageBase,
                       float speed,
                       float cooldown,
                       bool isUnlockAvailable,
                       bool isUpgradeAvailable)
    {
        // Health = health;
        // HealthBase = healthBase;
        // Damage = damage;
        // DamageBase = damageBase;
        // Speed = speed;
        // CoolDown = cooldown;
        IsUnlockAvailable = isUnlockAvailable;
        IsUpgradeAvailable = isUpgradeAvailable;
    }
    
    /*
     * Stats.
     */

    public void GetLevelStats(int level, out float health, out float damage)
    {
        health = HealthBase;
        damage = DamageBase;
        
        for (var i = 1; i < level; i++)
        {
            health *= 1.20F;
            damage *= 1.20F;    
        }
    }

    /*
     * Static.
     */

    public static HeroUnitData FromHeroPrototype(HeroController prototype)
    {
        var data = new HeroUnitData(prototype.Id, prototype.TypeName, prototype.Description)
        {
            Health = prototype.Health,
            HealthBase = prototype.Health,
            Damage = prototype.Damage,
            DamageBase = prototype.Damage,
            Speed = prototype.Speed,
            CoolDown = prototype.CoolDown,
            SpawnCost = prototype.SpawnCost,
            Prototype = prototype
        };
        return data;
    }
}