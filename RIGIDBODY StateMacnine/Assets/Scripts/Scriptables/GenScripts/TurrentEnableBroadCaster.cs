using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrentEnableBroadCaster : MonoBehaviour
{
    public TurrentActivationManangement turretSO;
    void OnTriggerEnter(Collider col){
        if(LayerMask.LayerToName(col.gameObject.layer) == "Player" ){
            Debug.Log("o");
            turretSO.HasEnteredRange();
        }
        
    }
    void OnTriggerExit(Collider col){
        if(LayerMask.LayerToName(col.gameObject.layer) == "Player" ){
            turretSO.HasLeftRange();
        }

    }
}
