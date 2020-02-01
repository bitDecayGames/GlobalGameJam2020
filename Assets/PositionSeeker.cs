using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PositionSeeker : MonoBehaviour
{
    public const float THRESHOLD = 0.01f;

    public float Speed;
    public Vector2 dest;

    [HideInInspector]
    public Vector2 lastMove = new Vector2();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(V3toV2(transform.position), dest) > 0)
        {
            lastMove = (dest - V3toV2(transform.position)).normalized * Speed * Time.deltaTime;
            transform.position += V2toV3(lastMove);
        }

        if (Vector2.Distance(V3toV2(transform.position), dest) < THRESHOLD)
        {
            transform.position = V2toV3(dest);
        }
    }
    
    Vector2 V3toV2(Vector3 vIn)
    {
        return new Vector2(vIn.x, vIn.y);
    }

    Vector3 V2toV3(Vector2 vIn)
    {
        return new Vector3(vIn.x, vIn.y, 0);
    }
}
