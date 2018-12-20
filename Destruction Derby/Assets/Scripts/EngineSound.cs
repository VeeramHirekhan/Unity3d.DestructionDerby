using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSound : MonoBehaviour {

    private AudioSource theEngine;
    public AudioClip engineSound;
    private float speed;
    private float maxPitchMod;
    private float minPitchMod;

	// Use this for initialization
	void Awake () {

        theEngine = GetComponent<AudioSource>();


	}
	
	// Update is called once per frame
	void LateUpdate () {

       
        if (Input.GetKey("up"))
        {
            theEngine.pitch += .05F;


        }
        else if (Input.GetKey("down")) {

            theEngine.pitch -= .05F;

        }

        if (!theEngine.isPlaying)
        {
            theEngine.PlayOneShot(engineSound,1F);

        }

    }
}
