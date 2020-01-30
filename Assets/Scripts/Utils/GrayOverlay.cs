using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils {
    public class GrayOverlay : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            transform.position = new Vector3(100000, 100000, 0);
            transform.localScale = new Vector3(1, 1, 1);
            var camPos = Camera.main.transform.position;
            camPos.z = 0;
            transform.position = camPos;
            transform.localScale = new Vector3(1, 1, 0) * 10000000f + new Vector3(0, 0, 1);
        }
    }
}