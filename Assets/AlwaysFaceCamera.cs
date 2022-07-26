using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour
{
    private Camera toFace;
    // Start is called before the first frame update
    void Start()
    {
        toFace = Camera.main; //assign the camera we're facing
    }

    // Update is called once per frame
    void Update() 
    { //match the forward vectors, this makes it face the camera. always.
        transform.forward = toFace.transform.forward;
    }
}
