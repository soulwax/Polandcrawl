using UnityEngine;
using System.Collections;

public class Light2D_EventTest : MonoBehaviour 
{
    void Start()
    {
        Light2DEmitter.OnBeamStay += OnBeamStay;
        Light2DEmitter.OnBeamEnter += OnBeamEnter;
        Light2DEmitter.OnBeamExit += OnBeamExit;
    }

    void OnBeamEnter(GameObject obj, Light2DEmitter emitter)
    {
        if (obj.GetInstanceID() == gameObject.GetInstanceID() && emitter.eventPassedFilter == "BlockChange")
        {
            GetComponent<Renderer>().material.color = emitter.lightColor;
            Debug.Log("Entered: " + emitter.name);
        }
    }
    void OnBeamStay(GameObject obj, Light2DEmitter emitter)
    {
        if (obj.GetInstanceID() == gameObject.GetInstanceID() && emitter.eventPassedFilter == "BlockChange")
        {
            transform.Rotate(0, 0, Time.deltaTime * 50);
        }
    }
    void OnBeamExit(GameObject obj, Light2DEmitter emitter)
    {
        if (obj.GetInstanceID() == gameObject.GetInstanceID() && emitter.eventPassedFilter == "BlockChange")
        {
            GetComponent<Renderer>().material.color = Color.white;
            Debug.Log("Exited: " + emitter.name);
        }
    }
}
