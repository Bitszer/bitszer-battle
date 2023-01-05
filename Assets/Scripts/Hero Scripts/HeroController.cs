using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class HeroController : MonoBehaviour
{
    [Header("Core")]
    [SerializeField] private int    id          = 0;
    [SerializeField] private string typeName    = null;
    [SerializeField] private string description = null;

    [Header("Stats")]
    [SerializeField] private float health       = 0;
    [SerializeField] private float damage       = 0;
    [SerializeField] private float heroSpeed    = 1;
    [SerializeField] private float coolDownTime = 0;
    [SerializeField] private int   spawnCost    = 0;
    [SerializeField] private float attackRange  = 2;
    [SerializeField] private bool  isRangeHero  = false;
    [SerializeField] private bool  isFlyingHero = false;

    [Header("Components")]
    [SerializeField] public Animator BloodAnimController;
    [SerializeField] public GameObject HealthBar;

    public int    Id          => id;
    public Guid   Guid        { get; private set; }
    public string TypeName    => typeName;
    public string Description => description;

    public int   Level       => level;
    public float Health      => health;
    public float Damage      => damage;
    public float Speed       => heroSpeed;
    public float CoolDown    => coolDownTime;
    public int   SpawnCost   => spawnCost;
    public bool  IsRangeHero => isRangeHero;

    public bool IsEnemySelected => Enemy != null;

    [NonSerialized] private int  level = 0;
    [NonSerialized] public  bool isEnemyCharacter;

    [NonSerialized] public bool isDead       = false;
    [NonSerialized] public bool isGotByEnemy = false;

    private readonly List<Guid> _damageReceivedFromInThisFrame = new List<Guid>();

    private PlayerHeroManager _playerHeroManager;
    
    private Animator   _animationController;
    private GameObject RangeWeapon;
    
    private bool coroutineCheck = false;
    
    private int _direction;
    
    private bool         isFighting                     = false;
    private bool         isHealthVisible                = false;
    private bool         isEnemyHereBeforeWalkingStarts = false;
    private GameObject   Enemy;
    private Collider2D[] enemyCols = new Collider2D[10];
    private float        initialHealth;
    private bool         _isInTempleArea  = false;
    private bool         fightArea        = false;
    private bool         isAttackingTower = false;

    private HeroController _heroController => Enemy != null ? Enemy.transform.parent.GetComponent<HeroController>() : null;

    private bool _ultimateDamageReceivingBlocked;

    /*
     * Mono Behavior.
     */

    private void Start()
    {
        Guid = Guid.NewGuid();

        _playerHeroManager = FindObjectOfType<GameManager>().playerHeroManager;
        _animationController = GetComponent<Animator>();

        _isInTempleArea = false;

        if (isRangeHero)
        {
            if (isEnemyCharacter)
                GetComponentInChildren<BoxCollider2D>().enabled = false;

            for (int i = 0; i < transform.childCount; i++)
                RangeWeapon = transform.GetChild(i).gameObject;
        }

        initialHealth = health;

        isGotByEnemy = false;
        
        _direction = isEnemyCharacter ? -1 : 1;
    }

    private void Update()
    {
        if (!fightArea)
        {
            if (_isInTempleArea)
            {
                GetComponentInChildren<BoxCollider2D>().enabled = true;
                fightArea = true;
            }
        }

        if (!isFighting)
        {
            if (isEnemyHereBeforeWalkingStarts)
            {
                Fight();
                isEnemyHereBeforeWalkingStarts = false;
            }

            UpdateMove();
        }
        
        _damageReceivedFromInThisFrame.Clear();
    }

    /*
     * Public.
     */

    public void OnEnteredTowerArea()
    {
        _isInTempleArea = true;
    }

    /*
     * Movement.
     */
    
    private void UpdateMove()
    {
        transform.Translate(Vector2.right * _direction * heroSpeed * Time.deltaTime, 0);
    }
    
    public void RecieveDamage(float damageReceved, string from = null)
    {
        // if (from != null)
            // _log.Debug(name + "(" + Guid + ") damage => " + damageReceved + " from: " + from);
        
        health -= damageReceved;

        if (health <= 0)
        {
            isGotByEnemy = false;
            
            var localScale = HealthBar.transform.localScale;
            localScale = new Vector3(0, localScale.y, localScale.z);
            HealthBar.transform.localScale = localScale;
            
            DeathAnimatio();
        }
        else
        {
            BloodAnimController.SetTrigger("hit");
            float val = ClampRange.Clamp(initialHealth, 0, 1.06f, 0, health);

            if (!isHealthVisible)
            {
                isHealthVisible = true;
                HealthBar.transform.parent.gameObject.SetActive(true);
            }

            if (health > 0)
                HealthBar.transform.localScale = new Vector3(val, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
        }
    }

    // Plays the DeathAnimation
    public void DeathAnimatio()
    {
        isFighting = true;
        this.transform.Find("AttackArea").GetComponent<BoxCollider2D>().enabled = false;
        isDead = true;
        _animationController.SetBool("isDead", true);
        Invoke("DestroyHero", 0.8f);
    }

    // If The HP gets Zero Hero Destoys itself.
    private void DestroyHero()
    {
        if (isEnemyCharacter)
        {
            CentralVariables.EnemyMax--;
            CentralVariables.EnemyUnitInstances.Remove(this);
        }
        else
        {
            CentralVariables.PlayerMax--;
            CentralVariables.PlayerUnitsInstances.Remove(this);
        }

        Destroy(gameObject);
    }
    
    public void SetStats(HeroUnitData data)
    {
        level = data.Level;
        health = data.Health;
        damage = data.Damage;
        heroSpeed = data.Speed;
        coolDownTime = data.CoolDown;
    }
    
    /*
     * WARNING!
     * Do not remove. It's used for melee heroes attack.
     */
    public void CauseDamage()
    {
        if (!isAttackingTower && _heroController != null)
        {
            SoundManager.Instance.PlayOneShot(SoundManager.SoundState.SwordDamage, 0.3f);
            _heroController.RecieveDamage(damage);
        }

        if (isAttackingTower && _enemyTower != null && !_enemyTower.IsDestroyed)
        {
            SoundManager.Instance.PlayOneShot(SoundManager.SoundState.SwordDamage, 0.3f);
            _enemyTower.ReceiveDamage(this);
        }
    }

    private Tower _enemyTower;

    // Gives Damage to Oponent.
    IEnumerator StartAttacking()
    {
        coroutineCheck = true;
        if (_heroController != null && Enemy != null)
        {
            while (!_heroController.isDead && !CentralVariables.IS_GAME_END)
            {
                if (!isRangeHero)
                {
                    yield return new WaitForSeconds(0.6f);
                }
                else
                {
                    yield return null;
                }
            }
        }

        Enemy = null;
        enemyCols = null;
        isFighting = false;
        Invoke(nameof(Fight), 0.1f);
        coroutineCheck = false;
    }

    IEnumerator StartAttackingTower(GameObject towerGameObject)
    {
        isAttackingTower = true;
        coroutineCheck = true;
        
        _enemyTower = towerGameObject.GetComponent<Tower>();

        while (!_enemyTower.IsDestroyed && !CentralVariables.IS_GAME_END)
            yield return new WaitForSeconds(0.6f);

        EndAttackAnimation();
        isFighting = false;
        coroutineCheck = false;
        isAttackingTower = false;
        _enemyTower = null;
    }

    // ---------------------------------------------------------------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (isEnemyCharacter)
        {
            if (hit.CompareTag("power"))
                ReceiveUltimateDamage();

            if (hit.CompareTag("Player") && !isFighting)
                Fight();

            if (hit.CompareTag("playertower") && !isFighting)
                DestroyTower(hit.gameObject);
        }
        else
        {
            if (hit.CompareTag("enemy") && !isFighting)
                Fight();

            if (hit.CompareTag("enemytower") && !isFighting)
                DestroyTower(hit.gameObject);
        }
        
        if (hit.CompareTag("rangeweapon"))
            ReceiveRangeDamage(hit.transform.parent.GetComponent<HeroController>(), hit.gameObject);
    }

    private void ReceiveUltimateDamage()
    {
        // Needed to compensate Unity collider isTrigger property issue.
        if (_ultimateDamageReceivingBlocked)
            return;

        _ultimateDamageReceivingBlocked = true;
        StartCoroutine(UltimateDamageBlockingCooldown());
        RecieveDamage(_playerHeroManager.ultimate.damage, "ultimate");
    }

    private IEnumerator UltimateDamageBlockingCooldown()
    {
        _ultimateDamageReceivingBlocked = true;
        yield return new WaitForSeconds(2.0F);
        _ultimateDamageReceivingBlocked = false;
    }

    private void ReceiveRangeDamage(HeroController sender, GameObject hitGameObject)
    {
        if (!isGotByEnemy)
            return;
        
        // Needed to compensate Unity collider issue with isTrigger property.
        if (_damageReceivedFromInThisFrame.Contains(sender.Guid))
            return;
        
        _damageReceivedFromInThisFrame.Add(sender.Guid);
        
        SoundManager.Instance.PlayOneShot(SoundManager.SoundState.ArrowDamage, 0.3f);

        RecieveDamage(sender.damage);
        
        if (!sender.isFlyingHero)
            hitGameObject.SetActive(false);
    }

    private void DestroyTower(GameObject tower)
    {
        if (CentralVariables.IS_GAME_END)
            return;
        
        StartAttackAnimation();
        isFighting = true;
        //float _towerHealth = tower.GetComponent<Tower>().TowerHealth;
        if (!coroutineCheck)
            StartCoroutine(StartAttackingTower(tower));
        
    }

    // Start Attacking Animation
    private void StartAttackAnimation()
    {
        if (isRangeHero)
        {
            _animationController.SetBool("isRangeAttack", true);
        }
        else
        {
            _animationController.SetBool("isAttacking", true);
        }
    }

    // Ends Attacking Animation
    private void EndAttackAnimation()
    {
        if (isRangeHero)
        {
            _animationController.SetBool("isRangeAttack", false);
        }
        else
        {
            _animationController.SetBool("isAttacking", false);
        }
    }

    // Starts Fighting
    private void Fight()
    {
        if (!CentralVariables.IS_GAME_END)
        {
            GetEnemyForFight();
            if (IsEnemySelected)
            {
                isEnemyHereBeforeWalkingStarts = true;
                StartAttackAnimation();
                isFighting = true;
                if (!coroutineCheck)
                    StartCoroutine(StartAttacking());
            }
            else
            {
                EndAttackAnimation();
                isFighting = false;
            }
        }
        else
        {
            EndAttackAnimation();
            isFighting = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


    // Cast a Ray from Center to Find if there is any enemy inside the attack area
    private void GetEnemyForFight()
    {
        // EnemyCols List of gameobject will get all the enemies inside it's attack area.
        enemyCols = Physics2D.OverlapCircleAll(gameObject.transform.position, attackRange);

        // Get one enemy from the list.
        for (int i = 0; i < enemyCols.Length; i++)
        {
            if (enemyCols[i].transform.parent.name != this.name)
            {
                if (isEnemyCharacter)
                {
                    if (enemyCols[i] != null && enemyCols[i].CompareTag("Player"))
                    {
                        Enemy = enemyCols[i].gameObject;
                        if (isRangeHero && _heroController != null)
                            _heroController.isGotByEnemy = true;
                        break;
                    }
                }
                else
                {
                    if (enemyCols[i] != null && enemyCols[i].CompareTag("enemy"))
                    {
                        Enemy = enemyCols[i].gameObject;
                        if (isRangeHero && _heroController != null)
                            _heroController.isGotByEnemy = true;
                        break;
                    }
                }
            }
        }
    }

    /*
     * DO NOT REMOVE!
     * Called implicitly.
     */
    public void ReloadTheWeapon()
    {
        if (isRangeHero)
            RangeWeapon.SetActive(true);
    }
}