using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{

    public static DontDestroy script;
    public LvlSwitchSO sceneSwitcher;
    void Awake(){
        if(script != null){
            Destroy(this.gameObject);
            return;

        }
        script = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        retrieveRespawn(sceneSwitcher.respawnVector);
        
    }

    // Update is called once per frame
   
      
    
    void OnEnable(){
        sceneSwitcher.switchRespawnPoint.AddListener(retrieveRespawn);
    }
    void OnDisable(){
        sceneSwitcher.switchRespawnPoint.RemoveListener(retrieveRespawn);
    }
    public void retrieveRespawn(float[] position){
        transform.position = new Vector3(position[0],position[1],position[2]);
         

    }
}
