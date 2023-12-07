using AmazingAssets.CurvedWorld.Example;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IDamagable
{
    [Header("Ссылки")]
    [SerializeField] GameController gameController;
    [SerializeField] SoundController soundController;
    [SerializeField] PlayerCanvasController canvasController;
    Animator animator;
    PlayerMovement playerMovement;
    [SerializeField] public WeaponMovementController weaponMovementController;
    [SerializeField] PlayerWeaponController weaponController;
    [SerializeField] Vector3 startCoordinates;
    [SerializeField] HealthSlider healthBar;
    [Header("Характеристики")]
    [SerializeField] float enemyDamage = 10;                     //Старт - 10
    [SerializeField] float moveSpeed = 70;                       //Старт - 70
    [SerializeField] float rotaionSpeed;                  //Старт - 230
    [SerializeField] float maxHealth = 3;       
    [SerializeField] float currentHealth;
    float weaponDamage = 1;
    [SerializeField] float immortalityInterval = 1.5f;             //Время неузвимости
    public bool isImmortal;
    float immortalityTimer;
    public bool isAlive;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        canvasController = GetComponentInChildren<PlayerCanvasController>();
    }
    void Start()
    {
        enemyDamage = Progress.Instance.playerInfo.playerDamage;
        rotaionSpeed = Progress.Instance.playerInfo.playerRotation;
        moveSpeed = Progress.Instance.playerInfo.playerSpeed;
        weaponMovementController.SetRotationSpeed(rotaionSpeed);       
        weaponController.SetEnemyDamage(enemyDamage);
        weaponController.SetWeaponDamage(weaponDamage);
        playerMovement.enabled = false;
        healthBar.SetSliderMaxValue(maxHealth);
        ResetHealth();
    }
    private void Update()
    {
        //Проверка на неуязвимость
        if(isImmortal)
        {
            immortalityTimer -= Time.deltaTime;
            if(immortalityTimer <= 0 )
                ChangeImmortalityState();
        }
    }

    public void RecieveDamage(float damageValue)
    {
        if (isImmortal || !isAlive)
            return;

        currentHealth -= damageValue;
        canvasController.ShowDamageValueText(damageValue);
        healthBar.ChangeCurrentHealthValue(currentHealth);
        soundController.Play("PlayerTakeDamage");
        if (currentHealth == 0)
        {
            StartCoroutine(DeathProccess());
            return;
        }
        ChangeImmortalityState();
    }
    IEnumerator DeathProccess()
    {
        playerMovement.enabled = false;
        animator.SetFloat("speed", 0);
        animator.SetBool("isDeath", true);
        isAlive = false;

        EventManager.InvokePlayerDied();
        healthBar.gameObject.SetActive(false);
        weaponController.ShowWeaponModel(false);
        yield return new WaitForSeconds(2.5f);
        gameController.EndLevel(false);
        yield return null;
    }
    //Восстановление персонажа
    public void ResetPlayer()
    {       
        transform.position = Vector3.zero;
        animator.SetBool("isDeath", false);
        animator.SetFloat("speed", 0);
        ResetHealth();
        GetComponent<BoxCollider>().enabled = true;
        weaponMovementController.ResetWeapon();
        weaponController.ShowWeaponModel(true);
        isImmortal = false;
    }
    public void ResetHealth()
    {
        isAlive = true;
        healthBar.gameObject.SetActive(true);
        currentHealth = maxHealth;
        healthBar.ResetSlider();
    }
    void ChangeImmortalityState()
    {
        isImmortal = !isImmortal;
        if (isImmortal)
        {
            immortalityTimer = immortalityInterval;
        }
        animator.SetBool("isImmortal", isImmortal);
    }

    //Скорость вращения оружия
    public void ChangeRotationSpeed(float rotationSpeedDiff)
    {
        rotaionSpeed += rotationSpeedDiff;
        Progress.Instance.playerInfo.playerRotation = rotaionSpeed;
        YandexSDK.Save();
        weaponMovementController.SetRotationSpeed(rotaionSpeed);
    }
    //Скорость перемещения
    public float GetMovementSpeed()
    {
        return moveSpeed;
    }
    public void ChangeMovementSpeed(float moveSpeedDiff)
    {
        moveSpeed += moveSpeedDiff;
        Progress.Instance.playerInfo.playerSpeed = moveSpeed;
        YandexSDK.Save();
        playerMovement.SetMovementSpeed(moveSpeed);
    }
    //Урон по противнику
    public float GetEnemyDamage()
    {
        return enemyDamage;
    }
    public void ChangeEnemyDamage(float damageDiff)
    {
        enemyDamage += damageDiff;
        Progress.Instance.playerInfo.playerDamage = enemyDamage;
        YandexSDK.Save();
        weaponController.SetEnemyDamage(enemyDamage);
    }
    //урон по оружию
    public float GetWeaponDamage()
    {
        return weaponDamage;
    }

}
