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
    
    public SpriteRenderer overlayRender;

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
                SetSprite(left);
            }
            else if (seeker.lastMove.y > 0)
            {
                SetSprite(up);
            }
            else if (seeker.lastMove.y < 0)
            {
                SetSprite(down);
            }

            FlipSprite(seeker.lastMove.x > 0);
        }
    }

    private void SetSprite(Sprite sprite) {
        render.sprite = sprite;
        if (overlayRender != null) overlayRender.sprite = sprite;
    }

    private void FlipSprite(bool flipX) {
        render.flipX = flipX;
        if (overlayRender != null) overlayRender.flipX = flipX;
    }
}
