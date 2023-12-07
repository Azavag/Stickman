using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyType
{ 
   Regular,
   Miniboss,
   Boss
}

public class EnemyController : MonoBehaviour, IDamagable
{
    [Header ("Ссылки моба")]
    [SerializeField] protected Transform playerTransform;
    EnemyCanvasController canvasController;
    protected NavMeshAgent agent;
    protected Rigidbody rb;
    protected Animator animator;
    [SerializeField] protected AnimationClip deathClip;
    [SerializeField] protected GameObject attackControllerZone;
    SoundController soundController;
    EnemyAttackController attackController;
    [Header("Характеристики")]
    [SerializeField] EnemyType enemyType;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float startAgentSpeed;
    [SerializeField] protected float damage;
    [Header("Состояния")]
    protected bool isFollowPlayer;
    protected bool underGround;
    protected bool isAlive;
    protected bool isDeathAnimationEnd;
    protected Vector3 agentTarget;
    float spawnSpeed = 2f;
    bool invicible;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        canvasController = GetComponentInChildren<EnemyCanvasController>();
        attackController = attackControllerZone.GetComponent<EnemyAttackController>();
        soundController = FindObjectOfType<SoundController>();

        EventManager.PlayerDied += OnPlayerDied;
        EventManager.GameReseted += OnGameReseted;
    }
    protected virtual void Start()
    {        
        agent.speed = startAgentSpeed;
        isAlive = true;
        agent.enabled = false;
        NavmeshAgentState(false);
        attackControllerZone.SetActive(false);
    }  
    void Update()
    {
        if(underGround)
            CheckGround();
        else animator.SetFloat("speed", agent.velocity.sqrMagnitude);     
    }  
    private void FixedUpdate()
    {
        if (isDeathAnimationEnd)
            MoveUnderground();
        if (!isAlive || invicible)
            return;
        //Движение к игроку по NavMesh на поверхности
        if (isFollowPlayer)
        {
            SetTarget(playerTransform.position);
            agent.SetDestination(agentTarget);
        }
        //Движение к игроку под землёй
        if (underGround)
            RiseToGround();            
    }
    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }
    public virtual void RecieveDamage(float damageValue)
    {
        if (invicible)
            return;
        currentHealth -= damageValue;
        if (StaticSettings.CanShowDamageNumber())
            canvasController.ShowDamageValueText(damageValue);
        AnimateDamage();
        soundController.Play("EnemyTakeDamage");
        if (currentHealth <= 0 && isAlive)
        {
            invicible = true;
            StartCoroutine(DeathProccess());
            return;
        }
        
    }
    public virtual IEnumerator DeathProccess()
    {
        DisableComponents();               
        animator.SetTrigger("death");
        yield return new WaitForSeconds(deathClip.length);
        isDeathAnimationEnd = true;
    }
    public void AnimateDamage()
    {
        animator.SetTrigger("damaged");
    }
    protected void DisableComponents()
    {
        NavmeshAgentState(false);
        isAlive = false;
        agent.enabled = false;
        GetComponent<Collider>().enabled = false;
        EventManager.InvokeEnemyTypeDied(enemyType);
        EventManager.InvokeEnemyDied();
        Destroy(gameObject, 2 * deathClip.length);
    }
    protected void RiseToGround()
    {
        Vector3 movementVector = Vector3.up;
        rb.MovePosition(transform.position + movementVector * spawnSpeed * Time.deltaTime);
    }
    protected virtual void CheckGround()
    {
        if (transform.position.y >= 0)
        {
            UnderGround(false);
            agent.enabled = true;
            NavmeshAgentState(true);
            animator.SetTrigger("onGround");
            if(enemyType == EnemyType.Regular)
                attackControllerZone.SetActive(true);
        }
    }
    public void UnderGround(bool state)
    {
        underGround = state;
    }
    protected void MoveUnderground()
    {
        Vector3 movementVector = Vector3.down;
        rb.MovePosition(transform.position + movementVector * 0.5f * spawnSpeed * Time.deltaTime);
    }
    public void SetTargetTransform(Transform player)
    {
        playerTransform = player;
    }
    public float GetDamage()
    {
        return damage;
    }
    protected void SetTarget(Vector3 taregtVector)
    {
        agentTarget = taregtVector;
    }
    public void NavmeshAgentState(bool state)
    {
        isFollowPlayer = state;
        if (state)
            agent.speed = startAgentSpeed;
        else agent.speed = 0;
    }
    //В анимации EnemyAttackAnimation
    public void AttackPlayer()
    {
        if (invicible)
            return;
        attackController.TryAttackPlayer(damage);
    }
    //В анимации EnemyAttackAnimation
    public void CheckAttackEnd()
    {
        attackController.StopAttackAnimation();
    }
    void OnPlayerDied()
    {
        attackController.SetPlayerInZone(false);
        invicible = true;
        NavmeshAgentState(false);
    }
    protected virtual void OnGameReseted()
    {
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        EventManager.PlayerDied -= OnPlayerDied;
        EventManager.GameReseted -= OnGameReseted;
    }
}
