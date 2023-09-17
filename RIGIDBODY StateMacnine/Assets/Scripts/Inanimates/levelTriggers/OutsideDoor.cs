using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class OutsideDoor : MonoBehaviour,IInteractable
{
    public LvlSwitchSO sceneSwitchSO;
 
    public void interact(){
        if(sceneSwitchSO != null){
            sceneSwitchSO.OutsideRoomSwitch();
        }
    }

}