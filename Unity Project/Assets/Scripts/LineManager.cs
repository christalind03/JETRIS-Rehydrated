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

        InvokeRepeating("UpdateRows", 0, 0.75f);
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

        // Disable the softbody cubes and their correlating meshes
        foreach(GameObject detectedObject in detectedObjects)
        {
            int cubeNumber = Convert.ToInt32(detectedObject.name[detectedObject.name.Length - 1].ToString());
            GameObject detectedObjectMesh = detectedObject.transform.root.GetChild(1).GetChild(cubeNumber - 1).gameObject;

            detectedObject.SetActive(false);
            detectedObjectMesh.SetActive(false);
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
}