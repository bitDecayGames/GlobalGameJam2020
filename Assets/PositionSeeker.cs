using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utils;

public class PositionSeeker : MonoBehaviour
{
    public const float THRESHOLD = 0.01f;

    public float Speed;
    public Vector2 dest;

    public bool arrived = false;

    [HideInInspector]
    public Vector2 lastMove = new Vector2();
    
    // Start is called before the first frame update
    void Start()
    {
        // Start this where we are
        dest = VectorMath.V3toV2(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(VectorMath.V3toV2(transform.position), dest) > 0)
        {
            arrived = false;
            lastMove = (dest - VectorMath.V3toV2(transform.position)).normalized * Speed * Time.deltaTime;
            transform.position += VectorMath.V2toV3(lastMove);
        }

        if (Vector2.Distance(VectorMath.V3toV2(transform.position), dest) < THRESHOLD)
        {
            arrived = true;
            transform.position = VectorMath.V2toV3(dest);
        }
    }
}
