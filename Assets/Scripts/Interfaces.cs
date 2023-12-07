using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDamagable
{
    void RecieveDamage(float damageValue);

}
interface IKnockbackable
{
    void RecieveKnockback(float knockbackValue);

}

