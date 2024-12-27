using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWorldPosition : MonoBehaviour
{
    public Transform FollowThis;
    public float yOffset = 50;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 sp = Camera.main.WorldToScreenPoint(FollowThis.position);


        // add y offset
        sp.y += yOffset;

        transform.position = sp;
    }
}
