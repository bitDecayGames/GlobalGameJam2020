using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JobManager : MonoBehaviour {
    public InventoryItemToSprites ItemMap;
    public UnityEvent OnPhoneRing;

    private List<GameObject> UnavailableLocations = new List<GameObject>();
    public List<GameObject> PossibleLocations = new List<GameObject>(); // PLEASE DON'T EDIT THIS DIRECTLY, IT IS PUBLIC FOR DEBUG PURPOSES
    public bool CallIsWaiting;
    private float CurrentPhoneCallTimer = 3f;
    private float NextPhoneCallTimer = 20.0f; // this is the starting number of seconds between phone call rings
    private float MinNextPhoneCallTime = 5.0f; // you must wait at least this long for another job
    private float SecondsUntilJobFailure = 30.0f; // how long you have without working on this job before it fails
    private float SecondsToCompleteJob = 3.0f; // how long the car has to sit there for the job to be done

    /// <summary>
    /// Add a possible location for this job manager to use as a client location.
    /// Probably needs to be called when the Tiled Map is first available and we parse through the
    /// tiles to find houses. 
    /// </summary>
    /// <param name="possibleLocation">the transform of the object that you want the job bubble to hover over</param>
    public void AddPossibleLocation(GameObject possibleLocation) {
        PossibleLocations.Add(possibleLocation);
    }

    /// <summary>
    /// This only gets called when the phone is "picked up"
    /// </summary>
    private void CreateJob() {
        Debug.Log("Create job");
        // Pick a random location that doesn't already have a job
        var location = PossibleLocations[Random.Range(0, PossibleLocations.Count)];
        UnavailableLocations.Add(location); // only one job per location at a time
        // TODO: Create the requirements for a job that are based on the hardness level the player is currently at
        var requirements = new List<InventoryType> {InventoryType.WRENCH};
        // TODO: Create price from requirements
        var price = 100;

        // // create the job as a child object of the location the job is at
        // var jobObj = new GameObject();
        // jobObj.name = "Job";
        // jobObj.transform.parent = location;
        // jobObj.transform.localPosition = Vector3.zero;
        var job = location.gameObject.AddComponent<Job>();
        // jobs take 1 second longer for each requirement.  Seems fair?
        job.Init(ItemMap, requirements, price, SecondsUntilJobFailure, SecondsToCompleteJob + requirements.Count, isSuccess => {
            Debug.Log($"Job is done: " + (isSuccess ? "Success!" : "FAILED!"));
            // FMOD: Fail and success
            if (isSuccess)
            {
                FMODSoundEffectsPlayer.Instance.PlaySoundEffect(SFX.CompleteTask);
                FindObjectOfType<Wallet>().AddMoney(price);
            }
            else
            {
                FMODSoundEffectsPlayer.Instance.PlaySoundEffect(SFX.FailTask);
            }
            // make this location available again to receive a job
            UnavailableLocations.Remove(location);
            AddPossibleLocation(location);
        });
    }

    /// <summary>
    /// This method should get called when the phone UI element is clicked
    /// </summary>
    public void OnPhoneCallTaken() {
        if (CallIsWaiting) {
            Debug.Log("Phone call answered");
            CallIsWaiting = false;
            // FMOD
            FMODSoundEffectsPlayer.Instance.PlaySoundEffect(SFX.PhoneAnswer);
            NextPhoneCallTimer *= 0.9f; // TODO: this number could be affected by upgrades to get the next job faster?
            if (NextPhoneCallTimer < MinNextPhoneCallTime) NextPhoneCallTimer = MinNextPhoneCallTime;
            CreateJob();
        }
    }

    private void Update() {
        if (!CallIsWaiting) {
            CurrentPhoneCallTimer -= Time.deltaTime;
            if (CurrentPhoneCallTimer <= 0 && PossibleLocations.Count > 0) { // only ring the phone if there is at least one available location
                CallIsWaiting = true;
                CurrentPhoneCallTimer = NextPhoneCallTimer;
                if (OnPhoneRing != null) {
                    // TODO: FX: play phone call noise here (maybe get softer as time goes on so we don't annoy ppl? You can use the NextPhoneCallTimer as a volume control since it starts at 20 and goes to 5 over time.)
                    // FMOD
                    FMODSoundEffectsPlayer.Instance.PlaySoundEffect(SFX.Phone);
                    OnPhoneRing.Invoke(); // this notifies the phone UI element to do it's ring sequence
                }
            }
        }
    }
}