using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour, IDamagable
{
    public EnemyType1 data;
    //state

    EnemyBaseState _currentState;
    public EnemyIdle idleState = new EnemyIdle();
    public EnemyChase chaseState = new EnemyChase();
    public EnemyAttack attackState = new EnemyAttack();
    public EnemyAwait awaitState = new EnemyAwait();



    //inherits from scriptable object
    float health;
    float knockBackForce;
    private int _attackRange;
    private float _sightRange;
    private float _slerpSpeed;
    private float _moveSpeed;
    private float _lungeDist;
    private float _height;
    private float _maxSlopeAngle;
    private float _lightDamage;
    private float _heavyDamage;

    //private vars
   
    private Transform _currentPosition;
    
    
    
    [SerializeField] private LayerMask _player;

    private Rigidbody _enemyRigidBod;
    private Transform _enemyOrient;
    //
    
    public Transform CurrentPosition {get { return _currentPosition; }set{ _currentPosition = value; }}
    public float SlerpSpeed {get{ return _slerpSpeed; }set{ _slerpSpeed = value; }}
    public int AttackRange { get { return _attackRange;}}
    public float SightRange { get { return _sightRange;}}
    public LayerMask Player { get { return _player;}}
    public Rigidbody EnemyRigidBod { get { return _enemyRigidBod;} set{ _enemyRigidBod = value; }}
    public float MoveSpeed{ get { return _moveSpeed; } set { _moveSpeed = value; } }
    public float LungeDist { get { return _lungeDist; } set { _lungeDist = value; } }
    public float Height { get { return _height; } set { _height= value; } }
    public float MaxSlopeAngle { get { return _maxSlopeAngle; } set { _maxSlopeAngle = value; } }
    



    void Start()
    {
        _currentState = idleState;
        _currentState.EnterState(this);
        _enemyRigidBod = GetComponent<Rigidbody>();

        _currentPosition = GetComponent<Transform>();


         health = data.health;
         knockBackForce = data.knockBackForce;
         _attackRange = data.attackRange;
         _sightRange= data.sightRange;
         _slerpSpeed = data.slerpSpeed;
         _moveSpeed = data.moveSpeed;
        _lungeDist = data.attackLungeDistance;
        _height = data.entHeight;
        _maxSlopeAngle = data.maxSlopeAngle;
        _lightDamage = data.lightAttackDamage;
        _heavyDamage = data.HeavyAttackDamage;


}

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateState(this);
        //Debug.DrawLine(state.CurrentPosition.position, state.CurrentPosition.forward, Color.blue, state.AttackRange);
        Debug.DrawRay(_currentPosition.position, _currentPosition.forward.normalized * _attackRange, Color.blue);

    }
    void OnCollisionEnter(Collision collision){
        _currentState.CollisionEnter(this, collision);

    }
    void OnTriggerEnter(Collider col){
        _currentState.TriggerEnter(this, col);
    }
    void OnTriggerStay(Collider col)
    {
        _currentState.TriggerStay(this, col);
    }

    public void SwitchState(EnemyBaseState state)
    {
        _currentState = state;
        state.EnterState(this);
    }
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }


    public void tookLighthit()
    {
        Debug.Log("took hit");
        Debug.Log(-transform.forward);
        _enemyRigidBod.AddForce(-transform.forward.normalized * knockBackForce, ForceMode.Force);
        health -= _lightDamage;
        if (health <= 0f)
        {
            Debug.Log("enenmy died");
            // run death logic
            /*
            animation plays
            collider is disabled and maybe msh rendererdada
            done using coroutine
            */
            GetComponent<EnemyStateMachine>().enabled = false;
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
        }


    }
    public void tookHeavyhit(Vector3 dir)
    {
        Debug.Log("took hit");
        Debug.Log(-transform.forward);
        _enemyRigidBod.AddForce(dir * knockBackForce, ForceMode.Force);
        health -= _heavyDamage;
        if (health <= 0f)
        {
            Debug.Log("enenmy died");
            // run death logic
            /*
            animation plays
            collider is disabled and maybe msh rendererdada
            done using coroutine
            */
            GetComponent<EnemyStateMachine>().enabled = false;
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
        }


    }
    public void TookKnockBack()
    {
        Debug.Log(" enemy knockback");
        _enemyRigidBod.AddForce(transform.forward.normalized * knockBackForce , ForceMode.Force);
      


    }
}
