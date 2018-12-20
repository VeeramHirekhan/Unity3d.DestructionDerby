using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour 
{
    // public GameObject player;

    // private Vector3 offset; 

    // // Use this for initialization
    // void Start () 
    // {
    //     offset = transform.position - player.transform.position;
    // }
    
    // void LateUpdate () 
    // {
    //     transform.position = player.transform.position + offset;
	// 	//transform.position = Player.transform.position + _offset;
    //  	transform.rotation = player.transform.rotation;
    // }

        public Transform target;
        public float distance = 8.0f;
        public float height = 3.0f;

        [Range(.01f, 15.0f)]
        public float damping = 5.0f;
        public bool smoothRotation = true;
        public bool followBehind = true;
        
        [Range(.01f,15.0f)]
        public float rotationDamping = 6.5f;

        void LateUpdate () {
               Vector3 wantedPosition;
               if(followBehind)
                       wantedPosition = target.TransformPoint(0, height, -distance);
               else
                       wantedPosition = target.TransformPoint(0, height, distance);
     
               transform.position = Vector3.Lerp (transform.position, wantedPosition, Time.deltaTime * damping);

               if (smoothRotation) {
                       Quaternion wantedRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
                       transform.rotation = Quaternion.Slerp (transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
               }
               else transform.LookAt (target, target.up);
         }
}