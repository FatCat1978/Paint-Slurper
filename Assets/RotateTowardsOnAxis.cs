using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsOnAxis : MonoBehaviour
{
    //tldr rotates only on one axis towards the player, or, rather, the main camera.

    // Start is called before the first frame update

    Camera target;

    [SerializeField] bool rotateX;
    [SerializeField] bool rotateY;
    [SerializeField] bool rotateZ;
    
    void Start()
    {
        target = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            Vector3 OldRot = transform.eulerAngles;

            transform.LookAt(target.transform);

            Vector3 NewRot = transform.eulerAngles;

            if (!rotateX)
                NewRot.x = OldRot.x;
            if(!rotateY)
                NewRot.y = OldRot.y;
            if(!rotateZ)
                NewRot.z = OldRot.z;

            transform.eulerAngles = NewRot;

        }
    }
}
