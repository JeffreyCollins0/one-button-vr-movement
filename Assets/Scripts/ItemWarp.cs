using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class ItemWarp : MonoBehaviour{
    [SerializeField] private float launchForce = 10f;
    [SerializeField] private float minimumVelocityCutoff = 0.1f;
    [SerializeField] private bool useDebugControls = false;
    [SerializeField] private CameraFollow camControl;
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private GameObject warpItemPrefab;
    [SerializeField] private Camera cam;
    private Rigidbody rb;
    private PseudoFreelook debugLook;

    private bool thrown = false;
    private GameObject thrownItem = null;
    private Vector3 warpLocation = Vector3.zero;

    // for FOV distortion effect
    private float defaultNearDist;
    private float defaultFOV;
    private float defaultNearWidth;
    private float FOVMultiplier = 1f;
    private float transTime = 0f;

    void Start(){
        rb = GetComponent<Rigidbody>();
        debugLook = GetComponent<PseudoFreelook>();
        WarpItemMove.ItemDestroyEvent += this.onItemDestroy;

        defaultNearDist = cam.nearClipPlane;
        defaultFOV = cam.fieldOfView;
        defaultNearWidth = (2f * defaultNearDist * Mathf.Tan(defaultFOV / 2f));
    }

    void Update(){
        // look input
        Vector3 inputLook = Camera.main.transform.forward;
        if(useDebugControls){
            inputLook = debugLook.getLookDirection();
        }
        camControl.SetLookDirection(inputLook);

        // tap input
        if(Input.GetMouseButtonDown(0) && transTime == 0){
            if(!thrown){
                // throw self
                thrown = true;
                thrownItem = Instantiate(warpItemPrefab, transform);
                thrownItem.transform.position = transform.position + inputLook.normalized;
                Rigidbody thrownRb = thrownItem.GetComponent<Rigidbody>();
                thrownRb.velocity = inputLook.normalized * launchForce;
            }else{
                // warp to item
                warpLocation = thrownItem.transform.position;
                Destroy(thrownItem);
                thrownItem = null;
                thrown = false;
                transform.position = warpLocation;
            }
        }
    }

    void onItemDestroy(){
        thrown = false;
        thrownItem = null;
    }
}
