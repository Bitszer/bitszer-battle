using System;
using Utility.Logging;

[Serializable]
public class Wood 
{
	public int Stick;
	public int Lumber;
	public int Ironwood;
	public int Bloodwood;
	
	public Wood()
	{
		Stick = 0;
		Lumber = 0;
		Ironwood = 0;
		Bloodwood = 0;
	}

	public void Log(ILog log)
	{
		log.Debug($"Stick: {Stick}");
		log.Debug($"Lumber: {Lumber}");
		log.Debug($"Ironwood: {Ironwood}");
		log.Debug($"Bloodwood: {Bloodwood}");
	}
}