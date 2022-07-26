using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class AnimationTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    Animation toplay;

    private bool beenactivated = false;
    void Start()
    {
        toplay = GetComponent<Animation>();
    }

    public void TriggerAnimation()
    {
        if(!beenactivated)
        { 
            toplay.Play();
            
            AudioSource AS = GetComponent<AudioSource>();
            if (AS != null)
                AS.Play();
        
        }
        beenactivated = true;
        
    
    }



}