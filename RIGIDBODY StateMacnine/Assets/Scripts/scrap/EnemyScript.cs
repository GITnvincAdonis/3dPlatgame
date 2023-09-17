using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour,IDamagable
{
    public Transform player;
    Rigidbody rb;
    float health = 100f;
    bool isDead;
    float knockBackForce= 5f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void tookLighthit(){
        Debug.Log("took hit");
        health -= 20f;
        if(health <=0f){
            Debug.Log("enenmy died");
            // run death logic
            /*
            animation plays
            collider is disabled and maybe msh renderer
            done using coroutine
            */
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
        }
        
        
    }
    public void tookHeavyhit(Vector3 thing)
    {
        Debug.Log("took hit");
        health -= 20f;
        if (health <= 0f)
        {
            Debug.Log("enenmy died");
            // run death logic
            /*
            animation plays
            collider is disabled and maybe msh renderer
            done using coroutine
            */
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
        }


    }
    public void TookKnockBack(){
        rb.AddForce(player.forward * knockBackForce * 10000f, ForceMode.VelocityChange);

        
    }
}

/*

enemy state machine

idle
chase
await
attack
*/
