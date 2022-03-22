using System;
using Utility.Logging;

[Serializable]
public class Herbs 
{
	public int Sage;
	public int Rosemary;
	public int Chamomile;
	public int Valerian;

	public string SageId;
	public string RosemaryId;
	public string ChamomileId;
	public string ValerianId;

	public Herbs()
	{
		Sage = 0;
		Rosemary = 0;
		Chamomile = 0;
		Valerian = 0;
	}
	
	public void Log(ILog log)
	{
		log.Debug($"Sage: {Sage}");
		log.Debug($"Rosemary: {Rosemary}");
		log.Debug($"Chamomile: {Chamomile}");
		log.Debug($"Valerian: {Valerian}");
	}
}