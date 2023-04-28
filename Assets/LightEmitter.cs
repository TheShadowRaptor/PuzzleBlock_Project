using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEmitter : MonoBehaviour
{
    public Light actualLight;

    public static List<LightEmitter> all = new List<LightEmitter>();

    private void OnEnable()
    {
        all.Add(this);
    }

    private void OnDisable()
    {
        all.Remove(this);
    }

    public static bool IsInLightRange(Vector3 position)
    {
        for (int i = 0; i < all.Count; i++)
        {
            //Debug.Log($"{all[i].actualLight.range * all[i].actualLight.range}");
            if ((position - all[i].transform.position).sqrMagnitude < all[i].actualLight.range * all[i].actualLight.range)
            {
                return true;
            }
        }
        return false;
    }
}
