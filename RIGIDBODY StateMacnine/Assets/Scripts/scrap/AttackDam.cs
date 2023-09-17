using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDam : MonoBehaviour
{
    void OnTriggerEnter(Collider col){
        var CanBehit = col.GetComponent<IDamagable>();
        if(CanBehit !=null){
            CanBehit.tookLighthit();
            CanBehit.TookKnockBack();
        }
    }
}
