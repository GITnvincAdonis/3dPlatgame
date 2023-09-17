using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FlyingState : PlayerBaseState
{
  
    
    
    public override void EnterState(PlayerStateMachine state){
        //Debug.Log("flight");
        state.Anim.SetBool("IsFlying", true);
        _currentlyJumping = false;
        
        _maxSpeed = state.MovementSpeed *3.5f;
        _currentSpeed = state.MovementSpeed;
        
        
        
    }
    public override void UpdateState(PlayerStateMachine state){
       
        GroundCheck(state);
        GetInputs();
        HandleDrag(state);
        CheckSwitchState(state);
        XYMovementYRotation(state);
        SpeedControl(state);
        ArtificialSpeedIncrease(state);

        state.RigidBod.AddForce(inputDir.normalized * _currentSpeed* Time.deltaTime, ForceMode.Force);
        state.Stamina = Mathf.Max(state.Stamina - 4 *Time.deltaTime, 0f);
        

    }
     public override void TriggerEnter(PlayerStateMachine state,Collider col){
         if(LayerMask.LayerToName(col.gameObject.layer)== "waterLayer"){
            state.Anim.SetBool("Inwater", true);
            state.Anim.SetBool("IsFlying", false);
            state.PlayerContainer.transform.position = new Vector3(state.PlayerContainer.transform.position.x,col.transform.position.y,state.PlayerContainer.transform.position.z);
            state.SwitchState(state.swimmingState);
        }
     }
    public override void TriggerStay(PlayerStateMachine state,Collider col){}
    public override void TriggerExit(PlayerStateMachine state, Collider col){
        
    }
    public override void CollisionEnter(PlayerStateMachine state, Collision col){}
    public override void CollisionStay(PlayerStateMachine state, Collision col){}
    void CheckSwitchState(PlayerStateMachine state){
        if(_isGrounded||_flightPress||state.Stamina== 0f ){
            state.SwitchState(state.groundState);
            state.Anim.SetBool("IsFlying", false);
        }


    }
    
   

    
}
