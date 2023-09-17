using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "ScriptableObject/PlayerAimData")]
public class PlayerBulletsBroadcast : ScriptableObject
{

    //limiting range magnitude of upward force
    [Range(0,40)][SerializeField]private float _upWardAngle;
    public float upWardAngle {
        get{return _upWardAngle;}
        set
        {
            _upWardAngle = value;
            if(_upWardAngle > 40f)
                {
                _upWardAngle = 40f;
                }
        }
        
        }
    public UnityEvent<float> bulletAngleChange;
  
    
    void OnEnable(){
        if(bulletAngleChange == null){
            bulletAngleChange = new UnityEvent<float>();
        }
    }
    public void IncreaseAngle(){
        upWardAngle += 12f*Time.deltaTime;
        bulletAngleChange.Invoke(upWardAngle);
    }
    public void ResetAngle(){
        upWardAngle = 0f;
        bulletAngleChange.Invoke(upWardAngle);
    }
}
