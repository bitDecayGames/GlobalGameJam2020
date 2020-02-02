using System;
using System.Collections.Generic;
using UnityEngine;

public class Job : MonoBehaviour {
    private const float BUBBLE_WIDTH = 0.3f;
    
    private List<InventoryType> Required;
    private float SecondsRemainingUntilFailure;
    private float SecondsRemainingToComplete;
    private bool IsBeingWorkedOn;
    private bool IsComplete;
    private bool IsFailed;
    private InventoryItemToSprites itemMap;
    private Action OnJobComplete;
    private Action<bool> OnJobEnd; // true if the job is successfully completed

    public void Init(InventoryItemToSprites itemMap, List<InventoryType> Required, float SecondsUntilFailure, float SecondsToComplete, Action<bool> OnJobEnd) {
        this.itemMap = itemMap;
        this.Required = Required;
        SecondsRemainingUntilFailure = SecondsUntilFailure;
        SecondsRemainingToComplete = SecondsToComplete;
        this.OnJobEnd = OnJobEnd;
        CreateRequirementBubbles();
    }

    private void CreateRequirementBubbles() {
        var length = Required.Count;
        var totalWidth = length * BUBBLE_WIDTH;
        for (int i = 0; i < length; i++) {
            var x = (totalWidth / -2f) + (i * BUBBLE_WIDTH);
            var bubbleObj = new GameObject();
            bubbleObj.name = "JobRequirement: " + Required[i];
            bubbleObj.transform.parent = transform;
            bubbleObj.transform.localPosition = Vector3.zero;
            var bubbleObjPos = bubbleObj.transform.position;
            bubbleObjPos.x = x;
            bubbleObj.transform.position = bubbleObjPos;
            var bubbleSpr = bubbleObj.AddComponent<SpriteRenderer>();
            bubbleSpr.sortingLayerName = "UI";
            var key = itemMap.Get(Required[i]);
            if (key != null) {
                bubbleSpr.sprite = key.image;
            } else {
                Debug.LogWarning($"Failed to find sprite for inventory type: {Required[i]}");
            }
        }
    }
    
    /// <summary>
    /// Use this for when the car shows up at the customer's house and is now working on the job
    /// </summary>
    /// <param name="OnJobComplete">A call back to let you know that you've completed the job so you can remove the items for that job and re-enable car select-ability</param>
    public void MarkJobAsBeingWorkedOn(Action OnJobComplete) {
        IsBeingWorkedOn = true;
        this.OnJobComplete = OnJobComplete;
    }

    private void Update() {
        if (IsBeingWorkedOn) {
            SecondsRemainingToComplete -= Time.deltaTime;
            if (SecondsRemainingToComplete <= 0) IsComplete = true;
        } else if (IsComplete) {
            // TODO: FX: play some completed job sound (maybe based on the Required types?)
            if (OnJobComplete != null) OnJobComplete();
            if (OnJobEnd != null) OnJobEnd(true);
            Destroy(gameObject);
        } else if (IsFailed) {
            // TODO: FX: play some failed job sound (maybe based on the Required types?)
            if (OnJobEnd != null) OnJobEnd(false);
            Destroy(gameObject);
        } else {
            SecondsRemainingUntilFailure -= Time.deltaTime;
            if (SecondsRemainingUntilFailure <= 0) IsFailed = true;
        }
    }
}