using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils {
    public class EasyNavigator : MonoBehaviour {
        
        private static string FadeToBlackTransitionObject = "TransitionGameObjects/FadeOut";
        private bool createdFadeoutTransition;

        public void GoToScene(string sceneName)
        {
            Debug.Log("Button pressed");
            if (!createdFadeoutTransition)
            {
                Debug.Log("Fadeout transition created");
                GameObject fadeOutTransitionTemplate = Resources.Load<GameObject>(FadeToBlackTransitionObject);
                GameObject fadeOutTransitionInstance = Instantiate(fadeOutTransitionTemplate);
                fadeOutTransitionInstance.GetComponent<FadeToBlack>().FadeOutToScene(2f, sceneName);
                FMODSoundEffectsPlayer.Instance.PlaySoundEffect(SFX.Sound);
                createdFadeoutTransition = true;
            }
        }
    }
}