using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    /// <summary>
    /// Gets the Vector3 that's directly in front of the object
    /// </summary>
    /// <param name="origin">What's the origin of the Vector?</param>
    /// <param name="distance">How far away do you want it to be?</param>
    /// <param name="height">What height? (optional)</param>
    /// <returns></returns>
    public static Vector3 GetFrontOfObject(Transform origin, float distance,  float height = -1f)
    {
        Vector3 objectPos = origin.position;
        Vector3 playerDirection = origin.forward;
        Vector3 finalLocation = objectPos + (playerDirection * distance);
        if (height != -1f)
        {
            finalLocation = new Vector3(finalLocation.x, height, finalLocation.z);
        }
        return finalLocation;
    }

    /// <summary>
    /// Takes in a timestamp string and turns it into a DateTime C# object
    /// </summary>
    /// <param name="originalString"></param>
    /// <returns></returns>
    public static DateTime StringToDateTime(string originalString)
    {
        return DateTime.Parse(originalString);
    }

    /// <summary>
    /// Remaps a value between two scales
    /// </summary>
    /// <param name="value"></param>
    /// <param name="from1"></param>
    /// <param name="to1"></param>
    /// <param name="from2"></param>
    /// <param name="to2"></param>
    /// <returns></returns>
    public static float ConvertLinearRange(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    /// <summary>
    /// Finds the direction vector to any given point
    /// </summary>
    /// <param name="targetDestination"></param>
    /// <returns></returns>
    public static Vector3 FindDirectionToPoint(Vector3 origin, Vector3 destination)
    {
        return (origin - destination).normalized;
    }

    public static System.Random rng = new System.Random();

    /// <summary>
    /// Shuffles a list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// Generates a random Normalized Vector3 with values from -1 to 1.
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetRandomNormalizedVector3()
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
    }

    /// <summary>
    /// Checks if layer number is present in a given layermask
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="layermask"></param>
    /// <returns></returns>
    public static bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }

    public static Vector3 GetMidPointBetweenTwoTransforms(Transform transformA, Transform transformB)
    {
        return (transformA.position + transformB.position) * 0.5f;
    }

}