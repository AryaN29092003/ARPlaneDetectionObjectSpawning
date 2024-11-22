using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SpawnableManager : MonoBehaviour
{
    ARRaycastManager m_RaycastManager;
    List<ARRaycastHit> m_Hit = new List<ARRaycastHit>();
    [SerializeField]
    GameObject spawnablePrefab;

    GameObject spawnedObject;
    // Start is called before the first frame update
    void Start()
    {
        spawnedObject = null;
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (m_RaycastManager.Raycast(touch.position, m_Hit, TrackableType.PlaneWithinPolygon))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    if (spawnedObject == null)
                        spawnedObject = Instantiate(spawnablePrefab, m_Hit[0].pose.position, Quaternion.identity);
                    else
                        spawnedObject.transform.position = m_Hit[0].pose.position;
                }
                // else if (Input.GetTouch(0).phase == TouchPhase.Moved && spawnedObject != null)
                // {
                //     spawnedObject.transform.position = m_Hit[0].pose.position;
                // }
                // if (Input.GetTouch(0).phase == TouchPhase.Ended)
                // {
                //     //spawnedObject = null;
                // }
            }
        }
    }
}