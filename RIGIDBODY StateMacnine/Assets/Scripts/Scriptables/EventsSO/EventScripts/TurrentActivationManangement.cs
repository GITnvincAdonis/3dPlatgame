using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Objects/Turret Activators")]
public class TurrentActivationManangement : ScriptableObject
{
    public bool isWithin;
    public bool isOut;
    public UnityEvent<bool> Entered;
    public UnityEvent<bool> Exited;
    
    private void OnEnable(){
        isWithin =false;
        if(Entered ==null){
            Entered = new UnityEvent<bool>();
        }
         if(Exited==null){
            Exited = new UnityEvent<bool>();
        }
    }
    public void HasEnteredRange(){
        isWithin = true;
      
        
        Entered.Invoke(isWithin);
    }
    public void HasLeftRange(){
        isWithin = false;
      
        
        Entered.Invoke(isWithin);
    }

}
