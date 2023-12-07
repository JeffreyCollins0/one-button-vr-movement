using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoFreelook : MonoBehaviour{
    [SerializeField] private float debugKeyboardSensitivity = 1.1f;
    [SerializeField] private LineRenderer lookIndicator = null;
    [SerializeField] private bool invertYAxis = false;

    private Vector3 lookDirection = Vector3.forward;

    void Update(){
        // keyboard input
        int invertMultiplier = 1;
        if(invertYAxis){
            invertMultiplier = -1;
        }

        Vector2 input = Vector2.zero;
        if(Input.GetKey("w")){
            input += invertMultiplier*(Vector2.down * debugKeyboardSensitivity);
        }
        if(Input.GetKey("s")){
            input += invertMultiplier*(Vector2.up * debugKeyboardSensitivity);
        }
        if(Input.GetKey("a")){
            input += (Vector2.left * debugKeyboardSensitivity);
        }
        if(Input.GetKey("d")){
            input += (Vector2.right * debugKeyboardSensitivity);
        }

        float turnSpeed = (3200f * Mathf.Deg2Rad * Time.deltaTime / Time.timeScale);
        Vector3 horizontalDirection = (Quaternion.Euler(0f, turnSpeed*input.x, 0f) * clip(lookDirection, true, false, true));
        Vector3 verticalDirection = (Quaternion.Euler(turnSpeed*input.y, 0f, 0f) * lookDirection);
        verticalDirection = clip(verticalDirection, false, true, false);
        lookDirection = (horizontalDirection + verticalDirection).normalized;
        
        // turn indicator, if it exists
        if(lookIndicator != null){
            Vector3[] newPositions = {Vector3.zero, (lookDirection * 1.5f)};
            lookIndicator.SetPositions(newPositions);
        }
    }

    public Vector3 getLookDirection(){
        return ((lookDirection != Vector3.zero) ? lookDirection : Vector3.forward);
    }

    public void setLookDirection(Vector3 newDirection){
        lookDirection = newDirection;
        if(lookIndicator != null){
            Vector3[] newPositions = {Vector3.zero, lookDirection.normalized};
            lookIndicator.SetPositions(newPositions);
        }
    }

    private Vector3 clip(Vector3 vector, bool keepX, bool keepY, bool keepZ){
        return new Vector3(
            (keepX ? vector.x : 0f),
            (keepY ? vector.y : 0f),
            (keepZ ? vector.z : 0f)
        );
    }
}
