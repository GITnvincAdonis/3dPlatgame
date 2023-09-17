using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


enum SceneName{
    HomeRoom,
    StarterRoom,
    OutsideRoom,
}

[CreateAssetMenu(menuName = "Events/levelSwitchSO")]
public class LvlSwitchSO : ScriptableObject
{
    
    public UnityEvent<string> switchedEvent;
    public UnityEvent<float[]> switchRespawnPoint;
    public string gameSceneToSwitchTo;
    public float[] respawnVector;
    Dictionary<SceneName, string> names = new Dictionary<SceneName, string>
    {
        { SceneName.HomeRoom, "SampleScene" },
        { SceneName.StarterRoom, "StartingRoom" },
        { SceneName.OutsideRoom, "OutsideWorld"},
        // ...
    };
    void OnEnable(){
        if(switchedEvent == null){
            switchedEvent = new UnityEvent<string>();
        }
        if(switchRespawnPoint == null){
            switchRespawnPoint = new UnityEvent<float[]>();
        }
    }



    //room switching functions. Call to switch to change the corresponding events
    //  must include:
    // - name of event to switch to (gameSceneToSwitchTo)
    // - spawn position (respawn vector)
    public void StartRoomSwitch(){
        respawnVector = new float[3];
        respawnVector[0]=  -2f;
        respawnVector[1] = 4f;
        respawnVector[2] = 0f;
        
        gameSceneToSwitchTo = names[SceneName.StarterRoom];
        switchedEvent.Invoke(gameSceneToSwitchTo);
        switchRespawnPoint.Invoke(respawnVector);
    }
    public void HomeRoomSwitch(){
        respawnVector = new float[3];
        respawnVector[0]=  0f;
        respawnVector[1] = 2f;
        respawnVector[2] = 0f;
        
        gameSceneToSwitchTo = names[SceneName.HomeRoom];
        switchedEvent.Invoke(gameSceneToSwitchTo);
        switchRespawnPoint.Invoke(respawnVector);
    }
    public void OutsideRoomSwitch(){
        respawnVector = new float[3];
        respawnVector[0]=  0f;
        respawnVector[1] = 5f;
        respawnVector[2] = 0f;
        
        gameSceneToSwitchTo = names[SceneName.OutsideRoom];
        switchedEvent.Invoke(gameSceneToSwitchTo);
        switchRespawnPoint.Invoke(respawnVector);
    }
   

    
}
