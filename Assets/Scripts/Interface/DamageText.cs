using UnityEngine;
using System.Collections;

public class DamageText : MonoBehaviour
{
    private int time = 0;

    //I'm used to transfer temporary coordinates into actual coordinates from my
    // Java times, not sure if it's necessary here, probably not but it doesn't cause harm either
    private float x, y; //<-- these are actual coordinates
    private float xa, ya, za; // <-- this is the directional vector
    private float xx, yy, zz; // <-- temporary coordinates

    //Note: the z axis is merely simulated here for a bouncing effect, the actual z coordinate doesn't get changed at all

    void Start()
    {
        Gaussian g = new Gaussian(); //normal distribution for more fancy randomness
        this.x = transform.position.x; 
        this.y = transform.position.y;
        this.xx = x; 
        this.yy = y; 
        this.zz = 2; //at first the digit flies upwards
        this.xa = (float)(g.NextGaussian() * 0.06); //bounce off towards the sides
        this.ya = (float)(g.NextGaussian() * 0.04); 
        this.za = (float)(Random.Range(0f, 1f) * 0.27); //upward direction
    }

    //using FixedUpdate because this is technically Physics
    void FixedUpdate()
    {
        time++;
        if (time > 60)
        {
            Destroy(this.gameObject);
        }
        xx += xa; //apply directional vector
        yy += ya;
        zz += za;
        if (zz < 0) 
        {
            zz = 0; //stop falling as the digit reached an imaginary floor
            za *= -0.5f; //bounce off, reduce impulse
            xa *= 0.6f; //apply some friction as well
            ya *= 0.6f; 
        }
        za -= 0.015f; //gravitational pull
        x = xx; //apply temporary coordinates to the actual ones
        y = yy;

        transform.position = new Vector3(x,y+zz,3); //use actual coords to display movement, simulated z axis gets added to the y axis
    }
}