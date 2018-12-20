using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanningCarerma : MonoBehaviour {


    public Transform[] target;
    public float speed;
    public Transform focus;

    private int current;
	
	// Update is called once per frame
	void Update () {
        if (transform.position != target[current].position)
        {
            transform.LookAt(focus);
            Vector3 pos = Vector3.MoveTowards(transform.position, target[current].position, speed * Time.deltaTime);
            GetComponent<Rigidbody>().MovePosition(pos);
        }
        else
        {
            current = (current + 1) % target.Length;
        }
	}
}
