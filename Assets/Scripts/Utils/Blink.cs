using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour {
    public Color Tint;
    public float SecondsToBlink;
    public float BlinkRate;

    private List<SpriteTracker> sprites;
    private float blinkTimeTracker;
    private bool blink;
    private bool isInit;

    public void Init(Color Tint, float SecondsToBlink, float BlinkRate) {
        isInit = true;
        this.Tint = Tint;
        this.SecondsToBlink = SecondsToBlink;
        this.BlinkRate = BlinkRate;
        blinkTimeTracker = BlinkRate;
        var spriteArr = GetComponentsInChildren<SpriteRenderer>();
        sprites = new List<SpriteTracker>();
        foreach (var spr in spriteArr) {
            sprites.Add(new SpriteTracker(spr));
        }
        sprites.ForEach(s => s.SetColor(Tint));
        blink = true;
    }

    private void Update() {
        if (isInit) {
            if (SecondsToBlink < 0) {
                sprites.ForEach(s => s.SetColor(s.originalColor));
                Destroy(this);
            } else {
                SecondsToBlink -= Time.deltaTime;
                blinkTimeTracker -= Time.deltaTime;
                if (blinkTimeTracker < 0) {
                    blinkTimeTracker = BlinkRate;
                    blink = !blink;
                    if (blink) sprites.ForEach(s => s.SetColor(Tint));
                    else sprites.ForEach(s => s.SetColor(s.originalColor));
                }
            }
        }
    }

    private class SpriteTracker {
        public SpriteRenderer sprite;
        public Color originalColor;

        public SpriteTracker(SpriteRenderer sprite) {
            this.sprite = sprite;
            originalColor = sprite.color;
        }

        public SpriteTracker SetColor(Color color) {
            sprite.color = color;
            return this;
        }
    }
}