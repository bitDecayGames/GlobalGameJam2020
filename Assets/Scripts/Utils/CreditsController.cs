using System;
using UnityEngine;
using Utils;

public class CreditsController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) ||
            Input.GetKeyDown(KeyCode.Mouse1) ||
            Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetKeyDown(KeyCode.Space))
        {
            var delayedSceneTransition = gameObject.AddComponent<DelayedSceneTransition>();
            delayedSceneTransition.DelayInSeconds = 0;
            delayedSceneTransition.Scene = Scenes.TitleScreen;
        }
    }
}