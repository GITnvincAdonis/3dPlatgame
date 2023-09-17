
using UnityEngine;

public class PlatformActivators : MonoBehaviour
{
    public MovePlatformBroadCast platformSO;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision col){
        if(platformSO != null && col.collider.CompareTag("Player")){
            Debug.Log("true");
            platformSO.OnWalkOn();
        }
    }

}
