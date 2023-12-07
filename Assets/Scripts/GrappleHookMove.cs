using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrappleHookMove : MonoBehaviour{
    [SerializeField] private float moveSpeed = 0.4f;
    public static UnityAction GrappleAttachEvent;

    private Transform[] lineObjects = null;
    private Vector3 moveDirection = Vector3.zero;
    private bool grappled = false;

    void Start(){
        lineObjects = new Transform[transform.childCount];
        for(int i=0; i<transform.childCount; i++){
            lineObjects[i] = transform.GetChild(i);
        }
    }

    void Update(){
        if(!grappled){
            transform.position = transform.position + (moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision other){
        if(!other.collider.CompareTag("Player")){
            GrappleAttachEvent();
            grappled = true;
        }
    }

    public void detach(){
        Destroy(gameObject);
    }

    public void launch(Vector3 direction){
        moveDirection = direction;

        Quaternion selfRotation = Quaternion.identity;
        Quaternion hookRotation = Quaternion.Euler(90f, 0f, 0f);
        selfRotation.SetLookRotation(direction);
        transform.rotation = selfRotation * hookRotation;
    }

    public void updateLine(Vector3 playerPosition){
        Quaternion newRotation = Quaternion.identity;
        Quaternion hookRotation = Quaternion.Euler(90f, 0f, 0f);
        newRotation.SetLookRotation((transform.position-playerPosition).normalized);
        transform.rotation = newRotation * hookRotation;

        for(int i=0; i<transform.childCount; i++){
            Transform child = lineObjects[i];
            Vector3 childOffset = (transform.position - playerPosition) * ( (float)i / (float)transform.childCount );
            child.position = playerPosition + childOffset;
            child.rotation = newRotation;
        }
    }
}
