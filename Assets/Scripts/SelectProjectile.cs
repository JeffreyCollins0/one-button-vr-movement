using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectProjectile : MonoBehaviour{
    [SerializeField] private float lifeTime = 0.8f;

    void Update(){
        if(lifeTime > 0){
            lifeTime -= Time.deltaTime;
            if(lifeTime <= 0){
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision other){
        MenuItem selectedItem = other.collider.gameObject.GetComponent<MenuItem>();
        if(selectedItem != null){
            selectedItem.tap();
            Destroy(gameObject);
        }
    }
}
