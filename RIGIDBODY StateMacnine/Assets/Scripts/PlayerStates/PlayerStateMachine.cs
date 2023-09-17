using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour, IDamagable
{

    //SCRIPTABLE OBJECTS
    public PlayerData data;
    public PlayerBulletsBroadcast bulletAngle;




    // STATES DECLARATION
    PlayerBaseState currentState;
    
    public GroundedState groundState = new GroundedState();
    public FlyingState flightState = new FlyingState();
    public CombatState fightState = new CombatState();
    public BulletBillState billState = new BulletBillState();
    public FPSState shootingState = new FPSState();
    public SwimState swimmingState =  new SwimState();
   








    float knockBackForce;
    float health;

    //vars
 
    [SerializeField] private Rigidbody _rigidBod;
    [SerializeField] private Transform _playerOrientation;
    [SerializeField] private Transform _playerContainer;
    [SerializeField] private Transform _playerBod;
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private Transform _enemyPos;
   
    [SerializeField] private Camera cam;
    [SerializeField] private Transform gun;
    [SerializeField] private GameObject playerBullets;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CapsuleCollider capcollider;

    [SerializeField] private GameObject _lowerStairCheck;
    [SerializeField] private GameObject _upperStairCheck;

    private Animator anim;

    float FlightStaminaStorage;
    private float _slerpSpeed; 
    private float _movementSpeed;
    private float _groundDrag;
    private float _airDrag;
    private float _playerHeight;
    private float _jumpTime;
    private float _fallMultiplier;
    private float _jumpMultiplier;
    private float _flightStamina;
    private float _sightRange;
    private int _attackRange;
    private float _maxSlopeAngle;
    private float _lightAttackDist;
    private float _heavyAttackDist;
    private float _lightAttackDamage;
    private float _heavyAttackDamage;
    private float _stamina;
    private float _bulletStamina;
    private float _bulletSpeed;

    private LayerMask _bulletbreakable;
    private LayerMask _layerMask;
    private LayerMask _enemyMask;
    private LayerMask _bulletSwitch;
    private LayerMask _interact;

    
    //getters and setters
    public float GroundDrag {get{return _groundDrag;}set{_groundDrag = value;}}
    public float SlerpSpeed {get{return _slerpSpeed;}set{_slerpSpeed = value;}} 
    public float MovementSpeed {get{return _movementSpeed;}set{_movementSpeed =value;}}
    public float AirDrag {get{return _airDrag;}set{_airDrag = value;}}
    public float PlayerHeight {get{return _playerHeight;}}
    public LayerMask LayerMask {get{return _layerMask;}}

    
    public Rigidbody RigidBod {get{return _rigidBod;}}   
    public Transform PlayerOrientation {get{return _playerOrientation;}}
    public Transform PlayerContainer {get{return _playerContainer;}}
    public BoxCollider Collider {get{return _collider;}}
    public Transform PlayerBod {get{return _playerBod;}}
    public Transform EnemyPos {get{return _enemyPos;}}

    public float JumpTime {get{return _jumpTime;}}

    public float FallMultiplier {get{return _fallMultiplier;}}
    public float JumpMultiplier {get {return _jumpMultiplier;}}

    public float FlightStamina {get{return _flightStamina;}set{_flightStamina = value;}}
    public float AFlightStaminaStorage {get {return FlightStaminaStorage;}}
    public Camera Cam {get{return cam;}}

    public int PlayerAttackRange { get { return _attackRange;}}
    public float PlayerSightRange { get { return _sightRange;}}
    public LayerMask EnemyMask{ get { return _enemyMask;}}
    public LayerMask BulletSwitch{ get { return _bulletSwitch;}}
    public LayerMask BulletBreakable{ get { return _bulletbreakable;}}
    public LayerMask Interact{ get { return _interact;}}
    public float MaxSlopeAngle {get{ return _maxSlopeAngle;}}
    public float HeavyAttackDist { get { return _heavyAttackDist; } }
     public float LightAttackDist {get{ return _lightAttackDist;}}
    public float LightAttackDamage { get { return _lightAttackDamage; } }
    public float HeavyAttackDamage { get { return _heavyAttackDamage; } }
    public float Stamina { get { return _stamina; } set{ _stamina = value; } }
    public float BulletStamina { get { return _bulletStamina; } set{ _bulletStamina = value; } }
    public float BulletSpeed { get { return _bulletSpeed; } set{ _bulletSpeed = value; } }
    public Transform Gun { get { return gun; } }
    public Cinemachine.CinemachineVirtualCamera VirtualCam {get{return virtualCamera;}}
    public Animator Anim {get{return anim;}}
    public CapsuleCollider CapCollider {get{return capcollider;}}
    


    public PlayerBulletsBroadcast BulletAngle {get{return bulletAngle;}}

    public GameObject LowerStairCheck {get{return _lowerStairCheck;}}
    public GameObject UpperStairCheck {get{return _upperStairCheck;}}
    Vector3 collisionHeight;
    Vector3 max;
    void Awake(){
        _upperStairCheck.transform.position = new Vector3(_upperStairCheck.transform.position.x, transform.position.y - 0.2f,_upperStairCheck.transform.position.z);
    }
    void Start()
    {
        

        AssignDataFields();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        currentState = groundState;   
        currentState.EnterState(this);
        anim = GetComponent<Animator>();
}

    void AssignDataFields(){
        _slerpSpeed = data.slerpSpeed;
        _movementSpeed = data.moveSpeed;
        _groundDrag = data.groundDrag;
        _airDrag = data.airDrag;

        _playerHeight = data.entHeight;
        _jumpTime = data.jumpTime;
        _fallMultiplier = data.fallMultiplier;
        _jumpMultiplier = data.jumpMultiplier;

        _flightStamina = data.flightStamina;

        _sightRange = data.sightRange;
        _attackRange = data.attackRange;

        _maxSlopeAngle = data.maxSlopeAngle;
        knockBackForce = data.knockBackForce;
        health = data.health;
        _lightAttackDist = data.LightAttackDist;
        _heavyAttackDist = data.HeavyAttackDist;

        _lightAttackDamage = data.lightAttackDamage;
        _heavyAttackDamage = data.HeavyAttackDamage;
        _stamina = data.stamina;
        _bulletStamina = data.bulletStamina;
        _bulletSpeed = data.bulletSpeed;
        _bulletbreakable = data.whatIsBulletBreakable;
        FlightStaminaStorage = _flightStamina ;

        _layerMask = data.whatIsGround;
        _enemyMask = data.whatIsEnemy;
        _bulletSwitch = data.whatIsBulletSwitches;
        _interact = data.whatIsInteractable;
        
    }

    //======================================================================================================================
    // responsible for assigning the current states override functions in their corresponding callbacks
    void Update()
    {currentState.UpdateState(this); //Debug.Log(_stamina);    
    }

    public void SwitchState(PlayerBaseState state){
        currentState = state;
        state.EnterState(this);
    }

    void OnTriggerEnter(Collider col){
        currentState.TriggerEnter(this, col);

    }
    void OnTriggerStay(Collider col){
        currentState.TriggerStay(this, col);

    }
    void OnTriggerExit(Collider col){
        currentState.TriggerExit(this, col);
    }
    
    void OnCollisionEnter(Collision collision){
        currentState.CollisionEnter(this,collision);
    }

    void OnCollisionStay(Collision col){
    currentState.CollisionStay(this,col);
    }
    void OnCollisionExit(Collision collision){
        max = Vector3.zero;
    }
    //=========================================================================================================================
    //==================================================================================================================
    //Interface implementations
    public void tookLighthit(){
        Debug.Log("player took hit");
        health -= _lightAttackDamage;
        _rigidBod.AddForce(-_playerBod.forward.normalized * knockBackForce, ForceMode.Force);
        if (health <= 0f)
        {
            Debug.Log("player died");
            // run death logic
            /*
            animation plays
            collider is disabled and maybe msh renderer
            done using coroutine
            */
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
        }}
  
    public void tookHeavyhit(Vector3 dir){
        StartCoroutine(HitCo());
        Debug.Log("player took hit");
        health -= _heavyAttackDamage;
        _rigidBod.AddForce(dir * knockBackForce, ForceMode.Force);
        if (health <= 0f)
        {
            Debug.Log("player died");
            // run death logic
            /*
            animation plays
            collider is disabled and maybe msh renderer
            done using coroutine
            */
            GetComponent<Collider>().enabled = false;
            GetComponent<PlayerStateMachine>().enabled = false;
            
        }
        }

    public void TookKnockBack(){
        Debug.Log(" player knockback");}
    //==================================================================================================================
    public void CreateCharges(){
        Vector3 pointOfShot = gun.position + gun.forward * 1.5f;
        Instantiate(playerBullets, pointOfShot, gun.rotation );
    }
    //================================================================
    //Debugging purposes
    void OnDrawGizmosSelected(){
        //Vector3 forwardPos = _playerBod.position + _playerBod.forward * 1.5f;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_playerBod.position , _sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_playerBod.position, _attackRange);
    }
    IEnumerator  HitCo(){
        anim.SetBool("IsHit", true);
        yield return null;
        anim.SetBool("IsHit", false);
    }
    
}
