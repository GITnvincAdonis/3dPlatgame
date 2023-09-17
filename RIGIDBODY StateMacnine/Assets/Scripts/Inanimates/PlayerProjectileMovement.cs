using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileMovement : MonoBehaviour
{   
    public PlayerBulletsBroadcast angleSO;
    Rigidbody rb;
    float upAngle;
    // Start is called before the first frame update
    void Start()
    {
        RetrieveAngle(angleSO.upWardAngle);
        
        
        rb =  GetComponent<Rigidbody>();
        //rb.AddForce(new Vector3(0f,upAngle,0f), ForceMode.Impulse);
        // upward force wa redirected to forward force
        rb.AddForce(transform.forward * (20f + upAngle), ForceMode.Impulse);
        angleSO.ResetAngle();
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnEnable(){
        angleSO.bulletAngleChange.AddListener(RetrieveAngle);
    }
    void OnDisable(){
        angleSO.bulletAngleChange.RemoveListener(RetrieveAngle);
    }
    void RetrieveAngle(float angledData){
        upAngle =  angledData;
    }
}
