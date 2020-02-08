
using UnityEngine;

public static class SelectionManager
{
    public static GameObject selectionMgr;
    public static FollowObject indicator;

    public static GameObject currentSelected;
    


    public static void SetSelected(GameObject o)
    {
        if (selectionMgr == null)
        {
            selectionMgr = GameObject.Find("SelectionMgr");
        }

        if (indicator == null)
        {
            indicator = selectionMgr.GetComponent<FollowObject>();
        }

        foreach (SpriteRenderer spriteRenderer in indicator.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.enabled = true;
        }
        
        if (indicator.chase != null)
        {
            // TODO: Do we need to tell the selecting thing we aren't selected any longer?
        }

        indicator.chase = o;
        currentSelected = o;
        // FMOD
        FMODSoundEffectsPlayer.Instance.PlaySoundEffect(SFX.Click);
    }

    public static void ClearSelection()
    {
        foreach (SpriteRenderer spriteRenderer in indicator.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.enabled = true;
        }
        currentSelected = null;

    }
}