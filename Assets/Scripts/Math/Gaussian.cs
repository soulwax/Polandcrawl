using System.Collections;
using UnityEngine;

public class Gaussian {

	//Writing own class because neither c# nor unity implemented this shit
    //I refrain from using the unity mathf and Random class here, because they are limited to floats
    private double nextNextGaussian;
    private bool haveNextNextGaussian = false;


    //produces normally distributed 'Gaussian' value between 0.0 and standard deviation 1.0
    //70% of the results will be between -1.0 and 1.0
    public double NextGaussian()
    {
        System.Random rnd = new System.Random();
        int attempts = 0;
        if (haveNextNextGaussian)
        {
            haveNextNextGaussian = false;
            return nextNextGaussian;
        }
            
        else
        {
            double v1, v2, s;
            do
            {
                v1 = 2 * rnd.NextDouble() - 1;   // between -1.0 and 1.0
                v2 = 2 * rnd.NextDouble() - 1;   // between -1.0 and 1.0
                s = v1 * v1 + v2 * v2;           
            } while ((s >= 1 || s == 0) && (attempts < 10));
            double multiplier = System.Math.Sqrt(-2 * System.Math.Log(s) / s);
            nextNextGaussian = v2 * multiplier;
            haveNextNextGaussian = true;
            return v1 * multiplier;
        }
    }
}
