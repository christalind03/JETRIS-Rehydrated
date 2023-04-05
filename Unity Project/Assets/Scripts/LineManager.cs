using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
    }

    void Update()
    {   
        int numRowsCleared = 0;

        if(IsGameEnd())
        {
            Debug.Log("Game Over!");
            SceneManager.LoadScene("Game Over");
        }

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

    private bool IsGameEnd()
    {
        int meshParentIndex = 1;

        Ray raycast = new Ray(_initialRaycastPosition + (Vector3.up * (_rowSize)), Vector3.forward);
        RaycastHit raycastHit;

        for(int columnNum = 0; columnNum < _columnSize; columnNum++)
        {
            if(Physics.Raycast(raycast, out raycastHit))
            {
                GameObject detectedObjectMeshParent = raycastHit.collider.transform.root.GetChild(meshParentIndex).gameObject;

                if(detectedObjectMeshParent.TryGetComponent<CheckMovement>(out CheckMovement detectedObjectMovement) && detectedObjectMovement.IsMoving() == false)
                {
                    return true;
                }
            }

            raycast.origin += Vector3.right;
        }

        return false;
    }

    private bool IsRowFull(int numRow)
    {
        int meshParentIndex = 1;

        Ray raycast = new Ray(_initialRaycastPosition + (Vector3.up * numRow), Vector3.forward);
        RaycastHit raycastHit;

        for(int columnNum = 0; columnNum < _columnSize; columnNum++)
        {
            if(Physics.Raycast(raycast, out raycastHit))
            {
                GameObject detectedObjectMeshParent = raycastHit.collider.transform.root.GetChild(meshParentIndex).gameObject;

                if(detectedObjectMeshParent.GetComponent<CheckMovement>().IsMoving() == true)
                {
                    return false;
                }
    
                raycast.origin += Vector3.right;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    private void ClearLine(int numRow)
    {
        int cubeParentIndex = 0;
        int meshParentIndex = 1;
        List<GameObject> detectedObjects = GetDetectedObjectList(numRow);

        // Disable the softbody cubes and their correlating meshes
        foreach(GameObject detectedObject in detectedObjects)
        {
            GameObject rootObject = detectedObject.transform.root.gameObject;
            GameObject cubeParent = rootObject.transform.GetChild(cubeParentIndex).gameObject;
            GameObject meshParent = rootObject.transform.GetChild(meshParentIndex).gameObject;

            if(CountActiveChildren(meshParent) > 1)
            {
                // If the mesh parent has more than one chid, disable only the detected objects along with their mesh.
                int cubeNumber = Convert.ToInt32(detectedObject.name[detectedObject.name.Length - 1].ToString());
                GameObject detectedObjectMesh = meshParent.transform.GetChild(cubeNumber - 1).gameObject;

                detectedObject.SetActive(false);
                detectedObjectMesh.SetActive(false);

                // Additionally, change the fixed joint's connected rigidbody to the first active child
                meshParent.GetComponent<FixedJoint>().connectedBody = GetFirstActiveChild(cubeParent).GetComponent<Rigidbody>();
            }
            else
            {
                // Otherwise, destroy the entire object from the root.
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

    private GameObject GetFirstActiveChild(GameObject parentObject)
    {
        foreach(Transform childTransform in parentObject.transform)
        {
            GameObject childObject = childTransform.gameObject;

            if(childObject.activeSelf)
            {
                return childObject;
            }
        }

        Debug.LogError($"No active child found in {parentObject.name}");
        return null;
    }
}