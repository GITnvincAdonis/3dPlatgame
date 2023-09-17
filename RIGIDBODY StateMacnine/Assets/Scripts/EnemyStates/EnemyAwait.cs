using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAwait : EnemyBaseState
{
    float timeInAwait;
    public override void EnterState(EnemyStateMachine state){
        //Debug.Log("awaiting");
        timeInAwait = 0f;
    }
    public override void UpdateState(EnemyStateMachine state){
        PositionDifference(state);
        timeInAwait += 2f * Time.deltaTime; 
        CheckSwitch(state);
        PlayerInRange(state);

    }
    public override void CollisionEnter(EnemyStateMachine state, Collision collision) {}
    public override void TriggerStay(EnemyStateMachine state, Collider col) {}
    public override void TriggerEnter(EnemyStateMachine state, Collider col) {}
    void CheckSwitch(EnemyStateMachine state){
        if(timeInAwait > 4f && !InAttackRange){
            state.SwitchState(state.chaseState);
        }
        else if(timeInAwait > 4f && InAttackRange){
            state.SwitchState(state.attackState);
        }
        if(!InSightRange){
             state.SwitchState(state.idleState);
        }
    }
}
