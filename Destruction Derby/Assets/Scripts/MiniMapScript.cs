using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour {

	public Transform player;

	void LateUpdate(){
		Vector3 position = player.position;
		position.y = transform.position.y;
		transform.position = position;

		transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
	}
}
