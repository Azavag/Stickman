using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeaponController : MonoBehaviour, IDamagable
{
    [Header("Ссылки")]
    [SerializeField] WeaponMovementController weaponMovementController;
    [SerializeField] BossController bossController;
    [SerializeField] GameObject weaponModel;
    Animator animator;
    //----Характеристики----//
    float damage;
    float maxDurability;
    float currentDurability;
    float untouchTime = 0.3f;
    float untouchTimer;
    bool isWeaponUntouchable;
    [SerializeField] Mesh[] bossWeaponsMeshArray;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        maxDurability = bossController.GetWeaponDurability();

        currentDurability = maxDurability;
        damage = bossController.GetDamage();
        isWeaponUntouchable = false;
        WearRandomWeapon();
    }

    void WearRandomWeapon()
    {
        int randomNumber = Random.Range(0, bossWeaponsMeshArray.Length);
        weaponModel.GetComponent<MeshFilter>().mesh = bossWeaponsMeshArray[randomNumber];        
    }

    private void FixedUpdate()
    {
        if (isWeaponUntouchable)
        {
            untouchTimer += Time.fixedDeltaTime;
            if (untouchTimer >= untouchTime)
            {
                isWeaponUntouchable = false;
                untouchTimer = 0;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {       
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.RecieveDamage(damage);
            }
        }       
        if (other.gameObject.CompareTag("Weapon") && !isWeaponUntouchable)
        {
            weaponMovementController.SwapWeaponDirection();
            isWeaponUntouchable = true;
        }
    }
    public void RecieveDamage(float damageValue)
    {       
        currentDurability -= damageValue;
        animator.SetTrigger("damaged");
        bossController.AnimateDamage();
        if (currentDurability <= 0) 
        {
            bossController.ChangeImmotralState(false);
            bossController.ResetWeaponState(true);
            ShowWeaponModel(false);
            bossController.ShowUnweaponText();
        }
    }
    public void ShowWeaponModel(bool state)
    {
        gameObject.SetActive(state);
        GetComponent<Collider>().enabled = state;
    }
    public void ResetDurablity()
    {
        currentDurability = maxDurability;
    }
    public void SetDurability(float durability)
    {
        maxDurability = durability;
        currentDurability = durability;
    }
}
