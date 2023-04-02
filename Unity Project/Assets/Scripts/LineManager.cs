using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineManager : MonoBehaviour
{
    private int _rowSize = 20;
    private int _columnSize = 10;

    private Vector3 _initialRaycastPosition = new Vector3(-4.5f, 0.5f, -1f);
    private LevelManager _levelManager;
    private ScoreManager _scoreManager;

    void Awake()
    {
        _levelManager = this.gameObject.GetComponent<LevelManager>();
        _scoreManager = this.gameObject.GetComponent<ScoreManager>();

        InvokeRepeating("UpdateRows", 0, 1.15f);
    }

    private void UpdateRows()
    {   
        int numRowsCleared = 0;

        // Check to see if a row is full. If it is, clear it and add to the number of rows cleared
        for(int rowNum = 0; rowNum < _rowSize; rowNum++)
        {
            if(IsRowFull(rowNum))
            {
                ClearLine(rowNum);
                numRowsCleared++;
            }
        }

        _scoreManager.UpdateScore(_levelManager.GetCurrentLevel(), numRowsCleared);
    }

    private bool IsRowFull(int numRow)
    {
        Ray raycast = new Ray(_initialRaycastPosition + (Vector3.up * numRow), Vector3.forward);
        RaycastHit raycastHit;

        for(int columnNum = 0; columnNum < _columnSize; columnNum++)
        {
            if(!Physics.Raycast(raycast, out raycastHit))
            {
                return false;
            }

            raycast.origin += Vector3.right;
        }

        return true;
    }

    private void ClearLine(int numRow)
    {
        List<GameObject> detectedObjects = GetDetectedObjectList(numRow);
        int meshParentIndex = 1;

        // Disable the softbody cubes and their correlating meshes
        foreach(GameObject detectedObject in detectedObjects)
        {
            GameObject rootObject = detectedObject.transform.root.gameObject;
            GameObject meshParent = rootObject.transform.GetChild(meshParentIndex).gameObject;

            // If the mesh parent has more than one child, disable only the detected objects. Otherwise, destroy the entire object from the root.
            if(CountActiveChildren(meshParent) > 1)
            {
                int cubeNumber = Convert.ToInt32(detectedObject.name[detectedObject.name.Length - 1].ToString());
                GameObject detectedObjectMesh = meshParent.transform.GetChild(cubeNumber - 1).gameObject;

                detectedObject.SetActive(false);
                detectedObjectMesh.SetActive(false);
            }
            else
            {
                Destroy(rootObject);
            }
        }
        
        _levelManager.UpdateLinesCleared();
    }

    private List<GameObject> GetDetectedObjectList(int numRow)
    {
        List<GameObject> allDetectedObjects = new List<GameObject>();
        Ray raycast = new Ray(_initialRaycastPosition + (Vector3.up * numRow), Vector3.forward);
        RaycastHit raycastHit;

        for(int columnNum = 0; columnNum < _columnSize; columnNum++)
        {
            if(Physics.Raycast(raycast, out raycastHit))
            {
                GameObject detectedObject = raycastHit.collider.transform.gameObject;
                
                if(!allDetectedObjects.Contains(detectedObject))
                {
                    allDetectedObjects.Add(detectedObject);
                }
            }

            raycast.origin += Vector3.right;
        }

        return allDetectedObjects;
    }

    private int CountActiveChildren(GameObject parentObject)
    {
        int numActiveChildren = 0;

        foreach(Transform child in parentObject.transform)
        {
            if(child.gameObject.activeSelf)
            {
                numActiveChildren++;
            }
        }

        return numActiveChildren;
    }
}