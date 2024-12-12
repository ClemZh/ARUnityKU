using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class BlockPlacements : MonoBehaviour
{
    public GameObject block; // This is the block that will be placed
    public ARSessionOrigin sessionOrigin; // This is the ARSessionOrigin object
    public ARRaycastManager raycastManager; // This is the ARRaycastManager object
    public ARPlaneManager planeManager; // This is the ARPlaneManager object
    private List<ARRaycastHit> hits = new List<ARRaycastHit>(); // This is a list of ARRaycastHit objects


    // Start is called before the first frame update
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Input.GetTouch(0).position;

            if (raycastManager.Raycast(touchPosition, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;
                Instantiate(block, hitPose.position, hitPose.rotation);
            }
        }
    }
}
