using System;
using System.Collections.Generic;
using UnityEngine;

public class JobLocator : MonoBehaviour
{
    public GameObject JobMarker;
    public GameObject jobMarker1;
    public GameObject jobMarker2;
    public GameObject jobMarker3;
    public List<GameObject> jobMarkers;
    
    Camera camera;
 
    void Start(){
        camera = Camera.main;
        jobMarkers = new List<GameObject>();
        
        jobMarker1 = Instantiate(JobMarker, transform);
        jobMarker1.GetComponent<SpriteRenderer>().enabled = false;
        jobMarkers.Add(jobMarker1);
        jobMarker2 = Instantiate(JobMarker, transform);
        jobMarker2.GetComponent<SpriteRenderer>().enabled = false;
        jobMarkers.Add(jobMarker2);
        jobMarker3 = Instantiate(JobMarker, transform);
        jobMarker3.GetComponent<SpriteRenderer>().enabled = false;
        jobMarkers.Add(jobMarker3);

    }
 
    void Update ()
    {
        Job[] activeJobs;
        activeJobs = FindObjectsOfType<Job>();
        for (int i = 0; i < 3; i++)
        {
            SpriteRenderer spriteRenderer;
            spriteRenderer = jobMarkers[i].GetComponent<SpriteRenderer>();
            if (i < activeJobs.Length)
            {
                Vector3 screenPos = camera.WorldToViewportPoint(activeJobs[i].transform.position); //get viewport positions
                if(screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1)
                {
                    spriteRenderer.enabled = false;
                    return;
                } 
                spriteRenderer.enabled = true;
                Vector2 onScreenPos = new Vector2(screenPos.x-0.5f, screenPos.y-0.5f)*2; //2D version, new mapping
                float max = Mathf.Max(Mathf.Abs(onScreenPos.x), Mathf.Abs(onScreenPos.y)); //get largest offset
                onScreenPos = (onScreenPos/(max*2))+new Vector2(0.5f, 0.5f); //undo mapping
                Vector3 screenPosVector3 = new Vector3(onScreenPos.x*Screen.width, onScreenPos.y*Screen.height, camera.nearClipPlane);
                Vector3 finalSpritePosition = camera.ScreenToWorldPoint(screenPosVector3);
                spriteRenderer.transform.position = finalSpritePosition;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }
    }
}