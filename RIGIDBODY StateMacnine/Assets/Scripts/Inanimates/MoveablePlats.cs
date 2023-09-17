using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveablePlats : MonoBehaviour
{

    Vector3[] waypoints = new Vector3[2];
    int currentIndex = 0;

    [SerializeField] private float speed;
    private Vector3 firstLocation;
    private Vector3 secondLocation;
     Rigidbody thingWithRigid;
    Vector3 connectionVelocity;
    Vector3 connectionWorldPosition;
    Rigidbody rb;
    
    


    void Start(){
        firstLocation = transform.position;
        secondLocation = transform.position + -transform.forward * 5f;
        waypoints[0] = firstLocation;
        waypoints[1] = secondLocation;
        rb = GetComponent<Rigidbody>();
        StartCoroutine(plaformCoroutine());
    }
    void Update()
    {
        
       

        
        
        
    }
    
    private IEnumerator plaformCoroutine(){
        
        while(true)
        {
            if(currentIndex < waypoints.Length)
        {
            Vector3 targetPosition = waypoints[currentIndex];
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
            currentIndex++;
            yield return new WaitForSeconds(3f);
        }
        else{
            currentIndex = 0;
        }
        }

        
        
     

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
        thingWithRigid.velocity = new Vector3(Vector3.Dot(relativeVelocity, xAxis), 0f, Vector3.Dot(relativeVelocity, zAxis)) + thingWithRigid.velocity;   
        
         
        
    }
    }
    void UpdateConnectionState(){
        Vector3 connectionMovement = transform.position - connectionWorldPosition;
		//Vector3 connectionMovement = thingWithRigid.position - new Vector3(connectionWorldPosition.x, thingWithRigid.position.y,connectionWorldPosition.z);
		connectionVelocity = connectionMovement / Time.fixedDeltaTime;
        connectionWorldPosition = transform.position;
        
		
	}

}
