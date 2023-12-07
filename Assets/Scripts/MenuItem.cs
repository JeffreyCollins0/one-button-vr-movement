using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuItem : MonoBehaviour{
    [SerializeField] private int targetScene = 0;

    public void tap(){
        SceneManager.LoadScene(targetScene);
    }
}
