using UnityEngine;

public class CustomInputManager : MonoBehaviour
{
    public GameObject PauseOverlayPrefab;
    public GameObject PauseOverlayInstance;
    void Update() {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (PauseOverlayInstance == null)
            {
                PauseOverlayInstance = Instantiate(PauseOverlayPrefab);
                FMODMusicPlayer.Instance.SetParameter(ParametersListEnum.Parameters.Track2, 1f);
            }
            else
            {
                Destroy(PauseOverlayInstance);
                FMODMusicPlayer.Instance.SetParameter(ParametersListEnum.Parameters.Track2, 0);
            }
        }
    }
}