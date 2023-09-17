using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (menuName = "Scriptable Objects/Enemy Data")]
public class EnemyType1 : ScriptableObject
{

   

    public float health;
    public float knockBackForce;
    public int attackRange;
    public int sightRange;
    public float slerpSpeed;
    public float moveSpeed;
    public float attackLungeDistance;
    public float entHeight;
    public float maxSlopeAngle;
    public float lightAttackDamage;
    public float HeavyAttackDamage;



}
