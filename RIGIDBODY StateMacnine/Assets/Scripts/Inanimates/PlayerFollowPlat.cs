using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowPlat : MonoBehaviour
{
    [SerializeField] LayerMask playerMask;
    int layerNum;
    // Start is called before the first frame update
    void Start()
    {
        layerNum = playerMask;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision col){
        var entityTouching = col.gameObject;
        if(LayerMask.LayerToName(entityTouching.layer) == "Player"){
            entityTouching.transform.SetParent(transform);
        }
         

    }
    void OnCollisionExit(Collision col){
        var entityTouching = col.gameObject;
        
            entityTouching.transform.SetParent(null);
        

    }
}
