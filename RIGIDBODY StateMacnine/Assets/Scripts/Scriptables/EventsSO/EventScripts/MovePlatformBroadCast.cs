using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Scriptable Objects/PlatActivators")]
public class MovePlatformBroadCast : ScriptableObject
{
    
    public UnityAction walkedOnButton;

    public void OnWalkOn(){
        walkedOnButton?.Invoke();
    }
}
