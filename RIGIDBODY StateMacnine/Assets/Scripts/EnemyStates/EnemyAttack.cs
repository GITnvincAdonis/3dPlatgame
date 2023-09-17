using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyBaseState
{
    float TimeInAttack;
    public override void EnterState(EnemyStateMachine state) {
        TimeInAttack = 0f;
        
    }
    public override void UpdateState(EnemyStateMachine state) {
        TimeInAttack += Time.deltaTime;

        RaycastHit col;

        if (TimeInAttack > 1f){
            if(OnSlope(state)){
                state.EnemyRigidBod.AddForce(GetSlopeMoveDir(state) * state.LungeDist, ForceMode.Impulse);
            }
            else{

            state.EnemyRigidBod.AddForce(state.CurrentPosition.forward * state.LungeDist, ForceMode.Impulse);
            }
             
            if(Physics.Raycast(state.transform.position, state.transform.forward, out col,state.AttackRange)){
                Debug.Log(col.collider.tag);
                var thingHit = col.collider.GetComponentInParent<IDamagable>();
                if(thingHit != null){
                    //Debug.Log("he hit you");
                   thingHit.tookHeavyhit(state.CurrentPosition.forward);
                }

            }
            
            //Debug.Log("leaving");
            state.SwitchState(state.awaitState);
        }
        

    }
    public override void CollisionEnter(EnemyStateMachine state, Collision collision) { }
    public override void TriggerStay(EnemyStateMachine state, Collider col) { }
    public override void TriggerEnter(EnemyStateMachine state, Collider col) { }
}
