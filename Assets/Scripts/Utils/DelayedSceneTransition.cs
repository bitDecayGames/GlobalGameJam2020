using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class DelayedSceneTransition : MonoBehaviour
{
    private static string FadeToBlackTransitionObject = "TransitionGameObjects/FadeOut";
    private bool createdFadeoutTransition;
    
    public string Scene;
    public float DelayInSeconds;

    private void Update()
    {
        DelayInSeconds -= Time.deltaTime;
        if (DelayInSeconds <= 0 && !createdFadeoutTransition)
        {
            GameObject fadeOutTransitionTemplate = Resources.Load<GameObject>(FadeToBlackTransitionObject);
            GameObject fadeOutTransitionInstance = Instantiate(fadeOutTransitionTemplate);
            fadeOutTransitionInstance.GetComponent<FadeToBlack>().FadeOutToScene(1f, Scene);
            createdFadeoutTransition = true;
        }
    }
}