using UnityEngine;

public class LineDetection : MonoBehaviour
{
    private int _rowSize = 20;
    private int _columnSize = 10;

    private Vector3 _initialRaycastPosition = new Vector3(-4.5f, 0.5f, -1f);

    void Update()
    {
        // Ray laser = new Ray(_initialRaycastPosition, Vector3.forward);
        // Debug.DrawRay(laser.origin, laser.direction, Color.green, 3f);

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

        for(int columnNum = 0; columnNum < _columnSize; columnNum++)
        {
            Debug.DrawRay(rayCast.origin, rayCast.direction, Color.green, 60f);
            rayCast.origin += Vector3.right;
        }

        return true;
    }
}
