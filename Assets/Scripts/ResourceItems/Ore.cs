using System;
using Utility.Logging;

[Serializable]
public class Ore 
{
	public int Copper;
	public int Silver;
	public int Gold;
	public int Platinum;

	public string CopperId;
	public string SilverId;
	public string GoldId;
	public string PlatinumId;

	public Ore()
	{
		Copper = 0;
		Silver = 0;
		Gold = 0;
		Platinum = 0;
	}
	
	public void Log(ILog log)
	{
		log.Debug($"Copper: {Copper}");
		log.Debug($"Silver: {Silver}");
		log.Debug($"Gold: {Gold}");
		log.Debug($"Platinum: {Platinum}");
	}
}