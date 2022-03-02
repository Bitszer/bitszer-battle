using System;
using System.Linq;

namespace Hero_Scripts
{
    [Serializable]
    public class PlayerBaseDataDto
    {
        public PlayerBaseDataUnitDto[] Units;
        
        public PlayerBaseDataUnitDto Get(string unitName)
        {
            return Units.FirstOrDefault(unitData => unitData.name == unitName);
        }
    }

    [Serializable]
    public class PlayerBaseDataUnitDto
    {
        public string name;
        public int    level;
        public float  health;
        public float  damage;
        public float  speed;
        public float  cooldown;
    }
}