using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBillState : PlayerBaseState
{
    float upbillSpeedLim;
    float lowerbillSpeedLim;
    float billStamina;
    float terminalVelocity;
    bool inTerminalSpeed;
    float calcSpeed;

    public override void EnterState(PlayerStateMachine state){
        state.Anim.SetBool("IsBullet", true);
        state.SlerpSpeed = 3f;
        calcSpeed = state.BulletSpeed;
        upbillSpeedLim =  state.BulletSpeed *2f;
        lowerbillSpeedLim = state.BulletSpeed *0.3f; 
        billStamina = state.BulletStamina;
        terminalVelocity = 8f;
        state.CapCollider.height = 1f;
        //state.PlayerBod.localScale = new Vector3(0.8f,0.8f,0.8f);
        
    }
    public override void UpdateState(PlayerStateMachine state){
        GetInputs();
        CheckSwitchState(state);
        XYMovementYRotation(state);
        

        if(calcSpeed > terminalVelocity){
            inTerminalSpeed =true;
        }
        else{
            inTerminalSpeed = false;
        }
        if(Input.GetKey(KeyCode.Space)){
            calcSpeed = Mathf.Min(calcSpeed + 600* Time.deltaTime,upbillSpeedLim);
            billStamina -= 2*Time.deltaTime;
        }
        else{
            calcSpeed = Mathf.Max(calcSpeed - 200* Time.deltaTime,lowerbillSpeedLim);
            billStamina -= Time.deltaTime;
        }
        state.RigidBod.AddForce(new Vector3(state.PlayerBod.forward.x,0f,state.PlayerBod.forward.z) * calcSpeed * Time.deltaTime, ForceMode.Force);
        state.RigidBod.useGravity = false;
        
        state.RigidBod.drag = 5f;
    }
    public override void CollisionEnter(PlayerStateMachine state, Collision col){
        // Debug.Log("Collided with layer: " + LayerMask.LayerToName(col.gameObject.layer));
        if(LayerMask.LayerToName(col.gameObject.layer) == "BulletBreakable" && inTerminalSpeed){
            var willDestruct = col.collider.GetComponent<IBulletBreak>();
            if(willDestruct!= null){
               willDestruct.destroy();
            }
            
        }
        else{
            state.Anim.SetBool("IsBullet", false);
            state.CapCollider.height = 2f;
            state.SwitchState(state.groundState); 
        }
    }
    public override void CollisionStay(PlayerStateMachine state, Collision col){}
     public override void TriggerEnter(PlayerStateMachine state,Collider col){}
    public override void TriggerStay(PlayerStateMachine state,Collider col){}
    public override void TriggerExit(PlayerStateMachine state, Collider col){}
    void CheckSwitchState(PlayerStateMachine state){
        if(_interactPress && CanBulletBill(state)||billStamina <= 0f){
            state.Anim.SetBool("IsBullet", false);
            state.CapCollider.height = 2f;
            state.SwitchState(state.groundState);                
        }
    }
}
