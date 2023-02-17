using UnityEngine;
using System.Collections;

public class CheckBounds : MonoBehaviour
{
    private bool _isColliding = false;
    private int _previousInstanceID = 0;

    void OnTriggerEnter(Collider otherCollider)
    {
        if(_isColliding)
        {
            return;
        }
        else
        {
            _isColliding = true;

            Transform thisObject = this.transform.root;
            Transform otherObject = otherCollider.transform.root;
            int currentInstanceID = otherObject.gameObject.GetInstanceID();
            Vector3 collisionPoint = otherCollider.ClosestPoint(transform.position);

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

            if(otherObject.tag == "Tetromino" && currentInstanceID != _previousInstanceID)
            {
                thisObject.position += Vector3.up;
                _previousInstanceID = currentInstanceID;
                Debug.Log("Dropping this object");
            }

            StartCoroutine(ResetCollision());
        }
    }

    private bool IsPositive(float num)
    {
        return num > 0;
    }

    private IEnumerator ResetCollision()
    {
        yield return new WaitForEndOfFrame();
        _isColliding = false;
    }
}
