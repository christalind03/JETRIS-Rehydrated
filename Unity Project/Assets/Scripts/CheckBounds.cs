using UnityEngine;
using System.Collections;

public class CheckBounds : MonoBehaviour
{
    private bool _isColliding = false;
    private Queue _gameQueue;

    void Awake()
    {
        _gameQueue = GameObject.FindWithTag("Game Master").GetComponent<Queue>();
    }

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

            if(otherObject.tag == "Tetromino" && collisionPoint.y <= 20f)
            {
                Debug.Log(otherObject.name);
                Debug.Log(collisionPoint.y);
                thisObject.position += Vector3.up;
                _gameQueue.UpdateQueue();
            }
            
            if(otherCollider.name == "Grid Floor")
            {
                _gameQueue.UpdateQueue();
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