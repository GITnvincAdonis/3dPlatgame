using UnityEngine;
using System.Collections;

public abstract class PlayerBaseState
{
    Vector3 _viewDir;
    protected Vector3 inputDir;
    protected bool _jumpPress;
    protected bool _attackPress;
    protected bool _flightPress;
    protected bool _isGrounded;
    protected float _flightStam;
    protected bool _dashPress;

    protected bool _shooterPress;
    protected bool _playerBulletAim;
    protected bool _bulletShoot;

    protected bool InSightRange;
    protected bool InAttackRange;
    protected bool exitingSlope;
    protected RaycastHit slopeHit;
    protected Collider closest;
    protected bool _currentlyJumping;
    protected bool _currentlyfalling;

    protected bool _interactPress;
    protected float timeInLocoState;
    protected bool addedF = false;
    protected float _currentSpeed;
    protected float _maxSpeed;
    
    //all inputs
    protected float _horizontalInput;
    protected float _verticalInput;
    public abstract void EnterState(PlayerStateMachine state);
    public abstract void UpdateState(PlayerStateMachine state);
    public abstract void CollisionEnter(PlayerStateMachine state, Collision col);
    public abstract void CollisionStay(PlayerStateMachine state, Collision col);

    public abstract void TriggerStay(PlayerStateMachine state,Collider col);
    public abstract void TriggerEnter(PlayerStateMachine state,Collider col);
    public abstract void TriggerExit(PlayerStateMachine state, Collider col);
    public  void CalculateAngDif(PlayerStateMachine state){
    _viewDir = state.PlayerContainer.position - new Vector3(state.Cam.transform.position.x, state.PlayerContainer.position.y, state.Cam.transform.position.z);

    }
    public void XYMovementYRotation(PlayerStateMachine state){
        CalculateAngDif(state);
        //set the forward path relative the camera direction
        state.PlayerOrientation.forward = _viewDir.normalized;
        inputDir = (state.PlayerOrientation.forward * _verticalInput) + (state.PlayerOrientation.right * _horizontalInput);

        if(inputDir != Vector3.zero){
            state.PlayerBod.forward = Vector3.Slerp(new Vector3(state.PlayerBod.forward.x,0f,state.PlayerBod.forward.z), inputDir.normalized, state.SlerpSpeed * Time.deltaTime);
            
        }
        
        
        

    }
     public void GetInputs(){
        _verticalInput = Input.GetAxis("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");
        _jumpPress  =Input.GetKeyDown(KeyCode.Space);
        _attackPress = Input.GetKeyDown(KeyCode.E);
        _shooterPress = Input.GetMouseButtonUp(1);
        _playerBulletAim = Input.GetMouseButton(0);
        _bulletShoot = Input.GetMouseButtonUp(0);
        _flightPress = Input.GetMouseButtonDown(0);
        _dashPress = Input.GetKey(KeyCode.LeftShift);
        _interactPress = Input.GetKeyDown(KeyCode.F);


    }
    protected void GroundCheck(PlayerStateMachine state){

        // benefits of CheckSphere:
        /*
        - allows for more comprehensive ground detection, giving whole mesh radius grounded detection
        /// center of Sphere is within bounding mesh as width is desirable, discarding the unwanted height to the ground detection 
        */

        Vector3 sphereCenter = state.CapCollider.bounds.min - (Vector3.down * state.CapCollider.radius);
    
        if(Physics.CheckSphere(sphereCenter,state.CapCollider.radius +0.4f, state.LayerMask)){
            //Debug.Log("you hit ground");
            _isGrounded = true; 
            state.RigidBod.AddForce(Vector3.down, ForceMode.Force);
            
            

            //state.RigidBod.constraints|= RigidbodyConstraints.FreezePositionY;  
           
        }
        else{
            //Debug.Log("not on ground");
            _isGrounded = false;
            //state.RigidBod.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }
    }
    
    protected bool DownStep(PlayerStateMachine state){
        if(Physics.Raycast(state.PlayerBod.position, Vector3.down, state.PlayerHeight* 0.5f + 0.6f, state.LayerMask)){
            return true;
        }
        return false;
    }
    
    protected void UpStairs(PlayerStateMachine state){
        Vector3 step = new Vector3(state.PlayerBod.forward.x * 0.35f,0.1f, state.PlayerBod.forward.z*0.35f); 
        float newUpForce = 5f + (3f*new Vector3(state.RigidBod.velocity.x,0f,state.RigidBod.velocity.z).magnitude);
        RaycastHit lowerHit;
        if(Physics.Raycast(state.LowerStairCheck.transform.position,state.LowerStairCheck.transform.TransformDirection(Vector3.forward),out lowerHit, 0.1f) && (_verticalInput != 0f || _horizontalInput != 0f)){
            Debug.Log("here");
            RaycastHit upperHit;
            if(!Physics.Raycast(state.UpperStairCheck.transform.position,state.UpperStairCheck.transform.TransformDirection(Vector3.forward),out upperHit, 0.3f)){
                Debug.Log("here");
                //state.PlayerContainer.position += step;
                state.RigidBod.AddForce(new Vector3(0f,newUpForce,0f)*600f *Time.deltaTime, ForceMode.Force);
            }
        }

        RaycastHit lowerHit45;
        if(Physics.Raycast(state.LowerStairCheck.transform.position,state.LowerStairCheck.transform.TransformDirection(.5f,0f,1f),out lowerHit45, 0.1f) && (_verticalInput != 0f || _horizontalInput != 0f)){
            Debug.Log("here");
            RaycastHit upperHit45;
            if(!Physics.Raycast(state.UpperStairCheck.transform.position,state.UpperStairCheck.transform.TransformDirection(.5f,0f,1f),out upperHit45, 0.3f)){
                Debug.Log("here");
                //state.PlayerContainer.position += step;
                state.RigidBod.AddForce(new Vector3(0f,newUpForce,0f)*600f *Time.deltaTime, ForceMode.Force);
            }
        }

        RaycastHit lowerHitM45;
        if(Physics.Raycast(state.LowerStairCheck.transform.position,state.LowerStairCheck.transform.TransformDirection(-.5f,0f,1f),out lowerHitM45, 0.1f) && (_verticalInput != 0f || _horizontalInput != 0f)){
            
            RaycastHit upperHitM45;
            if(!Physics.Raycast(state.UpperStairCheck.transform.position,state.UpperStairCheck.transform.TransformDirection(-.5f,0f,1f),out upperHitM45, 0.3f)){
                Debug.Log("here");
                //state.PlayerContainer.position += step;
                state.RigidBod.AddForce(new Vector3(0f,newUpForce,0f)*600f *Time.deltaTime, ForceMode.Force);
            }
        }
    }

    protected void PlayerInRange(PlayerStateMachine state)
    {
        Vector3 forwardPos = state.PlayerBod.position + state.PlayerBod.forward * 1.5f;
        InSightRange = Physics.CheckSphere(state.PlayerBod.position, state.PlayerSightRange, state.EnemyMask);
        
        InAttackRange = Physics.CheckSphere(forwardPos, state.PlayerAttackRange, state.EnemyMask);

        //calculates the closest enemy object
        Collider[] colliders = Physics.OverlapSphere(state.PlayerBod.position, state.PlayerSightRange, state.EnemyMask);
        float min = state.PlayerSightRange;
        

        foreach (Collider collider in colliders){
            Vector3 colPosition = collider.transform.position;
            Vector2 offset = new Vector2(colPosition.x - state.PlayerBod.position.x, colPosition.z - state.PlayerBod.position.z);
            float calcDist = offset.magnitude;
            if(calcDist < min){
                min = calcDist;
                closest = collider;
                
            }

        }
       
       

    }
    protected bool OnSlope(PlayerStateMachine state)
    {
        if (Physics.Raycast(state.PlayerBod.position, Vector3.down, out slopeHit, state.PlayerHeight * 0.5f + 0.6f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < state.MaxSlopeAngle && angle != 0;
        }
        
        return false;

    }
    
    protected Vector3 GetSlopeMoveDir(){
        return Vector3.ProjectOnPlane(inputDir, slopeHit.normal).normalized;
    }
    protected Vector3 AltGetSlopeMoveDir(PlayerStateMachine state){
        return Vector3.ProjectOnPlane(state.PlayerBod.forward, slopeHit.normal).normalized;
    }
    protected bool CanBulletBill(PlayerStateMachine state){
        Collider[] colliders = Physics.OverlapSphere(state.PlayerBod.position, 2f, state.BulletSwitch);
        foreach(Collider collider in colliders){
            var hasInterface = collider.GetComponent<IInteractable>();
            if(hasInterface != null){
                hasInterface.interact();
                return true;
                
            }
        }
        return false;
        
    }
    protected void CanInteract(PlayerStateMachine state){
        Collider[] colliders = Physics.OverlapSphere(state.PlayerBod.position, 2f, state.Interact);
        foreach(Collider collider in colliders){
            var hasInterface = collider.GetComponent<IInteractable>();
            if(hasInterface != null){
                hasInterface.interact();   
            }
        }
       
        
    }
    protected void HandleDrag(PlayerStateMachine state){
        if(_isGrounded){
            
            state.RigidBod.drag = state.GroundDrag;
            addedF = false;

        }
        else{
            state.RigidBod.drag = state.AirDrag;
        }
        if(_isGrounded && timeInLocoState > 3f){
            state.Stamina = Mathf.Min(state.Stamina + 10f * Time.deltaTime, 20f);
        }
        
    }
    protected void ArtificialSpeedIncrease(PlayerStateMachine state){
        if(_dashPress){
            _currentSpeed = Mathf.Min(_currentSpeed + 2000*Time.deltaTime,_maxSpeed);
        }
        if(!_dashPress){
            _currentSpeed= Mathf.Max(_currentSpeed - 4000f*Time.deltaTime,state.MovementSpeed);
        }

    }
    protected void SpeedControl(PlayerStateMachine state){
        Vector3 curVel = new Vector3(state.RigidBod.velocity.x,0f,state.RigidBod.velocity.z);
        if(curVel.magnitude > _maxSpeed){
            Vector3 LimitedVel = curVel.normalized * _maxSpeed;
            state.RigidBod.velocity = new Vector3(LimitedVel.x,state.RigidBod.velocity.y,LimitedVel.z);
        }
    }
    

}
