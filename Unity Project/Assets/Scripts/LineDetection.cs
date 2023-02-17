using UnityEngine;

public class LineDetection : MonoBehaviour
{
    private int _rowSize = 20;
    private int _columnSize = 10;

    private Vector3 _initialRaycastPosition = new Vector3(-4.5f, 0.5f, -1f);

    void Update()
    {
        CheckRows();
    }

    private void CheckRows()
    {
        for(int rowNum = 0; rowNum < _rowSize; rowNum++)
        {
            IsRowFull(rowNum);
        }
    }

    private bool IsRowFull(int numRow)
    {
        Ray rayCast = new Ray(_initialRaycastPosition + (Vector3.up * numRow), Vector3.forward);
        RaycastHit rayCastHit;

        for(int columnNum = 0; columnNum < _columnSize; columnNum++)
        {
            if(!Physics.Raycast(rayCast, out rayCastHit))
            {
                return false;
            }

            rayCast.origin += Vector3.right;
        }

        return true;
    }
}