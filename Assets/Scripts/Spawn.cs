using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Spawn : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager m_RaycastManager;

    [SerializeField]
    ARPlaneManager m_PlaneManager;

    [SerializeField]
    GameObject spawnablePrefab;

    public Camera arCam;
    private List<ARRaycastHit> m_Hit = new List<ARRaycastHit>();
    private GameObject spawnedObject;

    // Start is called before the first frame update
    void Start()
    {
        spawnedObject = null;
    }

    void OnEnable()
    {
        m_PlaneManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        m_PlaneManager.planesChanged -= OnPlanesChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
            return;

        // Perform an AR raycast from the touch position
        if (m_RaycastManager.Raycast(Input.GetTouch(0).position, m_Hit, TrackableType.Planes))
        {
            var hitPose = m_Hit[0].pose;

            if (Input.GetTouch(0).phase == TouchPhase.Began && spawnedObject == null)
            {
                SpawnPrefab(hitPose.position);
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && spawnedObject != null)
            {
                spawnedObject.transform.position = hitPose.position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                spawnedObject = null;
            }
        }
    }

    private void SpawnPrefab(Vector3 spawnPosition)
    {
        spawnedObject = Instantiate(spawnablePrefab, spawnPosition, Quaternion.identity);
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var addedPlane in args.added)
        {
            // Handle added planes (e.g., show a visual indicator or log)
            Debug.Log("Plane added: " + addedPlane.trackableId);
        }
        
        foreach (var updatedPlane in args.updated)
        {
            // Handle updated planes (e.g., update visual indicators or log)
            Debug.Log("Plane updated: " + updatedPlane.trackableId);
        }

        foreach (var removedPlane in args.removed)
        {
            // Handle removed planes (e.g., remove visual indicators or log)
            Debug.Log("Plane removed: " + removedPlane.trackableId);
        }
    }
}