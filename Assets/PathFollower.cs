using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PathFollower : MonoBehaviour
{
    public List<Tile> path;
    private int index;

    private PositionSeeker seeker;
    
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<PositionSeeker>();
        setDest();
    }

    // Update is called once per frame
    void Update()
    {
        if (path.Count == 0)
        {
            return;
        }
        
        if (seeker.arrived)
        {
            index++;
        }
        
        if (index >= path.Count)
        {
            return;
        }
        else
        {
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
        if (path.Count == 0)
        {
            return;
        }
        seeker.dest = VectorMath.V3toV2(path[index].transform.position);
    }
}
