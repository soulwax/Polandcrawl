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
        up = Input.GetKey(KeyCode.Keypad8);
        down = Input.GetKey(KeyCode.Keypad2);
        left = Input.GetKey(KeyCode.Keypad4);
        right = Input.GetKey(KeyCode.Keypad6);
        downleft = Input.GetKey(KeyCode.Keypad1);
        upleft = Input.GetKey(KeyCode.Keypad7); 
        upright = Input.GetKey(KeyCode.Keypad9);
        downright = Input.GetKey(KeyCode.Keypad3);
        wait = Input.GetKey(".");
    }

    public void MouseUpdate()
    {
        mpos = Input.mousePosition;
        this.x = mpos.x;
        this.y = mpos.y;
        lmb = Input.GetMouseButton(0);
        rmb = Input.GetMouseButton(1);
    }

    public void ReleaseAll()
    {
        up = down = right = left = upright = upleft = downright = downleft = lmb = rmb = false;
    }
}
