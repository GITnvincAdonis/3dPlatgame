using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour,IBulletBreak
{
    public void destroy(){
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
    }
  
}
