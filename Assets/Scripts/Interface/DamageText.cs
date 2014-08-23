using UnityEngine;
using System.Collections;

public class DamageText : MonoBehaviour
{
    private int time = 0;

    private float x, y;
    private float xa, ya, za;
    private float xx, yy, zz;


    void Start()
    {
        Gaussian g = new Gaussian();
        this.x = transform.position.x;
        this.y = transform.position.y;
        xx = x;
        yy = y;
        zz = 2;
        xa = (float)(g.NextGaussian() * 0.06);
        ya = (float)(g.NextGaussian() * 0.04);
        za = (float)(Random.Range(0f, 1f) * 0.27);
    }

    void FixedUpdate()
    {
        time++;
        if (time > 60)
        {
            Destroy(this.gameObject);
        }
        xx += xa;
        yy += ya;
        zz += za;
        if (zz < 0)
        {
            zz = 0;
            za *= -0.5f;
            xa *= 0.6f;
            ya *= 0.6f;
        }
        za -= 0.015f;
        x = xx;
        y = yy;

        transform.position = new Vector3(x,y+zz,3);
    }
}