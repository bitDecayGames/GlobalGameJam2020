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
