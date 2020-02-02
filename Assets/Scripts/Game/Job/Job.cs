using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class Job : MonoBehaviour {
    private const float BUBBLE_WIDTH = 0.5f;
    private const float VERTICAL_OFFSET = 0.75f;

    public List<InventoryType> Required;
    public float Price;
    private float SecondsRemainingUntilFailure;
    private float SecondsRemainingToComplete;
    private bool IsBeingWorkedOn;
    private bool IsComplete;
    private bool IsFailed;
    private InventoryItemToSprites itemMap;
    private Action OnJobComplete;
    private Action<bool> OnJobEnd; // true if the job is successfully completed
    private int SoundEffectId;

    public void Init(InventoryItemToSprites itemMap, List<InventoryType> Required, float price, float SecondsUntilFailure, float SecondsToComplete, Action<bool> OnJobEnd) {
        this.itemMap = itemMap;
        this.Required = Required;
        Price = price;
        SecondsRemainingUntilFailure = SecondsUntilFailure;
        SecondsRemainingToComplete = SecondsToComplete;
        this.OnJobEnd = OnJobEnd;
        CreateRequirementPanel();
        CreateMoneyText();
        CreateRequirementBubbles();
    }

    private void CreateRequirementPanel() {
        var panelObj = new GameObject();
        panelObj.name = "Panel";
        panelObj.transform.parent = transform;
        var pos = new Vector3(0, 0, 0);
        pos.y += VERTICAL_OFFSET;
        pos.x += 0.5f;
        panelObj.transform.localPosition = pos;
        var panelSpr = panelObj.AddComponent<SpriteRenderer>();
        var itemKey = itemMap.Get(InventoryType.JOB_PANEL);
        if (itemKey == null) throw new Exception("Can't find job panel key");
        panelSpr.sprite = itemMap.Get(InventoryType.JOB_PANEL).image;
        panelSpr.drawMode = SpriteDrawMode.Sliced;
        panelSpr.sortingLayerName = "UI";
        panelSpr.sortingOrder = -1;
        panelSpr.size = new Vector2(BUBBLE_WIDTH * 1.5f, Required.Count * BUBBLE_WIDTH + BUBBLE_WIDTH * 0.5f);
    }

    private void CreateMoneyText() {
        var textObj = new GameObject();
        textObj.transform.SetParent(transform);
        var text = textObj.AddComponent<TextMeshPro>();
        var textLocPos = text.transform.localPosition;

        text.text = MoneyConverter.FloatToMoney(Price);
        text.alignment = TextAlignmentOptions.Left;
        text.fontSize = 4;
        text.color = new Color(71 / 255.0f, 170 / 255.0f, 44 / 255.0f);
        text.sortingLayerID = SortingLayer.NameToID("UI");
        text.rectTransform.sizeDelta = new Vector2(5, 1);

        textLocPos.y = VERTICAL_OFFSET;
        textLocPos.z = 0;
        textLocPos.x = 3f;
        text.transform.localPosition = textLocPos;
    }

    private void CreateRequirementBubbles() {
        var length = Required.Count;
        var totalWidth = length * BUBBLE_WIDTH;
        for (int i = 0; i < length; i++) {
            var xOffset = (i * BUBBLE_WIDTH);
            var yOffset = VERTICAL_OFFSET + 0.4f;
            var bubbleObj = new GameObject();
            bubbleObj.name = "JobRequirement: " + Required[i];
            bubbleObj.transform.parent = transform;
            bubbleObj.transform.localPosition = Vector3.zero;
            var bubbleObjPos = bubbleObj.transform.position;
            bubbleObjPos.x += 0.5f;
            bubbleObjPos.y += yOffset + xOffset;
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
        if (!IsBeingWorkedOn) {
            // FMOD: Job reached
            FMODSoundEffectsPlayer.Instance.PlaySoundEffect(SFX.JobReached);
            Random random = new Random();
            switch (random.Next(0, 3)) {
                case 0:
                    SoundEffectId = FMODSoundEffectsPlayer.Instance.PlaySustainedSoundEffect(SFX.WorkMetalLoop);
                    break;
                case 1:
                    SoundEffectId = FMODSoundEffectsPlayer.Instance.PlaySustainedSoundEffect(SFX.WorkSquishLoop);
                    break;
                case 2:
                    SoundEffectId = FMODSoundEffectsPlayer.Instance.PlaySustainedSoundEffect(SFX.WorkWoodLoop);
                    break;
            }

            FMODSoundEffectsPlayer.Instance.PlaySoundEffect(SFX.JobReached);
            Debug.Log("JOB BEING WORKED");
            IsBeingWorkedOn = true;
            this.OnJobComplete = OnJobComplete;
        }
    }

    private void Update() {
        if (IsBeingWorkedOn) {
            SecondsRemainingToComplete -= Time.deltaTime;
            if (SecondsRemainingToComplete <= 0) {
                IsBeingWorkedOn = false;
                IsComplete = true;
            }
        } else if (IsComplete) {
            FMODSoundEffectsPlayer.Instance.StopSustainedSoundEffect(SoundEffectId);
            if (OnJobComplete != null) {
                OnJobComplete();
            }

            if (OnJobEnd != null) OnJobEnd(true);
            Destroy(this);
            for (int i = 0; i < transform.childCount; i++) {
                Destroy(transform.GetChild(i).gameObject);
            }
        } else if (IsFailed) {
            // FMOD: play some failed job sound (maybe based on the Required types?)
            if (OnJobEnd != null) OnJobEnd(false);
            Destroy(this);
            for (int i = 0; i < transform.childCount; i++) {
                Destroy(transform.GetChild(i).gameObject);
            }
        } else {
            SecondsRemainingUntilFailure -= Time.deltaTime;
            if (SecondsRemainingUntilFailure <= 0) IsFailed = true;
        }
    }
}