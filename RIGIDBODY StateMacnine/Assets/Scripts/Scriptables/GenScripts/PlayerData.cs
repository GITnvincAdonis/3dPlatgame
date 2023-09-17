using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Player Data")]
public class PlayerData : ScriptableObject
{


    //player mechanics
    public float health;
    public float stamina;
    public float lightAttackDamage;
    public float HeavyAttackDamage;
    public float LightAttackDist;
    public float HeavyAttackDist;
    public float knockBackForce;
    public int  attackRange;
    public int  sightRange;

    public float bulletStamina;
    public float bulletSpeed;


    //rigidbody manipulation
    public float groundDrag;
    public float airDrag;
    
    public float slerpSpeed;
    public float moveSpeed;
    
    public float entHeight;
    public float maxSlopeAngle;
    public float flightStamina;
    public float jumpTime;
    public float fallMultiplier;
    public float jumpMultiplier;

    //layers
    public LayerMask whatIsBulletBreakable;
    public LayerMask whatIsGround;
    public LayerMask whatIsEnemy;
    public LayerMask whatIsBulletSwitches;
    public LayerMask whatIsInteractable;
    

    





}
