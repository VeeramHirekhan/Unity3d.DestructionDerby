using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collison : MonoBehaviour {

    public BoxCollider box1;
    public int speed = 5;

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Enemy"){
            print(gameObject.name + " collided with" + col.gameObject.name);
            col.rigidbody.AddForce(transform.forward * speed);
		}

    }
}
