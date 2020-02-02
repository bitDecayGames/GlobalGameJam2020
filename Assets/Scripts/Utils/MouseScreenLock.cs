using UnityEngine;
    public class MouseScreenLock : MonoBehaviour
    {
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }