using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

public class PathFollower : MonoBehaviour
{
    public List<Tile> path;
    public int index;

    public GameObject destination;
    [FormerlySerializedAs("done")] public bool jobCheckDone = true;

    private PositionSeeker seeker;

    private void Awake()
    {
        seeker = GetComponent<PositionSeeker>();
        if (seeker == null)
        {
            throw new Exception("No seeker found on the object for PathFollower to interact with");
        }
        setDest();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (path.Count == 0)
        {
            return;
        }
        
        if (index >= path.Count)
        {
            if (destination == null)
            {
                return;
            }
            // TODO: Call our destination and mark as done
            
            if (!jobCheckDone)
            {
                tryStartJob();
            }

            var occ = destination.GetComponentInChildren<Occupiable>();
            if (occ != null)
            {
                // TODO: If we have multiple trucks at one location, this is gunna be buggy af
                occ.occupier = gameObject;
            }

            
            // TODO: Check for interactable here
            return;
        }
        else if (seeker.arrived)
        {
            index++;
            setDest();
        }
    }

    void tryStartJob()
    {
        jobCheckDone = true;
        var job = destination.GetComponent<Job>();
        if (job != null)
        {
            // TODO: compare inventory

            var inv = GetComponentInChildren<Inventory>();

            if (inv == null)
            {
                Debug.Log("no inventory found on the arriving vehicle");
                return;
            }

            var found = false;
            foreach (var reqItem in job.Required)
            {
                found = false;
                foreach (var slot in inv.Slots)
                {
                    if (slot.Item == reqItem)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    // we are missing an item for the job
                    // TODO: SFX for bad job attempt?
                    Debug.Log("Did not have item needed for job: " + reqItem);
                    
                    return;
                }
            }
            
            foreach (var reqItem in job.Required)
            {
                found = false;
                foreach (var slot in inv.Slots)
                {
                    if (slot.Item == reqItem)
                    {
                        if (slot.Item == InventoryType.PAINT ||
                            slot.Item == InventoryType.BATTERY ||
                            slot.Item == InventoryType.LIGHT_BULB)
                        {
                            // Remove the stuff from the player
                            inv.RemoveItemType(slot.Item);
                        }
                    }
                }
            }
            
            Debug.Log("GOTTA START THE JOB");
            job.MarkJobAsBeingWorkedOn(() => { Debug.Log("JOB DONE");});
        }
    }

    public void SetList(List<Tile> path)
    {
        this.path = path;
        index = 0;
        setDest();
    }

    void setDest()
    {
        if (path.Count == 0 || index >= path.Count)
        {
            return;
        }
        // Add .5 to each so it goes to the center
        seeker.dest = VectorMath.V3toV2(path[index].transform.position) + new Vector2(0.5f, 0.5f);
    }

    public void SetDestinationObject(GameObject destObj)
    {
        // FMOD
        FMODSoundEffectsPlayer.Instance.PlaySoundEffect(SFX.ClickGo);
        if (destination != null)
        {
            var occ = destination.GetComponentInChildren<Occupiable>();
            if (occ != null)
            {
                // TODO: If we have multiple trucks at one location, this is gunna be buggy af
                occ.occupier = null;
            }
        }
        
        destination = destObj;
        jobCheckDone = false;
    }
}
