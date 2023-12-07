using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WarpItemMove : MonoBehaviour{
    public static UnityAction ItemDestroyEvent;

    void OnCollisionEnter(Collision other){
        if(!other.collider.CompareTag("Player")){
            ItemDestroyEvent();
            Destroy(gameObject);
        }
    }

}
