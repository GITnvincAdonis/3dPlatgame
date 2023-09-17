using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimState : PlayerBaseState
{
    float SwimSpeed;
    public override void EnterState(PlayerStateMachine state){
        
        //state.RigidBod.useGravity = false;
        Debug.Log("swimming");
    }
    public override void UpdateState(PlayerStateMachine state){
        
        CheckSwitchState(state);
        GetInputs();
        //GroundCheck(state);
        XYMovementYRotation(state);
        
        SwimSpeed += (Mathf.Sqrt(Mathf.Pow(_horizontalInput,2)) + Mathf.Sqrt(Mathf.Pow(_verticalInput,2)))* 10* Time.deltaTime;

        if(_horizontalInput == 0 && _verticalInput == 0){
            SwimSpeed -= 15f * Time.deltaTime;
        }
        if(SwimSpeed < 0){
            SwimSpeed = 0;
        }
        if(SwimSpeed >5){
            SwimSpeed = 5;
        }
        //Debug.Log(state.Anim.GetFloat("SwimSpeed"));
        state.Anim.SetFloat("SwimSpeed", SwimSpeed);
        state.RigidBod.AddForce(GetSlopeMoveDir() * state.MovementSpeed *0.4f* Time.deltaTime, ForceMode.Force);
        
    }
    public override void CollisionEnter(PlayerStateMachine state, Collision col){}
    public override void CollisionStay(PlayerStateMachine state, Collision col){}

    public override void TriggerStay(PlayerStateMachine state,Collider col){}
    public override void TriggerEnter(PlayerStateMachine state,Collider col){}
    public override void TriggerExit(PlayerStateMachine state, Collider col){
        if(LayerMask.LayerToName(col.gameObject.layer)=="waterLayer"){
            //state.RigidBod.useGravity = true;
            state.Anim.SetBool("Inwater", false);
            
            state.SwitchState(state.groundState);
        }
    }

    void CheckSwitchState(PlayerStateMachine state){
        if(_jumpPress){
            state.RigidBod.velocity += (state.PlayerBod.forward * 5f) +  new Vector3(0, 60f,0);

        }
    }
}
