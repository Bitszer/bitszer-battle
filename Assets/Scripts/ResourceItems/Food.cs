using System;
using Utility.Logging;

[Serializable]
public class Food 
{
	public int Wheat;
	public int Rice;
	public int Corn;
	public int Potatoes;

	public string WheatId;
	public string RiceId;
	public string CornId;
	public string PotatoesId;

	public Food()
	{
		Wheat = 0;
		Rice = 0;
		Corn = 0;
		Potatoes = 0;
	}
	
	public void Log(ILog log)
	{
		log.Debug($"Wheat: {Wheat}");
		log.Debug($"Rice: {Rice}");
		log.Debug($"Corn: {Corn}");
		log.Debug($"Potatoes: {Potatoes}");
	}
}