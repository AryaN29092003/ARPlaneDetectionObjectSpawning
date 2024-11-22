using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class Spawn1 : MonoBehaviour
{
    [SerializeField] GameObject cubePrefab;
    private ARRaycastManager aRRaycastManager;
    private GameObject spawnedCube;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField] public GameObject menuPrefab;
    private GameObject menuInstance;

    private void Start()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        spawnedCube = Instantiate(cubePrefab, new Vector3(0f, 0f, 10f), Quaternion.identity);
        
    }

    private void Update()
    {

        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            Vector2 touchPos = touch.position;

            if (aRRaycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon)){
                spawnedCube.transform.position = hits[0].pose.position;
                if (menuInstance == null)
                    {
                        Vector3 menuPosition = spawnedCube.transform.position + new Vector3(0, 0.5f, 0); // Position it above the cube
                        menuInstance = Instantiate(menuPrefab, menuPosition, Quaternion.identity);
                        Button DeleteButton = menuInstance.transform.Find("DeleteButton").GetComponent<Button>();
                        Button RedButton = menuInstance.transform.Find("RedButton").GetComponent<Button>();
                        DeleteButton.onClick.AddListener();
                        RedButton.onClick.AddListener();
                    }
            }

        }
        
    }

    private void OnChangeColorToRedClick()
    {
       if (spawnedCube != null)
        {
            Renderer cubeRenderer = spawnedCube.GetComponent<Renderer>();
            if (cubeRenderer != null)
            {
                // Change to a random color using HSV for full color range
                cubeRenderer.material.color = Random.ColorHSV();
            }
        }
    }
    
    void OnDeleteButtonClick()
    {
        if (spawnedCube != null)
        {
            Destroy(spawnedCube);
        }
    }
}