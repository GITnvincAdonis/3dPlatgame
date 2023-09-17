using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState
{
    protected Vector3 playerLocation;
    protected bool InSightRange;
    protected bool InAttackRange;
    protected Vector3 detectedPos; 
    protected RaycastHit slopeHit;
    public abstract void EnterState(EnemyStateMachine state);
    public abstract void UpdateState(EnemyStateMachine state);
    public abstract void CollisionEnter(EnemyStateMachine state, Collision collision);
    public abstract void TriggerStay(EnemyStateMachine state, Collider col);
    public abstract void TriggerEnter(EnemyStateMachine state, Collider col);
    protected void PositionDifference(EnemyStateMachine state)
    {
    
        playerLocation = detectedPos - new Vector3(state.CurrentPosition.position.x, detectedPos.y, state.CurrentPosition.position.z);
        Quaternion rot= Quaternion.LookRotation(playerLocation);
        state.CurrentPosition.rotation = Quaternion.Slerp(state.CurrentPosition.rotation, rot, state.SlerpSpeed);
        
    }
    protected void PlayerInRange(EnemyStateMachine state)
    {
    
        InSightRange = Physics.CheckSphere(state.transform.position, state.SightRange, state.Player);
        if(Physics.CheckSphere(state.transform.position, state.SightRange, state.Player)){
            Collider[] detectedPlayer = Physics.OverlapSphere(state.transform.position, state.SightRange, state.Player);
            foreach(Collider col in detectedPlayer){
            detectedPos = col.transform.position;
            }
        }
        
        InAttackRange = Physics.CheckSphere(state.transform.position, state.AttackRange, state.Player);
        
        //Debug.DrawWireSphere(state.transform.position, state.SightRange, Color.red);
        
        

    }
    
    protected bool OnSlope(EnemyStateMachine state)
    {
        if (Physics.Raycast(state.EnemyRigidBod.position, Vector3.down, out slopeHit, state.Height * 0.5f + 0.4f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < state.MaxSlopeAngle && angle != 0;
        }

        return false;

    }

    protected Vector3 GetSlopeMoveDir(EnemyStateMachine state)
    {
        Debug.Log(Vector3.ProjectOnPlane(state.CurrentPosition.forward, slopeHit.normal).normalized);
        return Vector3.ProjectOnPlane(state.CurrentPosition.forward, slopeHit.normal).normalized;
    }

}
