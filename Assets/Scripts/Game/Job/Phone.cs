using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Phone : MonoBehaviour {
    public UnityEvent OnCallAnswered; // hook the job manager into this event
    
    public void OnCallComingIn() {
        // TODO: you might want to animate this phone sprite some how, like maybe make it blink white?
        // TODO: take this thing out, just for debugging
        GetComponentInChildren<Text>().text = "(Incomming Call...)";
    }

    public void OnPhoneButtonClicked() {
        // TODO: take this thing out, just for debugging
        GetComponentInChildren<Text>().text = "(No Calls)";
        if (OnCallAnswered != null) {
            OnCallAnswered.Invoke();
        }
    }
}