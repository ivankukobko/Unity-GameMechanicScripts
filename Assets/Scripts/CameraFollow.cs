using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public GameObject target;
	public Vector3 offset = new Vector3(0f, -10f, 10f);
	public float pitch = 1f; 

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, target.transform.position - offset, Time.deltaTime);
		transform.LookAt (target.transform.position + Vector3.up * pitch);
	}
}
