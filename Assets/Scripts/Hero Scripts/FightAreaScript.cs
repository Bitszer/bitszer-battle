using UnityEngine;

public class FightAreaScript : MonoBehaviour
{
	/*
	 * Mono Behavior.
	 */
	
	private void OnTriggerEnter2D(Collider2D hit)
	{
		if (hit.CompareTag("enemy") || hit.CompareTag("Player"))
			hit.GetComponentInParent<HeroController>().OnEnteredTowerArea();
	}
}