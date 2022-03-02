using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class Tower : MonoBehaviour
{
    [Header("Properties")] 
    [SerializeField] private bool isEnemyTower = false;
    
    [Header("Components")] 
    [SerializeField] private Image healthFillBar = null;
    [SerializeField] private Sprite[] TowerSprites = null;
    
    public bool IsDestroyed { get; private set; }

    private GameOver       _gameOver;
    private SpriteRenderer _spriteRenderer;
    
    private float _health;
    private float _healthMax;
    
    private readonly List<Guid> _damageReceivedFromInThisFrame = new List<Guid>();
    
    /*
     * Mono Behavior.
     */

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = TowerSprites[0];
        
        _gameOver = GameObject.Find("GameEndBounds").GetComponent<GameOver>();
    }

    private void Update()
    {
        _damageReceivedFromInThisFrame.Clear();
    }

    /*
     * Public.
     */

    public void SetTowerHealth(float health)
    {
        _healthMax = health;
        _health = health;
    }

    public void ResetTower()
    {
        IsDestroyed = false;
        
        if (_spriteRenderer != null)
            _spriteRenderer.sprite = TowerSprites[0];
        
        healthFillBar.fillAmount = 1;
    }
    
    public void ReceiveDamage(HeroController damageSource)
    {
        if (_damageReceivedFromInThisFrame.Contains(damageSource.Guid))
            return;
        
        _damageReceivedFromInThisFrame.Add(damageSource.Guid);
        
        var damage = damageSource.Damage;
        
        if (_health <= 0) 
            return;
        
        if (damageSource.IsRangeHero)
            SoundManager.Instance.PlayOneShot(SoundManager.SoundState.ArrowDamage, 0.3f);

        _health -= damage;

        var healthPercentage = _health / _healthMax * 100; 
        if (healthPercentage > 90) _spriteRenderer.sprite = TowerSprites[0];
        else if (healthPercentage > 75) _spriteRenderer.sprite = TowerSprites[1];
        else if (healthPercentage > 60) _spriteRenderer.sprite = TowerSprites[2];
        else if (healthPercentage > 45) _spriteRenderer.sprite = TowerSprites[3];
        else if (healthPercentage > 30) _spriteRenderer.sprite = TowerSprites[4];
        else if (healthPercentage >  0) _spriteRenderer.sprite = TowerSprites[5];
        else if (healthPercentage <= 0) _spriteRenderer.sprite = TowerSprites[6];

        if (_health > 0)
        {
            healthFillBar.fillAmount = ClampRange.Clamp(0, _healthMax, 0, 1, _health);
            return;
        }
        
        healthFillBar.fillAmount = 0;
        IsDestroyed = true;
        GameOver();
    }
    
    /*
     * Triggers.
     */
    
    private void OnTriggerEnter2D(Collider2D _hit)
    {
        if (_hit.CompareTag("rangeweapon"))
            ReceiveDamage(_hit.transform.parent.GetComponent<HeroController>());
    }
    
    /*
     * Game Over.
     */
    
    private void GameOver()
    {
        CentralVariables.IS_GAME_END = true; // Tell Globally that the Game is Over.

        if (CentralVariables.EnemyUnitInstances.Count > 0 || CentralVariables.PlayerUnitsInstances.Count > 0)
        {
            if (isEnemyTower)
            {
                CentralVariables.IS_PLAYER_WIN = true;
                for (var index = 0; index < CentralVariables.EnemyUnitInstances.Count; index++)
                    CentralVariables.EnemyUnitInstances[index].GetComponent<HeroController>().DeathAnimatio();
            }
            else
            {
                CentralVariables.IS_PLAYER_WIN = false;
                for (var index = 0; index < CentralVariables.PlayerUnitsInstances.Count; index++)
                    CentralVariables.PlayerUnitsInstances[index].GetComponent<HeroController>().DeathAnimatio();
            }
        }

        _gameOver.PlayGameOverAnimation();
    }
}