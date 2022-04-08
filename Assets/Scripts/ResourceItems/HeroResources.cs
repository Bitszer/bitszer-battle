using System;
using System.Collections.Generic;
using UnityEngine;
using Utility.Logging;

[Serializable]
public class HeroResources
{
    public Wood  wood;
    public Ore   ore;
    public Food  food;
    public Herbs herbs;
    
    /*
     * Operators.
     */
    
    public static HeroResources operator +(HeroResources resources01, HeroResources resources02)
    {
        var result = new HeroResources();

        result.wood.Stick = resources01.wood.Stick + resources02.wood.Stick;
        result.wood.Lumber = resources01.wood.Lumber + resources02.wood.Lumber;
        result.wood.Ironwood = resources01.wood.Ironwood + resources02.wood.Ironwood;
        result.wood.Bloodwood = resources01.wood.Bloodwood + resources02.wood.Bloodwood;

        result.ore.Copper = resources01.ore.Copper + resources02.ore.Copper;
        result.ore.Silver = resources01.ore.Silver + resources02.ore.Silver;
        result.ore.Gold = resources01.ore.Gold + resources02.ore.Gold;
        result.ore.Platinum = resources01.ore.Platinum + resources02.ore.Platinum;

        result.food.Wheat = resources01.food.Wheat + resources02.food.Wheat;
        result.food.Corn = resources01.food.Corn + resources02.food.Corn;
        result.food.Rice = resources01.food.Rice + resources02.food.Rice;
        result.food.Potatoes = resources01.food.Potatoes + resources02.food.Potatoes;

        result.herbs.Sage = resources01.herbs.Sage + resources02.herbs.Sage;
        result.herbs.Rosemary = resources01.herbs.Rosemary + resources02.herbs.Rosemary;
        result.herbs.Chamomile = resources01.herbs.Chamomile + resources02.herbs.Chamomile;
        result.herbs.Valerian = resources01.herbs.Valerian + resources02.herbs.Valerian;

        return result;
    }
    
    public static HeroResources operator -(HeroResources resources01, HeroResources resources02)
    {
        var result = new HeroResources();

        result.wood.Stick = resources01.wood.Stick - resources02.wood.Stick;
        result.wood.Lumber = resources01.wood.Lumber - resources02.wood.Lumber;
        result.wood.Ironwood = resources01.wood.Ironwood - resources02.wood.Ironwood;
        result.wood.Bloodwood = resources01.wood.Bloodwood - resources02.wood.Bloodwood;

        result.ore.Copper = resources01.ore.Copper - resources02.ore.Copper;
        result.ore.Silver = resources01.ore.Silver - resources02.ore.Silver;
        result.ore.Gold = resources01.ore.Gold - resources02.ore.Gold;
        result.ore.Platinum = resources01.ore.Platinum - resources02.ore.Platinum;

        result.food.Wheat = resources01.food.Wheat - resources02.food.Wheat;
        result.food.Corn = resources01.food.Corn - resources02.food.Corn;
        result.food.Rice = resources01.food.Rice - resources02.food.Rice;
        result.food.Potatoes = resources01.food.Potatoes - resources02.food.Potatoes;

        result.herbs.Sage = resources01.herbs.Sage - resources02.herbs.Sage;
        result.herbs.Rosemary = resources01.herbs.Rosemary - resources02.herbs.Rosemary;
        result.herbs.Chamomile = resources01.herbs.Chamomile - resources02.herbs.Chamomile;
        result.herbs.Valerian = resources01.herbs.Valerian - resources02.herbs.Valerian;

        return result;
    }

    public HeroResources()
    {
        food = new Food();
        ore = new Ore();
        wood = new Wood();
        herbs = new Herbs();
    }

    public void Make(ref List<string> resources, ref List<string> quantity)
    {
        resources.Clear();
        quantity.Clear();

        for (var i = 0; i < 16; i++)
        {
            switch (i)
            {
                case 0:
                    resources.Add("Stick");
                    quantity.Add(wood.Stick.ToString());
                    break;
                case 1:
                    resources.Add("Lumber");
                    quantity.Add(wood.Lumber.ToString());
                    break;
                case 2:
                    resources.Add("Ironwood");
                    quantity.Add(wood.Ironwood.ToString());
                    break;
                case 3:
                    resources.Add("Bloodwood");
                    quantity.Add(wood.Bloodwood.ToString());
                    break;
                case 4:
                    resources.Add("Copper");
                    quantity.Add(ore.Copper.ToString());
                    break;
                case 5:
                    resources.Add("Silver");
                    quantity.Add(ore.Silver.ToString());
                    break;
                case 6:
                    resources.Add("Gold");
                    quantity.Add(ore.Gold.ToString());
                    break;
                case 7:
                    resources.Add("Platinum");
                    quantity.Add(ore.Platinum.ToString());
                    break;
                case 8:
                    resources.Add("Wheat");
                    quantity.Add(food.Wheat.ToString());
                    break;
                case 9:
                    resources.Add("Corn");
                    quantity.Add(food.Corn.ToString());
                    break;
                case 10:
                    resources.Add("Rice");
                    quantity.Add(food.Rice.ToString());
                    break;
                case 11:
                    resources.Add("Potatoes");
                    quantity.Add(food.Potatoes.ToString());
                    break;
                case 12:
                    resources.Add("Sage");
                    quantity.Add(herbs.Sage.ToString());
                    break;
                case 13:
                    resources.Add("Rosemary");
                    quantity.Add(herbs.Rosemary.ToString());
                    break;
                case 14:
                    resources.Add("Chamomile");
                    quantity.Add(herbs.Chamomile.ToString());
                    break;
                case 15:
                    resources.Add("Valerian");
                    quantity.Add(herbs.Valerian.ToString());
                    break;
            }
        }
        
    }

    public HeroResources GetResourcesForLevel(int level)
    {
        if (level == 0)
            level = 1;

        var required = new HeroResources();
        required.wood.Stick = wood.Stick * level;
        required.wood.Lumber = wood.Lumber * level;
        required.wood.Ironwood = wood.Ironwood * level;
        required.wood.Bloodwood = wood.Bloodwood * level;

        required.ore.Copper = ore.Copper * level;
        required.ore.Silver = ore.Silver * level;
        required.ore.Gold = ore.Gold * level;
        required.ore.Platinum = ore.Platinum * level;

        required.food.Wheat = food.Wheat * level;
        required.food.Corn = food.Corn * level;
        required.food.Rice = food.Rice * level;
        required.food.Potatoes = food.Potatoes * level;

        required.herbs.Sage = herbs.Sage * level;
        required.herbs.Rosemary = herbs.Rosemary * level;
        required.herbs.Chamomile = herbs.Chamomile * level;
        required.herbs.Valerian = herbs.Valerian * level;

        return required;
    }

    public bool IsEnoughForUpgrade(HeroResources resources, int currentLevel)
    {
        //currentLevel = currentLevel < 0 ? 1 : currentLevel + 1;
        currentLevel = currentLevel < 0 ? 1 : currentLevel;

        //Debug.Log("wood.Stick: " + (wood.Stick * currentLevel <= resources.wood.Stick));
        //Debug.Log("wood.Lumber: " + (wood.Lumber * currentLevel <= resources.wood.Lumber));
        //Debug.Log("wood.Ironwood: " + (wood.Ironwood * currentLevel <= resources.wood.Ironwood));
        //Debug.Log("wood.Bloodwood: " + (wood.Bloodwood * currentLevel <= resources.wood.Bloodwood));
        //Debug.Log("ore.Copper: " + (ore.Copper * currentLevel <= resources.ore.Copper));
        //Debug.Log("ore.Silver: " + (ore.Silver * currentLevel <= resources.ore.Silver));
        //Debug.Log("ore.Gold: " + (ore.Gold * currentLevel <= resources.ore.Gold));
        //Debug.Log("ore.Platinum: " + (ore.Platinum * currentLevel <= resources.ore.Platinum));
        //Debug.Log("food.Wheat: " + (food.Wheat * currentLevel <= resources.food.Wheat));
        //Debug.Log("food.Corn: " + (food.Corn * currentLevel <= resources.food.Corn));
        //Debug.Log("food.Rice: " + (food.Rice * currentLevel <= resources.food.Rice));
        //Debug.Log("food.Potatoes: " + (food.Potatoes * currentLevel <= resources.food.Potatoes));
        //Debug.Log("herbs.Sage: " + (herbs.Sage * currentLevel <= resources.herbs.Sage));
        //Debug.Log("herbs.Sage: " + herbs.Sage);
        //Debug.Log("herbs.Sage * currentLevel: " + herbs.Sage * currentLevel);
        //Debug.Log("resources.herbs.Sage: " + resources.herbs.Sage);
        //Debug.Log("herbs.Rosemary: " + (herbs.Rosemary * currentLevel <= resources.herbs.Rosemary));
        //Debug.Log("herbs.Chamomile: " + (herbs.Chamomile * currentLevel <= resources.herbs.Chamomile));
        //Debug.Log("herbs.Valerian: " + (herbs.Valerian * currentLevel <= resources.herbs.Valerian));

        if (wood.Stick * currentLevel <= resources.wood.Stick &&
            wood.Lumber * currentLevel <= resources.wood.Lumber &&
            wood.Ironwood * currentLevel <= resources.wood.Ironwood &&
            wood.Bloodwood * currentLevel <= resources.wood.Bloodwood &&
            ore.Copper * currentLevel <= resources.ore.Copper &&
            ore.Silver * currentLevel <= resources.ore.Silver &&
            ore.Gold * currentLevel <= resources.ore.Gold &&
            ore.Platinum * currentLevel <= resources.ore.Platinum &&
            food.Wheat * currentLevel <= resources.food.Wheat &&
            food.Corn * currentLevel <= resources.food.Corn &&
            food.Rice * currentLevel <= resources.food.Rice &&
            food.Potatoes * currentLevel <= resources.food.Potatoes &&
            herbs.Sage * currentLevel <= resources.herbs.Sage &&
            herbs.Rosemary * currentLevel <= resources.herbs.Rosemary &&
            herbs.Chamomile * currentLevel <= resources.herbs.Chamomile &&
            herbs.Valerian * currentLevel <= resources.herbs.Valerian)
            return true;
        return false;
    }

    public Dictionary<string, int> ToAuctionHouseDictionary()
    {
        return new Dictionary<string, int>
        {
            { "wood_stick", wood.Stick },
            { "wood_lumber", wood.Lumber },
            { "wood_ironwood", wood.Ironwood },
            { "wood_bloodwood", wood.Bloodwood },
            { "ore_copper", ore.Copper },
            { "ore_silver", ore.Silver },
            { "ore_gold", ore.Gold },
            { "ore_platinum", ore.Platinum },
            { "food_wheat", food.Wheat },
            { "food_rice", food.Rice },
            { "food_corn", food.Corn },
            { "food_potatoes", food.Potatoes },
            { "herbs_sage", herbs.Sage },
            { "herbs_rosemary", herbs.Rosemary },
            { "herbs_chamomile", herbs.Chamomile },
            { "herbs_valerian", herbs.Valerian }
        };
    }

    /*
     * Debugging.
     */
    
    public void Log(ILog log)
    {
        log.Debug("==================================================");
        
        log.Debug("Wood:");
        wood.Log(log);
        
        log.Debug("Ore:");
        ore.Log(log);
        
        log.Debug("Food:");
        food.Log(log);
        
        log.Debug("Herbs:");
        herbs.Log(log);
        
        log.Debug("==================================================");
    }

    /*
     * Static.
     */

    public static HeroResources FromAuctionHouseDictionary(Dictionary<string, int> dict)
    {
        var res = new HeroResources();

        res.wood.Stick = dict.GetValueOrDefault("wood_stick", 0);
        res.wood.Lumber = dict.GetValueOrDefault("wood_lumber", 0);
        res.wood.Ironwood = dict.GetValueOrDefault("wood_ironwood", 0);
        res.wood.Bloodwood = dict.GetValueOrDefault("wood_bloodwood", 0);
        
        res.ore.Copper = dict.GetValueOrDefault("ore_copper", 0);
        res.ore.Silver = dict.GetValueOrDefault("ore_silver", 0);
        res.ore.Gold = dict.GetValueOrDefault("ore_gold", 0);
        res.ore.Platinum = dict.GetValueOrDefault("ore_platinum", 0);
        
        res.food.Wheat = dict.GetValueOrDefault("food_wheat", 0);
        res.food.Rice = dict.GetValueOrDefault("food_rice", 0);
        res.food.Corn = dict.GetValueOrDefault("food_corn", 0);
        res.food.Potatoes = dict.GetValueOrDefault("food_potatoes", 0);
        
        res.herbs.Sage = dict.GetValueOrDefault("herbs_sage", 0);
        res.herbs.Rosemary = dict.GetValueOrDefault("herbs_rosemary", 0);
        res.herbs.Chamomile = dict.GetValueOrDefault("herbs_chamomile", 0);
        res.herbs.Valerian = dict.GetValueOrDefault("herbs_valerian", 0);

        return res;
    }

    public static HeroResources GenerateRandomResources()
    {
        var random = new System.Random();
        
        var res = new HeroResources();
        res.wood.Stick = random.Next(0, 300);
        res.wood.Lumber = random.Next(0, 200);
        res.wood.Ironwood = random.Next(0, 100);
        res.wood.Bloodwood = random.Next(0, 20);

        res.ore.Copper = random.Next(0, 150);
        res.ore.Silver = random.Next(0, 500);
        res.ore.Gold = random.Next(0, 80);
        res.ore.Platinum = random.Next(0, 10);

        res.food.Wheat = random.Next(0, 600);
        res.food.Corn = random.Next(0, 100);
        res.food.Rice = random.Next(0, 200);
        res.food.Potatoes = random.Next(0, 100);

        res.herbs.Sage = random.Next(0, 500);
        res.herbs.Rosemary = random.Next(0, 50);
        res.herbs.Chamomile = random.Next(0, 180);
        res.herbs.Valerian = random.Next(0, 120);

        return res;
    }
}