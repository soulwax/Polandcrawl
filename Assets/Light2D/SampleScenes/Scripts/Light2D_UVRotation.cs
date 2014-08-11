using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light2DEmitter))]
public class Light2D_UVRotation : MonoBehaviour 
{
    public float speed = 5.0f;
    private Light2DEmitter emitter;

    void Start()
    {
        emitter = GetComponent<Light2DEmitter>();
    }

	void Update () 
    {
        emitter.uvVariable += Time.deltaTime * speed;
	}
}
