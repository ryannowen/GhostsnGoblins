using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxEffect : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallexEffect;

    // Start is called before the first frame update
    void Start()
    {
        //Vector3 pos = cam.transform.position;
        //pos.z = transform.position.z;
        //transform.position = pos;
        startpos = cam.transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = (cam.transform.position.x * parallexEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        float temp = (cam.transform.position.x * (1 - parallexEffect));

        if (temp > startpos + length)
            startpos += length;
        else if (temp < startpos - length)
            startpos -= length;
    }
}
