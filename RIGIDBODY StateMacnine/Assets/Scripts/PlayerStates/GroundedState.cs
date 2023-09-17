using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
///this state is responsible for the JUMP, GROUNDED and Fall state
///</summary>
public class GroundedState : PlayerBaseState
{
 /*
    Rigidbody thingWithRigid;
    Vector3 connectionVelocity;
    Vector3 connectionWorldPosition;
    */
    float _initialVelocity;
    float _timeToApex;
    float _minSpeed;
    bool isFalling;
    bool isJumping;

    public override void EnterState(PlayerStateMachine state)
    {
        timeInLocoState  = 0f;
        state.RigidBod.useGravity = true;
        state.SlerpSpeed = 7f;
        _maxSpeed = state.MovementSpeed *3f;
        _minSpeed = state.MovementSpeed;
        _currentSpeed = state.MovementSpeed;
        state.PlayerBod.localScale = new Vector3(1f,1f,1f);
        
    }

    public override void UpdateState(PlayerStateMachine state){
        timeInLocoState += Time.deltaTime;
        CheckSwitchState(state);
        GetInputs();
        XYMovementYRotation(state);      
        state.RigidBod.AddForce(GetSlopeMoveDir() * _currentSpeed * Time.deltaTime, ForceMode.Force);
        /*
        if(OnSlope(state)){
            //Debug.Log("slope hit");
            state.RigidBod.AddForce(GetSlopeMoveDir() * _currentSpeed * Time.deltaTime, ForceMode.Force);
            if(state.RigidBod.velocity.y != 0f){
                state.RigidBod.AddForce(Vector3.down, ForceMode.Force);
            }}else{
            state.RigidBod.AddForce(inputDir.normalized * _currentSpeed * Time.deltaTime, ForceMode.Force);}*/

        GroundCheck(state);
        HandleDrag(state);
        InitializeJumpVars(state);
        HandleJump(state);
        SpeedControl(state);
        ArtificialSpeedIncrease(state);
        AnimationTrigger(state);
        if(!OnSlope(state)  && _isGrounded){
            UpStairs(state);
        }
        

        //additional nudge when player is off ledge
        if(_isGrounded && state.RigidBod.velocity.y < -0.1f && !OnSlope(state)){
             if(!addedF){
               
                //state.RigidBod.AddForce(new Vector3(state.PlayerBod.forward.x, 0f, state.PlayerBod.forward.z) * 35f * Time.deltaTime, ForceMode.Impulse);
               
                addedF = true;
            }
        }
/*
        if( !isJumping && DownStep(state) && !_isGrounded){
            Debug.Log("down");
            state.RigidBod.AddForce(Vector3.down * 20f * Time.deltaTime, ForceMode.Impulse);
            
        }
*/        if(_interactPress){
            CanInteract(state);
            }
        if(!_isGrounded && !isJumping){
            //Debug.Log("gee you not groundeed");
            state.RigidBod.velocity += Physics.gravity * 15f * Time.deltaTime;
            /*
            if(!addedF){
               
                state.RigidBod.AddForce(new Vector3(state.PlayerBod.forward.x, 0f, state.PlayerBod.forward.z) * 16 * Time.deltaTime, ForceMode.Impulse);
               
                addedF = true;
            }
            */            
        }
        state.Anim.SetBool("IsFalling", true);
        state.RigidBod.useGravity = !OnSlope(state);
        state.RigidBod.velocity += Physics.gravity * 2f * Time.deltaTime;
        }

     public override void TriggerEnter(PlayerStateMachine state,Collider col){
        //Debug.Log(LayerMask.LayerToName(col.gameObject.layer));
        if(LayerMask.LayerToName(col.gameObject.layer)== "waterLayer"){
            state.Anim.SetBool("Inwater", true);
            //state.PlayerContainer.transform.position = new Vector3(state.PlayerContainer.transform.position.x,col.transform.position.y,state.PlayerContainer.transform.position.z);
            state.SwitchState(state.swimmingState);
        }
     }
    public override void TriggerStay(PlayerStateMachine state, Collider col){}
    public override void CollisionEnter(PlayerStateMachine state, Collision col){}
    public override void CollisionStay(PlayerStateMachine state, Collision col){}



/*
    void UpdateConnectionState(){
        if(LayerMask.LayerToName(col.gameObject.layer) == "MovingPlatforms"){
            thingWithRigid = col.collider.GetComponent<Rigidbody>();
            if(thingWithRigid != null){
                if(thingWithRigid.isKinematic || thingWithRigid.mass >= state.RigidBod.mass){
                    UpdateConnectionState();
                }
            }
        Vector3 xAxis = new Vector3(-1f,0f,0f);
        Vector3 zAxis = new Vector3(0f,0f,-1f);
        Vector3 relativeVelocity =  state.RigidBod.velocity - connectionVelocity;
        state.RigidBod.velocity = new Vector3(Vector3.Dot(relativeVelocity, xAxis), 0f, Vector3.Dot(relativeVelocity, zAxis)) + state.RigidBod.velocity;   
         
        
    }
        Vector3 connectionMovement = thingWithRigid.position - connectionWorldPosition;
		//Vector3 connectionMovement = thingWithRigid.position - new Vector3(connectionWorldPosition.x, thingWithRigid.position.y,connectionWorldPosition.z);
		connectionVelocity = connectionMovement / Time.fixedDeltaTime;
        connectionWorldPosition = thingWithRigid.position;
        
		
	}
*/
    public override void TriggerExit(PlayerStateMachine state, Collider col){}
    void CheckSwitchState(PlayerStateMachine state)
    {
        if(_attackPress && state.Stamina > 1f)
        {
            state.Anim.SetBool("IsFighting", true);
            state.SwitchState(state.fightState);}
        if(_flightPress && !_isGrounded && state.Stamina > 1f)
        {state.SwitchState(state.flightState);}    
        /*
        if(_dashPress && _isGrounded)
        {state.SwitchState(state.runState);}   */
        if(_interactPress && CanBulletBill(state) && _isGrounded){
            state.SwitchState(state.billState);                
        }
        if(_isGrounded && _shooterPress){
            state.Anim.SetBool("IsWalking", false);
            state.Anim.SetBool("IsRunning", false);
            state.SwitchState(state.shootingState); 
        }

    }
    

    void InitializeJumpVars(PlayerStateMachine state){
         _timeToApex = state.JumpTime * 0.5f;
        //_NormalGravity = (-2 * _maxJumpHeight) / Mathf.Pow(_timeToApex,2);
        _initialVelocity = -Physics.gravity.y * _timeToApex;
    }
    void HandleJump(PlayerStateMachine state){
        ///Debug.Log(isJumping);
        // original upwards force to jump
        if(_jumpPress && _isGrounded){
            
            state.RigidBod.velocity = new Vector3(state.RigidBod.velocity.x,0f,state.RigidBod.velocity.z);
            state.RigidBod.velocity = new Vector3(state.RigidBod.velocity.x,_initialVelocity,state.RigidBod.velocity.z);
        }
       // checking if player is in descent of jump
        if(state.RigidBod.velocity.y < -0.1f|| Input.GetKeyUp(KeyCode.Space) ){
             
            isFalling = true;
        }  
        else if (_isGrounded){
            state.Anim.SetBool("IsFalling", false);
            isJumping = false;
            isFalling = false;
        }
    
        
       
        // ISjUmping is important as character can be falling without jumping. the dowards force used to fix parabola isnt necessary elsewhere
        if(state.RigidBod.velocity.y > 2f){
            isJumping = true;
        }
        // steap decline force
        if(isFalling && isJumping){
            ///Debug.Log("fall");
            state.RigidBod.velocity += Physics.gravity * state.FallMultiplier * Time.deltaTime;
        }
    }

    
    void AnimationTrigger(PlayerStateMachine state){
        bool SkiQuestion =  state.Anim.GetBool("IsSkidding");
        bool RunQuestion = state.Anim.GetBool("IsRunning");
        bool WalkQuestion = state.Anim.GetBool("IsWalking");

        bool OnGround = state.Anim.GetBool("IsGrounded");
        

        if((_horizontalInput !=0f||_verticalInput!=0f) && !WalkQuestion){
            state.Anim.SetBool("IsWalking", true);
        }
        if((_horizontalInput == 0f && _verticalInput ==0f) && WalkQuestion){
            state.Anim.SetBool("IsWalking", false);
        }
        if(((_horizontalInput !=0f||_verticalInput!=0f) && _dashPress) && !RunQuestion){
            state.Anim.SetBool("IsRunning", true);
        }
        if((!_dashPress|| (_horizontalInput == 0f && _verticalInput ==0f)) && RunQuestion){
            state.Anim.SetBool("IsRunning", false);
        }
        if(_isGrounded){
            state.Anim.SetBool("IsGrounded", true);
            state.Anim.SetBool("IsJumping", false);
        }
        else if(!_isGrounded){
            state.Anim.SetBool("IsGrounded", false);
        }
        if(_jumpPress){
            state.Anim.SetBool("IsJumping", true);
        }
        
        
        /*      
        if(!RunQuestion && state.RigidBod.velocity.magnitude > 9f && _isGrounded){
            Debug.Log("yes");
            //state.Anim.SetTrigger("IsSkidding");
        }  if(SkiQuestion && state.RigidBod.velocity.magnitude < 0.6f){
            //state.Anim.SetBool("IsSkidding", false);
        }*/

    }
    
   
    
}
