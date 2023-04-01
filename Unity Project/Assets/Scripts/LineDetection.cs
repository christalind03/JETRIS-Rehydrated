using UnityEngine;
using System.Collections.Generic;

public class LineDetection : MonoBehaviour
{
    private int _rowSize = 20;
    private int _columnSize = 10;

    private Vector3 _initialRaycastPosition = new Vector3(-4.5f, 0.5f, -1f);
    private LineClearing _lineClearing;

    void Awake()
    {
        _lineClearing = this.gameObject.GetComponent<LineClearing>();    
    }

    void Update()
    {
        CheckRows();
    }

    private void CheckRows()
    {
        for(int rowNum = 0; rowNum < _rowSize; rowNum++)
        {
            if(IsRowFull(rowNum))
            {
                List<GameObject> detectedObjects = GetDetectedObjectList(rowNum);
                _lineClearing.ClearLine(detectedObjects);
            }
        }
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