using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPatforming : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision col){
        if(col.collider.CompareTag("Player")){
            col.transform.SetParent(transform);
        }
    }
     void OnCollisionExit(Collision col){
        if(col.collider.CompareTag("Player")){
            col.transform.SetParent(null);
        }
    }
   
}



