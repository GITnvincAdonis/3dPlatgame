using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ONEWAYplatMovement : MonoBehaviour
{
    public MovePlatformBroadCast activationChannelSO;
    
    

    [SerializeField] private float speed;
    private Vector3 firstLocation;
    private Vector3 secondLocation;
     Rigidbody thingWithRigid;
    Vector3 connectionVelocity;
    Vector3 connectionWorldPosition;

    


    void Start(){
        firstLocation = transform.position;
        secondLocation = transform.position + -transform.forward * 500f;
        
       
    }
    void Update()
    {
        
       

        
        
        
    }
 
    
    void OnCollisionStay(Collision col){
        if(LayerMask.LayerToName(col.gameObject.layer) == "Player"){
            thingWithRigid = col.collider.GetComponentInParent<Rigidbody>();
            if(thingWithRigid != null){
                UpdateConnectionState();
                
            }
        Vector3 xAxis = new Vector3(1f,0f,0f);
        Vector3 zAxis = new Vector3(0f,0f,1f);

        Vector3 relativeVelocity = connectionVelocity - thingWithRigid.velocity;
        Debug.Log(connectionVelocity);
         Debug.Log(thingWithRigid.velocity);
        thingWithRigid.velocity = new Vector3(Vector3.Dot(relativeVelocity, xAxis), 0f, Vector3.Dot(relativeVelocity, zAxis)) + thingWithRigid.velocity * 1.4f;   
        
         
        
    }
    }
    void UpdateConnectionState(){
        Vector3 connectionMovement = transform.position - connectionWorldPosition;
		//Vector3 connectionMovement = thingWithRigid.position - new Vector3(connectionWorldPosition.x, thingWithRigid.position.y,connectionWorldPosition.z);
		connectionVelocity = connectionMovement / Time.fixedDeltaTime;
        connectionWorldPosition = transform.position;
        
		
	}
    private void OnEnable(){
        activationChannelSO.walkedOnButton += MoveCoroutine;
    }
    private void OnDisable(){
        activationChannelSO.walkedOnButton -= MoveCoroutine;
    }
    IEnumerator DoRoutine(){
        while(true){
        if(Vector3.Distance(transform.position, secondLocation)> 0.01f){
            transform.position = Vector3.MoveTowards(transform.position, secondLocation, speed * Time.deltaTime);}
            yield return null;
        }
    }

    void MoveCoroutine(){
        StartCoroutine(DoRoutine());
    }
         
        
    

}

