using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public override void EnterState(PlayerStateMachine state){
        Debug.Log("running");
       
    }
    public override void UpdateState(PlayerStateMachine state){
        CheckSwitchState(state);
        GetInputs();
        GroundCheck(state);
        XYMovementYRotation(state);
        //if (OnSlope(state))
       // {
            state.RigidBod.AddForce(GetSlopeMoveDir() * state.MovementSpeed * 12, ForceMode.Force);
        //}
       // else
       // {

            //state.RigidBod.AddForce(new Vector3(state.PlayerBod.forward.x, 0f, state.PlayerBod.forward.z) * state.MovementSpeed * 6, ForceMode.Force);
       // }
       
        state.Stamina = Mathf.Max(state.Stamina - 4 *Time.deltaTime, 0f);
        
    }
     public override void TriggerEnter(PlayerStateMachine state,Collider col){}
    public override void TriggerStay(PlayerStateMachine state,Collider col){}
    public override void TriggerExit(PlayerStateMachine state, Collider col){}
    public override void CollisionEnter(PlayerStateMachine state, Collision col){}
    public override void CollisionStay(PlayerStateMachine state, Collision col){}
    void CheckSwitchState(PlayerStateMachine state){
        if(_jumpPress|| !_isGrounded||(_horizontalInput ==0 && _verticalInput==0)|| state.Stamina <= 0f){
            state.SwitchState(state.groundState);
        }
        
    }
}
