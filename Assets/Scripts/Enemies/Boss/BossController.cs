using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossController : EnemyController, IDamagable
{
    [Header("Характеристики босса")]
    [SerializeField] float weaponDurability;
    bool isImmortal;
    float runAwayResetInterval = 5;
    bool isRunAway;
    float runAwayResetTimer = 0.5f;
    [SerializeField] float runAwaySpeed;
    [SerializeField] float weaponRotationSpeed;
    [Header("Побег босса")]
    [SerializeField] bool canRunAway;
    float changeDirectionInterval = 0.75f;    // Интервал смены направления
    [SerializeField] float minAngle;
    [SerializeField] float maxAngle;
    float timeSinceLastDirectionChange;
    Vector3 randomDirection;
    float runRotationSpeed = 400; 
    [Header("Ссылки босса")]
    [SerializeField] GameObject weaponObject;
    [SerializeField] BossWeaponController weaponController;
    [SerializeField] WeaponMovementController weaponMovementController;
    [SerializeField] AnimationClip stunClip;
    BossCanvasController bossCanvasController;

    protected override void Awake()
    {
        base.Awake();
        bossCanvasController = GetComponentInChildren<BossCanvasController>();
        weaponController.ShowWeaponModel(false);
    }
    protected override void Start()
    {
        base.Start();
        ChangeImmotralState(true);
        UnderGround(true);
        timeSinceLastDirectionChange = changeDirectionInterval;        
    }

    void Update()
    {
        if (!isAlive)
            return;
        if (underGround)
            CheckGround();
        else animator.SetFloat("speed", agent.velocity.sqrMagnitude);

        if (isRunAway)
            animator.SetFloat("speed", 1f);
    }
    private void FixedUpdate()
    {
        if (!isAlive && !isDeathAnimationEnd)
            return;
        if (isDeathAnimationEnd)
        {
            MoveUnderground();
            return;
        }    
        //Подъём
        if (underGround)
            RiseToGround();
        // Движение к игроку по NavMesh на поверхности
        if (isFollowPlayer)
        {
            SetTarget(playerTransform.position);
            agent.SetDestination(agentTarget);
        }
        if (canRunAway && isRunAway)
        {
            RunAwayChangeDirectionTimer();
            RunAwayResetTimer();
            RunAwayFromPlayer();
        }    
    }
    public void ResetWeaponState(bool resetState)
    {     
        if (!canRunAway)
            return;

        NavmeshAgentState(!resetState);
        if (resetState == true)
        {
            StartCoroutine(StunProccess());
        }
    }


    protected override void CheckGround()
    {
        base.CheckGround();
        if(!underGround)
            weaponController.ShowWeaponModel(true);
    }
    public void SetAntiPlayerDirection()
    {
        weaponMovementController.SetAntiDirection(playerTransform.GetComponent<PlayerController>()
            .weaponMovementController.GetRotationDirection());
    }
    IEnumerator StunProccess()
    {
        NavmeshAgentState(false);
        animator.SetBool("stun", true);
        yield return new WaitForSeconds(stunClip.length);       
        animator.SetBool("stun", false);
        isRunAway = true;
    }
    void RunAwayFromPlayer()
    {
        rb.MovePosition(rb.position + randomDirection * runAwaySpeed * Time.fixedDeltaTime);
        
        if (randomDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(randomDirection);
            targetRotation.x = 0f;
            targetRotation.z = 0f;
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, runRotationSpeed * Time.fixedDeltaTime));
        }
    }   
    void RunAwayChangeDirectionTimer()
    {      
        timeSinceLastDirectionChange += Time.deltaTime;
        // Если прошло достаточно времени, сменяем направление
        if (timeSinceLastDirectionChange >= changeDirectionInterval)
        {
            GetRandomDirection();
            timeSinceLastDirectionChange = 0.0f;
        }
    }
    void GetRandomDirection()
    {       
        float randomAngle = Random.Range(minAngle, maxAngle);       // случайное отклонение в градусах 
        Vector3 directionToReferencePoint = transform.position - playerTransform.position;
        Quaternion randomRotation = Quaternion.Euler(0, randomAngle, 0);
        randomDirection = randomRotation * directionToReferencePoint;
        randomDirection.y = 0f;
        randomDirection.Normalize();         
    }
    void RunAwayResetTimer()
    {
        runAwayResetTimer += Time.deltaTime;
        if (runAwayResetTimer >= runAwayResetInterval)
        {
            ResetWeaponState(false);
            attackControllerZone.SetActive(true);
            isRunAway = false;
            runAwayResetTimer = 0.0f;
        }
    }
    public override void RecieveDamage(float damageValue)
    {
        if (isImmortal)
        {
            bossCanvasController.ShowImmortalText();
            return;
        }
        base.RecieveDamage(damageValue);
    }
    public override IEnumerator DeathProccess()
    {       
        DisableComponents();
        animator.SetTrigger("death");
        Destroy(weaponObject);
        
        yield return new WaitForSeconds(deathClip.length);
        isDeathAnimationEnd = true;
        yield return null;        
    }
    protected override void OnGameReseted()
    {
        base.OnGameReseted();
        Destroy(weaponObject);
    }   
    public void ChangeImmotralState(bool state)
    {
        isImmortal = state;
    }
    public void ShowUnweaponText()
    {
        bossCanvasController.ShowUnweaponText();
    }    
    public float GetWeaponDurability()
    {
        return weaponDurability;
    }
    public void SetRotationSpeed(float rotatinSpeed)
    {
        weaponRotationSpeed = rotatinSpeed;
        weaponMovementController.SetRotationSpeed(weaponRotationSpeed);
    }

    public void SetWeaponDurablity(float weaponDurability)
    {
        this.weaponDurability = weaponDurability;      
        weaponController.SetDurability(weaponDurability);
    }
}
