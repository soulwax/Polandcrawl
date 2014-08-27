using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class is very experimental, I am not sure if there are any benefits to this.
/// Basic idea was to improve control over input, but it might just be meaningless bloat.
/// </summary>

public class InputHandler {

    //keys
    public bool up = false;
    public bool down = false;
    public bool left = false;
    public bool right = false;
    public bool upleft = false;
    public bool upright = false;
    public bool downleft = false;
    public bool downright = false;

    public bool wait = false;

    //mouse related
    public Vector3 mpos; //mouse position
    public float xo, yo; //TODO: x,y origin
    public float x, y; //x,y current
    public bool lmb = false;
    public bool rmb = false;

    public InputHandler()
    {        
    }

    public void KeyUpdate()
    {
        if (Input.GetKey(KeyCode.Keypad8)) up = true;
        else up = false;

        if (Input.GetKey(KeyCode.Keypad2)) down = true;
        else down = false;

        if (Input.GetKey(KeyCode.Keypad4)) left = true;
        else left = false;

        if (Input.GetKey(KeyCode.Keypad6)) right = true;
        else right = false;

        if (Input.GetKey(KeyCode.Keypad1)) downleft = true;
        else downleft = false;

        if (Input.GetKey(KeyCode.Keypad7)) upleft = true;
        else upleft = false;

        if (Input.GetKey(KeyCode.Keypad9)) upright = true;
        else upright = false;

        if (Input.GetKey(KeyCode.Keypad3)) downright = true;
        else downright = false;

        if (Input.GetKey(".")) wait = true;
        else wait = false;      
    }

    public void MouseUpdate()
    {
        mpos = Input.mousePosition;
        this.x = mpos.x;
        this.y = mpos.y;

        if (Input.GetMouseButton(0)) lmb = true;
        else lmb = false;

        if (Input.GetMouseButton(1)) rmb = true;
        else rmb = false;
    }

    public void ReleaseAll()
    {
        up = down = right = left = upright = upleft = downright = downleft = lmb = rmb = false;
    }
}
