using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelector : MonoBehaviour{
    [SerializeField] private bool useDebugControls = false;
    [SerializeField] private CameraFollow camControl;
    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject selectPrefab;
    [SerializeField] private TextMesh debugOut;
    private PseudoFreelook debugLook;
    private Vector3 checkExtents = new Vector3(0.1f, 0.1f, 0.1f);
    
    void Start(){
        debugLook = GetComponent<PseudoFreelook>();
    }

    void Update(){
        // look input
        Vector3 inputLook = Camera.main.transform.forward;
        if(useDebugControls){
            inputLook = debugLook.getLookDirection();
        }
        camControl.SetLookDirection(inputLook);

        // tap input
        if(Input.GetMouseButtonDown(0)){
            // send projectile to collide (compiler doesn't like raycasting)
            GameObject sentCheck = Instantiate(selectPrefab);
            sentCheck.transform.position = transform.position;
            sentCheck.GetComponent<Rigidbody>().velocity = (inputLook.normalized * 6.2f);
        }
    }
}
