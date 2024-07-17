using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

    public GameObject go;
    public float rate = 1f;

    float counter = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter > rate)
        {
            Instantiate<GameObject>(go, new Vector3(
                Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2),
                Random.Range(transform.position.y - transform.localScale.y / 2, transform.position.y + transform.localScale.y / 2),
                Random.Range(transform.position.z - transform.localScale.z / 2, transform.position.z + transform.localScale.z / 2)
                ), Quaternion.identity);
            counter = 0;
        }
    }
}
