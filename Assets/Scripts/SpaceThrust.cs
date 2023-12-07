using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class SpaceThrust : MonoBehaviour{
    [SerializeField] private float launchForce = 5f;
    [SerializeField] private bool useDebugControls = false;
    [SerializeField] private CameraFollow camControl;
    [SerializeField] private MeshRenderer mesh;
    private LineRenderer launchIndicator;
    private Rigidbody rb;
    private PseudoFreelook debugLook;

    void Start(){
        launchIndicator = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
        debugLook = GetComponent<PseudoFreelook>();
    }

    void Update(){
        // get look direction
        Vector3 inputLook = Camera.main.transform.forward;
        if(useDebugControls){
            inputLook = debugLook.getLookDirection();
        }
        camControl.SetLookDirection(inputLook);

        // tap input
        Vector3 velocityDir = rb.velocity.normalized;
        if(Input.GetMouseButton(0)){
            // apply thrust
            Vector3 newVelDir = (rb.velocity + (inputLook.normalized * launchForce));
            rb.velocity = newVelDir.normalized * Mathf.Min(newVelDir.magnitude, 10f);
        }
        if(velocityDir.magnitude > 0){
            Quaternion defaultLookRot = Quaternion.identity;
            defaultLookRot.SetLookRotation(velocityDir);
            transform.rotation = defaultLookRot;
        }
    }
}
