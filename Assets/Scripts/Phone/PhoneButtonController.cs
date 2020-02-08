
using System;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.EventSystems;

public class PhoneButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public JobManager jobManager;

    private bool callTaken;
    private EventInstance pickupSoundEffect;
    private EventInstance dialToneSoundEffect;
    private EventInstance chatterSoundEffect;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        pickupSoundEffect = FMODSoundEffectsPlayer.Instance.PlaySoundEffect(SFX.PhoneAnswer);
        callTaken = jobManager.OnPhoneCallTaken();
        if (callTaken)
        {
            chatterSoundEffect = FMODSoundEffectsPlayer.Instance.PlaySoundEffect(SFX.Chatter);
        }
        else
        {
            dialToneSoundEffect = FMODSoundEffectsPlayer.Instance.PlaySoundEffect(SFX.DialTone);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        FMODSoundEffectsPlayer.Instance.PlaySoundEffect(SFX.PhoneHangup);
        pickupSoundEffect.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        chatterSoundEffect.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        dialToneSoundEffect.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    private void OnDestroy()
    {
        chatterSoundEffect.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        dialToneSoundEffect.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}