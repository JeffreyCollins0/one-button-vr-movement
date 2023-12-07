using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour{
    void Update(){
        transform.LookAt(Camera.main.transform);
        Quaternion rotate = Quaternion.Euler(0f, 180f, 0f);
        transform.rotation = (transform.rotation * rotate);
    }
}
