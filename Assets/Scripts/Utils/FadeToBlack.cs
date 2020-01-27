using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils {
    public class FadeToBlack : MonoBehaviour
    {
        public string DestinationScene;
        public float TimeToFade;
        public bool FadeoutNow;
        
        private float time;
        private bool started;

        private SpriteRenderer spriteRenderer;
        
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            SetAlpha(0);
            transform.position = new Vector3(100000, 100000, 0);
            transform.localScale = new Vector3(1, 1, 1);
            spriteRenderer.sortingOrder = 8000;
            var camPos = Camera.main.transform.position;
            camPos.z = 0;
            transform.position = camPos;
            transform.localScale = new Vector3(1, 1, 0) * 10000000f + new Vector3(0, 0, 1);
        }
        
        void Update() {
            if (FadeoutNow)
            {
                started = true;
            }
            
            if (started) {
                if (time < TimeToFade) {
                    time += Time.deltaTime;
                    SetAlpha(Mathf.Clamp(time / TimeToFade, 0f, 1f));
                } else {
                    started = false;
                    SceneManager.LoadScene(DestinationScene);
                }
            }
        }

        public bool IsFadingOut()
        {
            return started;
        }

        public void FadeOutToScene(float timeToFade, string scene) {
            if (started)
            {
                return;
            }
            
            TimeToFade = timeToFade;
            time = 0;
            DestinationScene = scene;
            started = true;
        }

        private void SetAlpha(float a) {
            var c = spriteRenderer.color;
            c.a = a;
            spriteRenderer.color = c;
        }
    }
}