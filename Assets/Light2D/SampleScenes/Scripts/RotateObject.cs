using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour 
{
    public float speed = 5.0f;

	void Update () 
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
	}
}
