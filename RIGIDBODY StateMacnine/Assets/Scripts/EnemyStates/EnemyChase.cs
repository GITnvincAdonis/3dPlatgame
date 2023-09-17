using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : EnemyBaseState
{
    float totDist;
    public override void EnterState(EnemyStateMachine state) {
        //Debug.Log("in chase state");
     }
    public override void UpdateState(EnemyStateMachine state) {
        PositionDifference(state);
        if(OnSlope(state)){
            state.EnemyRigidBod.AddForce(GetSlopeMoveDir(state) * state.MoveSpeed, ForceMode.Force);
        }
        else{
            state.EnemyRigidBod.AddForce(state.CurrentPosition.forward * state.MoveSpeed, ForceMode.Force);
        }
        
        PlayerInRange(state);
        CheckSwitch(state);
        //Debug.Log(OnSlope(state));

    }
    public override void CollisionEnter(EnemyStateMachine state, Collision collision) { }
    public override void TriggerStay(EnemyStateMachine state, Collider col) { }
    public override void TriggerEnter(EnemyStateMachine state, Collider col) { }
    void CheckSwitch(EnemyStateMachine state){
        if(InAttackRange){
        state.SwitchState(state.attackState);
        
        }
        if(!InSightRange){
             state.SwitchState(state.idleState);
        }
    }
    

    
}
