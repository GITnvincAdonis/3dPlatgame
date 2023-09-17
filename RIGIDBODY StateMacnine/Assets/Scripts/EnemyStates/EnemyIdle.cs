using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : EnemyBaseState
{
    float ran;
   
    public override void EnterState(EnemyStateMachine state) {
       // Debug.Log("hello from Idle");
        

     }
    public override void UpdateState(EnemyStateMachine state) {
        CheckSwitch(state);
        ran += Time.deltaTime;
        PlayerInRange(state);

        //Debug.Log(ran);
        
    }
    public override void CollisionEnter(EnemyStateMachine state, Collision collision) {

     }
    public override void TriggerStay(EnemyStateMachine state, Collider col) { 

    }
    public override void TriggerEnter(EnemyStateMachine state, Collider col) {
        
     }
    void CheckSwitch(EnemyStateMachine state){
        if(InSightRange){
            state.SwitchState(state.chaseState);
        }
    }
}
