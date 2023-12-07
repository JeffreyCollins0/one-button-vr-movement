using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour{
    [SerializeField] private float followAmount = 0.2f;
    [SerializeField] private float minimumCameraDistance = 0.3f;
    [SerializeField] private float maximumCameraDistance = 3f;
    [SerializeField] private float cameraElevation = 32f;
    [SerializeField] private GameObject objectToFollow = null;
    [SerializeField] private GameObject followCamera;

    private Vector3 lookDirection = Vector3.forward;
    private bool lockToInsideObject = false;

    void Update(){
        // track position to tracking object
        if(objectToFollow != null){
            Vector3 vectorToTarg = (objectToFollow.transform.position - transform.position);
            if(vectorToTarg.magnitude >= 0.1f){
                transform.position += (vectorToTarg * followAmount);
            }else{
                transform.position = objectToFollow.transform.position;
            }
        }

        // reposition the camera on a spring arm
        Vector3 camPos = transform.position;

        if(!lockToInsideObject){
            RaycastHit hit;
            if(Physics.Raycast(transform.position, -lookDirection, out hit, maximumCameraDistance)){
                camPos = hit.point + (Vector3.up * 0.05f);
            }else{
                camPos = transform.position - (lookDirection * maximumCameraDistance);
            }
        }

        // lerp position to desired position
        followCamera.transform.position = camPos;
        Quaternion viewRotation = Quaternion.identity;
        viewRotation.SetLookRotation(lookDirection);
        followCamera.transform.rotation = viewRotation;
    }

    public void SetLookDirection(Vector3 newLookDir){
        lookDirection = newLookDir;
    }

    public void LockToObject(){
        lockToInsideObject = true;
    }

    public void UnlockFromObject(){
        lockToInsideObject = false;
    }
}
