using System;
using UnityEngine;

public class CheckBounds : MonoBehaviour
{
    void OnTriggerEnter(Collider otherCollider)
    {
        Debug.Log("I hit something!");
        Transform thisObject = this.transform.root;
        Transform otherObject = otherCollider.transform.root;
        Vector3 collisionPoint = otherCollider.ClosestPoint(transform.position);
        // Debug.Log(collisionPoint);
        // Debug.Log(otherCollider.name);

        if(otherCollider.name == "Grid Pillars")
        {
            if(IsPositive(collisionPoint.x))
            {
                thisObject.position += Vector3.left;
            }
            else
            {
                thisObject.position += Vector3.right;
            }
        }

        if(otherObject.tag == "Tetromino")
        {
            thisObject.position += Vector3.up;
            Debug.Log("Dropping this object");
        }
    }

    private bool IsPositive(float num)
    {
        return num > 0;
    }
}
