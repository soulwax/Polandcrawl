using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour {

	public float turnSpeed = 10.0f;
	public float moveSpeed = 10.0f;


	private float x, y;
	private Transform cam;

	// Use this for initialization
	void Start () {
		cam = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		x = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
		y = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
		
		cam.Translate(x, y, 0);
	}
}
