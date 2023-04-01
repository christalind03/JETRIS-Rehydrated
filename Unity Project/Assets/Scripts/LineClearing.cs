using System;
using UnityEngine;
using System.Collections.Generic;

public class LineClearing : MonoBehaviour
{
    private LevelManager _levelManager;
    private PointsManager _pointsManager;

    void Start()
    {
        _levelManager = GameObject.FindGameObjectWithTag("UI").GetComponent<LevelManager>();
    }

    public void ClearLine(List<GameObject> detectedObjects)
    {
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
}