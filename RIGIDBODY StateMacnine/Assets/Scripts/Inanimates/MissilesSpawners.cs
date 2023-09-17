using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilesSpawners : MonoBehaviour
{
     public MissileMovement data;
      public TurrentActivationManangement turretSO;
     bool inrange = false;
     bool canShoot= false;
     bool isSpawning;
     private Coroutine spawningCoroutine;
    [SerializeField] GameObject prefab;
    [SerializeField] Transform spwanPoint;
    
    float _deathSpan;
    
    // Start is called before the first frame update
    void Start()
    {
        _deathSpan = data.deathSpan;
        AcceptBoolState(turretSO.isWithin);
        StartCoroutine(UpdateCanShoot());
         inrange = false;
         canShoot= false;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnBullets();
        /*
        Debug.Log(inrange);
        Debug.Log(canShoot);
        */
    
    }


    private void SpawnBullets()
    {
        if(inrange && canShoot){
            var missile = Instantiate(prefab, spwanPoint.position, spwanPoint.rotation);
            //set target durng runtime, as manually setting target requires prefab to be set
        }
    }

    private IEnumerator UpdateCanShoot(){
        while(true){
            yield return null;
            canShoot = true;
            yield return null;
            canShoot = false;
            yield return new WaitForSeconds(_deathSpan);}
    }
       
    
    private void OnEnable(){
    turretSO.Entered.AddListener(AcceptBoolState);
   }

    private void OnDisable(){
        turretSO.Entered.RemoveListener(AcceptBoolState);  
    }

   public void AcceptBoolState(bool state){
    inrange = state;
    }
}