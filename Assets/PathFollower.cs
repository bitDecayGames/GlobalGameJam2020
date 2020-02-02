using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PathFollower : MonoBehaviour
{
    public List<Tile> path;
    public int index;

    public GameObject destination;
    public bool done = true;

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
            if (done)
            {
                return;
            }

            done = true;
            if (destination == null)
            {
                Debug.Log("There was no destination to tell of done-ness");
                return;
            }
            // TODO: Call our destination and mark as done
            var job = destination.GetComponent<Job>();
            if (job != null)
            {
                Debug.Log("GOTTA START THE JOB");
                job.MarkJobAsBeingWorkedOn(() => { Debug.Log("JOB DONE");});
            }
            return;
        }
        else if (seeker.arrived)
        {
            index++;
            setDest();
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
        destination = destObj;
        done = false;
    }
}
