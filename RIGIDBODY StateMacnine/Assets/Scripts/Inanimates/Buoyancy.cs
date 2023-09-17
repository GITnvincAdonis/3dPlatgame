using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Buoyancy : MonoBehaviour
{

    public Transform[] surfacePoints;

    [SerializeField] private float waterDrag = 3f;
    [SerializeField] private float waterAngularDrag = 1f;

    [SerializeField] private float airDrag = 0f;
    [SerializeField] private float airAngularDrag = 0.05f;
    [SerializeField] private float BuoyantForce =10f;
     
    float surfaceHeight;

    Rigidbody rb;
    bool underWater;
    int floaterUnderWater;
    
    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        
    }
  
    void OnTriggerStay(Collider col){
        if(LayerMask.LayerToName(col.gameObject.layer)== "waterLayer"){
        rb.drag = waterDrag;
        floaterUnderWater =0;
        for(int i = 0; i< surfacePoints.Length; i++){
            
            if(surfaceHeight == 0){
                surfaceHeight = col.bounds.max.y;
                //Debug.Log(surfaceHeight);
            }   
            
            
            
            float surfDiff = surfacePoints[i].position.y - surfaceHeight;
            

            if(surfDiff < 0){
                rb.AddForceAtPosition(Vector3.up * BuoyantForce * Mathf.Abs(surfDiff) * Time.fixedDeltaTime, surfacePoints[i].position, ForceMode.Force);
                floaterUnderWater ++;
                if(!underWater){
                    underWater = true;
                    SwitchDrag(underWater);
                }


            } 
            
        }
        if(underWater && floaterUnderWater == 0){
                underWater =false;
                SwitchDrag(underWater);
            }
        }
    }
    void OnTriggerExit(Collider col){
        if(LayerMask.LayerToName(col.gameObject.layer)== "waterLayer"){
            surfaceHeight =0;
        }

    }
    void SwitchDrag(bool under){
        if(under){
            rb.drag = waterDrag;
            rb.angularDrag = waterAngularDrag;
        }
        else{
            rb.drag = airDrag;
            rb.angularDrag = airAngularDrag;
        }
    }
   
}
