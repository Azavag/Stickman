using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("—сылки")]
    [SerializeField] WeaponMovementController weaponMovementController;
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject weaponCollisionParticles;
    [SerializeField] SoundController soundController;
    float enemyDamage;
    float weaponDamage;
    float untouchTime = 0.3f;
    float untouchTimer;
    bool isWeaponUntouchable;

    void Start()
    {
        isWeaponUntouchable = false;
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
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Boss"))
        {
            if (other.gameObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.RecieveDamage(enemyDamage);
            }          
        }
        
        if(other.gameObject.CompareTag("EnemyWeapon") && !isWeaponUntouchable)
        {
            var collisionPoint = other.ClosestPoint(transform.position);
            weaponCollisionParticles.transform.position = collisionPoint;
            PlayWeaponCollisionParticle();
            soundController.Play("WeaponsHit");
            if (other.gameObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.RecieveDamage(weaponDamage);
            }      
            
            weaponMovementController.SwapWeaponDirection();
            isWeaponUntouchable = true;
        }       
    }
    void PlayWeaponCollisionParticle()
    {
        for(int i = 0; i < weaponCollisionParticles.transform.childCount; i++)
        {
            weaponCollisionParticles.transform.GetChild(i).GetComponent<ParticleSystem>().Play();          
        }        
    }
    public void SetEnemyDamage(float enemeyDmg)
    {
        enemyDamage = enemeyDmg; 
    }
    public void SetWeaponDamage(float weapongDmg)
    {
        weaponDamage = weapongDmg;
    }

    public void ShowWeaponModel(bool state)
    {
        gameObject.SetActive(state);
    }

}
