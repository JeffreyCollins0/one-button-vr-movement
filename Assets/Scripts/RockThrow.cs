using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class RockThrow : MonoBehaviour{
    [SerializeField] private float launchForce = 10f;
    [SerializeField] private float minimumVelocityCutoff = 0.1f;
    [SerializeField] private float reorientDuration = 0.8f;
    [SerializeField] private bool useDebugControls = false;
    [SerializeField] private CameraFollow camControl;
    [SerializeField] private MeshRenderer mesh;
    private LineRenderer launchIndicator;
    private Rigidbody rb;
    private PseudoFreelook debugLook;

    private Vector3 launchVec = Vector3.forward;
    private bool thrown = false;
    private float reorientTime = 0f;
    private float savedFixedDeltaTime = 0.02f;

    void Start(){
        launchIndicator = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
        debugLook = GetComponent<PseudoFreelook>();
    }

    void Update(){
        Vector3 inputLook = Camera.main.transform.forward;
        if(useDebugControls){
            inputLook = debugLook.getLookDirection();
        }
        camControl.SetLookDirection(inputLook);

        // thrown reset
        if(thrown && rb.velocity.magnitude <= minimumVelocityCutoff){
            thrown = false;
            mesh.enabled = false;
            camControl.LockToObject();
        }

        // tap input
        if(Input.GetMouseButtonDown(0)){
            if(!thrown){
                // throw self
                thrown = true;
                rb.velocity = inputLook.normalized * launchForce;
                mesh.enabled = true;
                reorientTime = 0f;
                Time.timeScale = 1.0f;
                Time.fixedDeltaTime = savedFixedDeltaTime;
                thrown = true;
            }else{
                // quick re-orient
                thrown = false;
                Time.timeScale = 0.3f;
                Time.fixedDeltaTime = (savedFixedDeltaTime * 0.3f);
                reorientTime = reorientDuration;

                Vector3 clippedLookRot = inputLook;
                if(useDebugControls){
                    clippedLookRot = new Vector3(inputLook.x, 0f, inputLook.z);
                    debugLook.setLookDirection(clippedLookRot);
                }
                Quaternion defaultLookRot = Quaternion.identity;
                defaultLookRot.SetLookRotation(clippedLookRot);
                transform.rotation = defaultLookRot;
            }
        }

        // time scaling
        if(reorientTime > 0f){
            reorientTime -= Time.deltaTime;

            if(reorientTime <= 0f){
                reorientTime = 0f;
                Time.timeScale = 1.0f;
                Time.fixedDeltaTime = savedFixedDeltaTime;
                thrown = true;
            }
        }
    }
}
