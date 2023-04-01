using UnityEngine;
using System.Collections.Generic;

public class LineClearing : MonoBehaviour
{
    public void ClearLine(int rowNum, List<GameObject> detectedObjects)
    {
        List<GameObject> rootObjects = GetRootObjects(detectedObjects);
        DisableObjects(detectedObjects);
    }

    private void DisableObjects(List<GameObject> detectedObjects)
    {
        foreach(GameObject detectedObject in detectedObjects)
        {
            detectedObject.SetActive(false);
        }
    }

    private List<GameObject> GetRootObjects(List<GameObject> detectedObjects)
    {
        List<GameObject> rootObjects = new List<GameObject>();

        foreach(GameObject detectedObject in detectedObjects)
        {
            GameObject rootObject = detectedObject.transform.root.gameObject;

            if(!rootObjects.Contains(rootObject))
            {
                rootObjects.Add(rootObject);
            }
        }

        return rootObjects;
    }
}