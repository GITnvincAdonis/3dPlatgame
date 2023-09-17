using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager script;
    public LvlSwitchSO sceneSwitcher;
    string currentLevel;
    void Awake(){
        if(script != null){
            Destroy(gameObject);
            return;
        }
        script = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        retrieveLvlToSwitch(sceneSwitcher.gameSceneToSwitchTo);
    }
    void OnEnable(){
        sceneSwitcher.switchedEvent.AddListener(retrieveLvlToSwitch);
    }
    void OnDisable(){
        sceneSwitcher.switchedEvent.RemoveListener(retrieveLvlToSwitch);
    }
    public async void retrieveLvlToSwitch(string name){
        if(name != currentLevel){
        var scene = SceneManager.LoadSceneAsync(name);
        currentLevel = name;
        }
        

    }
}
