using System.Collections;
using UnityEngine;

public class Gaussian {



	//Writing own class because neither c# nor unity implemented this shit
    //I refrain from using the unity Mathf and Random class here for precision
    private static double nextNextGaussian;
    private static bool haveNextNextGaussian = false;

    private static long seed;

    //produces normally distributed 'Gaussian' value between 0.0 and standard deviation 1.0
    //70% of the results will be between -1.0 and 1.0
    public static double NextGaussian()
    {
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
                v1 = 2 * NextDouble() - 1;   // between -1.0 and 1.0
                v2 = 2 * NextDouble() - 1;   // between -1.0 and 1.0
                s = v1 * v1 + v2 * v2;           
            } while (s >= 1 || s == 0);
            double multiplier = System.Math.Sqrt(-2 * System.Math.Log(s) / s);
            nextNextGaussian = v2 * multiplier;
            haveNextNextGaussian = true;
            return v1 * multiplier;
        }
    }

    //convenience methods for pseudo random numbers
    public static void SetSeed(long seed)
    {
        seed = (seed ^ 0x5DEECE66DL) & ((1L << 48) - 1);
    }

    //This is a linear congruential pseudorandom number generator, 
    //as defined by D. H. Lehmer and described by Donald E. Knuth in 
    //The Art of Computer Programming, Volume 3: Seminumerical Algorithms, section 3.2.1.
    protected static int Next(int bits)
    {
        seed = (seed * 0x5DEECE66DL + 0xBL) & ((1L << 48) - 1);
        return (int)((ulong)seed >> (48 - bits));
    }

    public static float NextFloat()
    {
        return Next(24) / ((float)(1 << 24));
    }

    public static double NextDouble()
    {
        return (((long)Next(26) << 27) + Next(27)) / (double)(1L << 53);
    }

    public static int NextInt(int n)
    {
        if (n <= 0)
            throw new System.ArgumentException ("n must be positive");

        if ((n & -n) == n)  // i.e., n is a power of 2
            return (int)((n * (long)Next(31)) >> 31);

        int bits, val;
        do
        {
            bits = Next(31);
            val = bits % n;
        } while (bits - val + (n - 1) < 0);
        return val;
    }
}
