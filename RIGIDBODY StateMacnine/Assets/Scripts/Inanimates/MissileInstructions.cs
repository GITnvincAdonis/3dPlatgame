using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileInstructions : MonoBehaviour
{
    public MissileMovement missileData;
    public Transform playerPos;

    float _turnSpeed;
    float _forwardForce;
    float _deathSpan;
    Rigidbody rb;
    LayerMask _playerMask;
    float _sightRange;
    bool turnSpeedIncrease;
    // Start is called before the first frame update
    

    private void FindPlayerTransform()
    {
        if (playerPos == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerPos = player.transform;
            }
        }
    }
    void Start()
    {
        FindPlayerTransform();
        _turnSpeed = missileData.turnSpeed;
        _forwardForce = missileData.forwardForce;
        _deathSpan = missileData.deathSpan;
        _playerMask = missileData.whatIsPlayer;
        _sightRange = missileData.sightRange;
        
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, _deathSpan);
        //initial Impact of bullet leaving barrell
        StartCoroutine(HomingSpeedDecreaser());
        rb.AddForce(new Vector3(transform.forward.x,0,transform.forward.z)* _forwardForce * 2f* Time.deltaTime, ForceMode.Impulse );
        
        
    }

    // Update is called once per frame
    void Update()
    {
        checkForPlayer();
        CalcPlayerPos();
        IncreaseTurnSpeed();
        rb.AddForce(new Vector3(transform.forward.x,0,transform.forward.z).normalized * _forwardForce * Time.deltaTime , ForceMode.Force );


        
    }
    void CalcPlayerPos(){
        Vector3 viewDir = playerPos.position - new Vector3(transform.position.x, playerPos.position.y,transform.position.z);
        Quaternion rot = Quaternion.LookRotation(viewDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, _turnSpeed * Time.deltaTime);
    }
    void checkForPlayer(){
        Vector3 forwardPos = transform.position + transform.forward * 1f;
        bool InSightRange = Physics.CheckSphere(forwardPos, _sightRange, _playerMask);
        if(InSightRange){
            StartCoroutine(HomingSpeedDecreaser());
        }
    }
    private IEnumerator HomingSpeedDecreaser(){
        _turnSpeed = _turnSpeed * 0.75f;
        yield return new WaitForSeconds(1f);
        turnSpeedIncrease = true;
        
    }
    private void IncreaseTurnSpeed(){
        if(turnSpeedIncrease){
            _turnSpeed = Mathf.Min(_turnSpeed + 1f * Time.deltaTime, missileData.turnSpeed);
        }
        if(_turnSpeed >= missileData.turnSpeed){
            turnSpeedIncrease= false;
        }
    }

    void OnCollisionEnter(Collision col){
         //Debug.Log(col.collider);
    
        var hitGameObject = col.collider;
        var parentObjectWithScript = hitGameObject.GetComponentInParent<IDamagable>();

        if (parentObjectWithScript != null){
            Debug.Log("HIT PLAYER");
            parentObjectWithScript.tookHeavyhit(new Vector3(transform.forward.x,0f,transform.forward.z));
        }
        else{
            var breakableObject = hitGameObject.GetComponent<IBulletBreak>();
            if(breakableObject != null){
                breakableObject.destroy();
            }
        }
    
    Destroy(gameObject);
        
    }
    void OnDrawGizmosSelected(){
        Vector3 forwardPos = transform.position + transform.forward * 1f;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(forwardPos, _sightRange);
    }
}
