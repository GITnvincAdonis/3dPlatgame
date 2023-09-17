using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraActivate : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera thisCam;
   
    void OnTriggerEnter(Collider col){
        if(LayerMask.LayerToName(col.gameObject.layer) == "Player" ){
            thisCam.enabled =true;
        }
        
    }
    void OnTriggerExit(Collider col){
        if(LayerMask.LayerToName(col.gameObject.layer) == "Player" ){
           thisCam.enabled =false;
        }

    }
}
