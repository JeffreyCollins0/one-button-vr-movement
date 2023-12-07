using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class GrappleHook : MonoBehaviour{
    [SerializeField] private float minimumDistanceCutoff = 0.52f;
    [SerializeField] private float baseGrappleSpeed = 12f;
    [SerializeField] private float grappleSpeedModulateDuration = 0.6f;
    [SerializeField] private AnimationCurve grappleSpeedCurve;
    [SerializeField] private bool useDebugControls = false;
    [SerializeField] private CameraFollow camControl;
    [SerializeField] private GameObject hookPrefab;
    private Rigidbody rb;
    private PseudoFreelook debugLook;

    private bool launched = false;
    private bool grappled = false;
    private GameObject launchedItem = null;
    private float speedModulateTime = 0f;

    void Start(){
        rb = GetComponent<Rigidbody>();
        debugLook = GetComponent<PseudoFreelook>();
        GrappleHookMove.GrappleAttachEvent += this.onGrapple;
    }

    void Update(){
        // look input
        Vector3 inputLook = Camera.main.transform.forward;
        if(useDebugControls){
            inputLook = debugLook.getLookDirection();
        }
        camControl.SetLookDirection(inputLook);

        // move to the hook if grappled
        if(grappled){
            // accelerate at the beginning of a grapple and fall off near the end
            float grappleSpeed = baseGrappleSpeed * grappleSpeedCurve.Evaluate(1f);
            if(speedModulateTime > 0){
                speedModulateTime -= Time.deltaTime;
                
                grappleSpeed = baseGrappleSpeed * grappleSpeedCurve.Evaluate(1f - (speedModulateTime / grappleSpeedModulateDuration));

                if(speedModulateTime <= 0){
                    speedModulateTime = 0;
                }
            }

            // move towards the target position
            Vector3 grappleDir = (launchedItem.transform.position - transform.position);
            rb.velocity = grappleDir.normalized * Mathf.Min(grappleDir.magnitude, grappleSpeed);

            launchedItem.GetComponent<GrappleHookMove>().updateLine(transform.position);

            // cut off if we get too close
            if(Vector3.Distance(transform.position, launchedItem.transform.position) <= minimumDistanceCutoff){
                launchedItem.GetComponent<GrappleHookMove>().detach();
                launchedItem = null;
                launched = false;
                grappled = false;
            }
        }

        // tap input
        if(Input.GetMouseButtonDown(0)){
            if(!launched && !grappled){
                // launch grappling hook
                launched = true;
                launchedItem = Instantiate(hookPrefab, transform);
                launchedItem.transform.position = transform.position + inputLook.normalized;
                launchedItem.GetComponent<GrappleHookMove>().launch(inputLook.normalized);
            }else{
                // detach from grappling hook
                launchedItem.GetComponent<GrappleHookMove>().detach();
                launchedItem = null;
                launched = false;
                grappled = false;
            }
        }
    }

    void onGrapple(){
        grappled = true;
    }
}
