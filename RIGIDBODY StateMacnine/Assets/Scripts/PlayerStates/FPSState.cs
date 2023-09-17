using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
/// this class handles player rotation through keyboard inputs
/// also serves as the eidtor of the BULLETANGLECHANGE event
/// remotely Controls the instanciation of playerbullet prefabs
///</summary>

public class FPSState : PlayerBaseState
{
    public override void EnterState(PlayerStateMachine state){
        state.Anim.SetBool("IsRunning", false);
        state.Anim.SetBool("IsWalking", false);
        state.Anim.SetBool("IsShooting", true);
        
        
       
        //Debug.Log("here");
        state.SlerpSpeed = 0.5f;
        state.VirtualCam.enabled = true;
      
    }
    public override void UpdateState(PlayerStateMachine state){
        GetInputs();
        FpsMovement(state);
        state.Anim.SetFloat("MoveX",_horizontalInput );
        state.Anim.SetFloat("MoveY",_verticalInput );


        float VInp = Input.GetAxisRaw("Mouse Y") * 5 *Time.deltaTime;
        float calcVertInp = state.Anim.GetFloat("VerticalInp" ) + VInp;


        if(calcVertInp < -1){
            calcVertInp= -1;
        }
        else if(calcVertInp > 1){
            calcVertInp = 1;
        }
        
        if(_verticalInput != 0f || _horizontalInput != 0f){
            state.Anim.SetBool("IsStrafing",true );
        }
        else if(_verticalInput == 0f && _horizontalInput == 0f){
            state.Anim.SetBool("IsStrafing",false );
        }


        state.Anim.SetFloat("VerticalInp",calcVertInp );

        
        CheckSwitchState(state);
        XYMovementYRotation(state);
        state.PlayerBod.rotation = Quaternion.Slerp(state.PlayerBod.rotation,state.Cam.transform.rotation,state.SlerpSpeed);
        if(_playerBulletAim && state.BulletAngle !=null){
            state.BulletAngle.IncreaseAngle();
        }
        if(_bulletShoot){
            state.Anim.SetBool("HasShot", true);
            state.CreateCharges();
            //state.BulletAngle.ResetAngle();
        }
        else if(!_bulletShoot){
            state.Anim.SetBool("HasShot", false);
        }
    }
     public override void TriggerEnter(PlayerStateMachine state,Collider col){
         if(LayerMask.LayerToName(col.gameObject.layer)== "waterLayer"){
            state.Anim.SetBool("Inwater", true);
            state.PlayerContainer.transform.position = new Vector3(state.PlayerContainer.transform.position.x,col.transform.position.y,state.PlayerContainer.transform.position.z);
            state.SwitchState(state.swimmingState);
        }
     }
    public override void CollisionEnter(PlayerStateMachine state, Collision col){}
    public override void CollisionStay(PlayerStateMachine state, Collision col){}
    public override void TriggerStay(PlayerStateMachine state,Collider col){}
    public override void TriggerExit(PlayerStateMachine state, Collider col){}
    void CheckSwitchState(PlayerStateMachine state){
        if(_shooterPress){
            state.Anim.SetBool("IsShooting", false);
            state.Anim.SetBool("IsStrafing",false );
            state.VirtualCam.enabled = false;
            state.SwitchState(state.groundState);
            
        }
    }
    void FpsMovement(PlayerStateMachine state){
        state.RigidBod.AddForce(state.PlayerBod.forward * _verticalInput * state.MovementSpeed *0.5f * Time.deltaTime, ForceMode.Force);
        state.RigidBod.AddForce(state.PlayerBod.right * _horizontalInput * state.MovementSpeed *0.5f* Time.deltaTime, ForceMode.Force);

    }
}
