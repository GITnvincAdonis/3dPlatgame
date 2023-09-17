using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : PlayerBaseState
{

    private float timeSinceLastAttack = 0f;
    //float timeheld = 0f;
    Vector3 directionToTarget;
 
   
    
    public override void EnterState(PlayerStateMachine state){
        
       Debug.Log("hit fight button");

        timeSinceLastAttack =0f;
        
        
      
        
        
       
        
        
      
        
        
        
    }
    public override void UpdateState(PlayerStateMachine state){
      
        
        
        GetInputs();
        
        PlayerInRange(state);
        
        CalcKnowbackDirection(state);
        
        
        HandleHeavyDamage(state);
        XYMovementYRotation(state); 
        
        GroundCheck(state);
        timeSinceLastAttack += Time.deltaTime;
        state.Anim.SetBool("IsFighting", false);
        state.SwitchState(state.groundState);


        //PerformHeavy(state);
        //Debug.Log(timeheld);
        //CheckSwitchState(state);


        /*
            if (InSightRange){
                
                if(timeheld >=2f)
                {   handleAttack(state);
                    PerformHeavy(state);
                    HandleHeavyDamage(state);           
                    
                    
                    timeheld = -2f;
                    
                }
                if(timeSinceLastAttack < 0.2f && Input.GetMouseButtonUp(1))
                {   
                    handleAttack(state);
                    PerformLight(state);
                    HandleLightDamage(state);  
                    timeheld = -2f;
                }
                
            }
            else if(!InSightRange){
                if(timeheld >2f)
                {   PerformHeavy(state);
                    timeheld = -2f;}
                else if(timeSinceLastAttack < 0.3f && Input.GetMouseButtonUp(1))
                {
                    PerformLight(state);}
                    timeheld = -2f;
            }
        
            if(Input.GetMouseButton(1)){
                timeheld += 3 * Time.deltaTime;
            }
            else{
                timeheld = 0f;
            }
           



*/        
        
    }
     public override void TriggerEnter(PlayerStateMachine state,Collider col){
         if(LayerMask.LayerToName(col.gameObject.layer)== "waterLayer"){
            state.Anim.SetBool("Inwater", true);
            state.PlayerContainer.transform.position = new Vector3(state.PlayerContainer.transform.position.x,col.transform.position.y,state.PlayerContainer.transform.position.z);
            state.SwitchState(state.swimmingState);
        }
     }
    public override void TriggerStay(PlayerStateMachine state,Collider col){}
    public override void TriggerExit(PlayerStateMachine state, Collider col){}
    public override void CollisionEnter(PlayerStateMachine state, Collision col){}

    public override void CollisionStay(PlayerStateMachine state, Collision col){}

    void HandleHeavyDamage(PlayerStateMachine state){
        if(InAttackRange){
        Collider[] hitColliders  = Physics.OverlapSphere(state.PlayerBod.position, state.PlayerAttackRange, state.EnemyMask);
            foreach (Collider hitCollider in hitColliders){
                Debug.Log(hitCollider);
                var thingHits = hitCollider.GetComponent<IDamagable>();
                if (thingHits != null){
                    Debug.Log("hi");
                    thingHits.tookHeavyhit(directionToTarget.normalized);
                }
            }
        }  
    }
    
    void CheckSwitchState(PlayerStateMachine state){
        if (timeSinceLastAttack > 0.2f || _jumpPress||!_isGrounded){
            state.SwitchState(state.groundState);
        }
    }
    void CalcKnowbackDirection(PlayerStateMachine state){
        if(InAttackRange){
            directionToTarget = closest.transform.position - new Vector3(state.PlayerContainer.position.x, closest.transform.position.y, state.PlayerContainer.position.z );
        }
        

        // Make the player look at the target's position.
        //state.PlayerOrientation.forward = directionToTarget.normalized;
        //state.PlayerBod.rotation = Quaternion.LookRotation(directionToTarget.normalized, Vector3.up);
        

    }
    IEnumerator StateSwitchDelay(PlayerStateMachine state){
        
        yield return new WaitForSeconds(3.1f);
    }
    void PerformHeavy(PlayerStateMachine state){
        
        
        //heavy animation
        //addmore force
        
        state.RigidBod.velocity = Vector3.zero;
        if(OnSlope(state)){
            state.RigidBod.AddForce(GetSlopeMoveDir() * state.HeavyAttackDist, ForceMode.Force);
        }
        else{
            state.RigidBod.AddForce(new Vector3(state.PlayerBod.forward.x, 0f, state.PlayerBod.forward.z) * state.HeavyAttackDist, ForceMode.Force);
        }
       
        

        
     
        timeSinceLastAttack =0f;
       

        
    }
    void PerformLight(PlayerStateMachine state){
        
        //light animation
        //add small force
        
        
        state.RigidBod.velocity = Vector3.zero;
        if(OnSlope(state)){
            state.RigidBod.AddForce(GetSlopeMoveDir() * state.LightAttackDist, ForceMode.Force);
        }
        else{
            state.RigidBod.AddForce(new Vector3(state.PlayerBod.forward.x, 0f, state.PlayerBod.forward.z) * state.LightAttackDist, ForceMode.Force);
        }
        
        
        timeSinceLastAttack += 0.4f;
        
        

        
        
    }
    void HandleLightDamage(PlayerStateMachine state){
        if(InAttackRange){
            Vector3 forwardPos = state.PlayerBod.position + state.PlayerBod.forward * 1.5f;

            Collider[] hitColliders = Physics.OverlapSphere(forwardPos, state.PlayerAttackRange, state.EnemyMask);

            foreach (Collider hitCollider in hitColliders)
            {
                Debug.Log(hitCollider);

                var thingHits = hitCollider.GetComponent<IDamagable>();
                if (thingHits != null)
                {
                    Debug.Log("hi");
                    thingHits.tookLighthit();
                }

            }
        }
        



    }

}

