using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName = "Scriptable Objects/MissileData")]
public class MissileMovement : ScriptableObject
{
    
    public float turnSpeed;
    public float forwardForce;
    public float deathSpan;
    public LayerMask whatIsPlayer;
    public float sightRange;
    
}
