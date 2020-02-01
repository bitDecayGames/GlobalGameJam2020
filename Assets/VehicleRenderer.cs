using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class VehicleRenderer : MonoBehaviour
{
    public Sprite up;

    public Sprite down;

    public Sprite left;

    private SpriteRenderer render;
    private PositionSeeker seeker;
    
    // Start is called before the first frame update
    void Start()
    {
        seeker = transform.GetComponent<PositionSeeker>();
        render = transform.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (seeker != null)
        {
            if (seeker.lastMove.x != 0)
            {
                render.sprite = left;
            }
            else if (seeker.lastMove.y > 0)
            {
                render.sprite = up;
            }
            else if (seeker.lastMove.y < 0)
            {
                render.sprite = down;
            }

            render.flipX = seeker.lastMove.x > 0;
        }
    }
}
