using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphStatsDice : MonoBehaviour
{

    public Transform[] bars;
    public float height = 2.0f;
    int throws = 0;
    int[] values = new int[6];
    int currentMax = 0;
    Vector3 floor = new Vector3(1, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform b in bars)
        {
            b.localScale =  new Vector3(b.localScale.x, 0.1f, b.localScale.z);
        }
    }

    public void InsertValue(int i)
    {
        throws++;
        values[i - 1]++;
        //Debug.Log("v("+i+"): " + values[i - 1] + ", cm: " + currentMax);
        if (values[i-1] > currentMax)
        {
            currentMax = values[i - 1];

            //scale and normalize all bars
            for(int idx = 0; idx < bars.Length; idx++)
            {
                bars[idx].localScale = floor + Vector3.up * height * values[idx] / currentMax;
                //bars[idx].localScale = Vector3.up * (float)values[idx];
            }

        } else
        {
            //scale and normalize only the affected bar
            bars[i - 1].localScale = floor + Vector3.up * height * values[i - 1] / currentMax;
        }
    }
}
