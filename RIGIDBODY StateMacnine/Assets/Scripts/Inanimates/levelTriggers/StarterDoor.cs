using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StarterDoor : MonoBehaviour,IInteractable
{
    public LvlSwitchSO sceneSwitchSO;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void interact(){
        if(sceneSwitchSO != null){
            sceneSwitchSO.StartRoomSwitch();
        }
    }

    
}
