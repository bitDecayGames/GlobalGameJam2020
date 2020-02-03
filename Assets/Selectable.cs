using UnityEngine;

public class Selectable : MonoBehaviour
{
    // TODO: this only hits the "first" collider which can be un-deterministic
    private void OnMouseUpAsButton()
    {
        
        Debug.Log($"Selected {name}");
        SelectionManager.SetSelected(gameObject);
    }
}
