using UnityEngine;
using System.Collections;

public class CheckBounds : MonoBehaviour
{
    private bool _isColliding = false;
    private GameQueue _gameQueue;
    private ScoreManager _scoreManager;

    void Awake()
    {
        _gameQueue = GameObject.FindWithTag("Game Master").GetComponent<GameQueue>();
        _scoreManager = GameObject.FindWithTag("Game Master").GetComponent<ScoreManager>();
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

            GameObject thisObject = this.transform.root.gameObject;
            Vector3 collisionPoint = otherCollider.ClosestPoint(transform.position);

            if(otherCollider.name == "Grid Pillars")
            {
                if(IsPositive(collisionPoint.x))
                {
                    thisObject.transform.position += Vector3.left;
                }
                else
                {
                    thisObject.transform.position += Vector3.right;
                }
            }

            int meshParentIndex = 1;
            GameObject otherObject = otherCollider.transform.root.gameObject;
            GameObject otherObjectMeshParent = otherObject.transform.GetChild(meshParentIndex).gameObject;

            if(otherObject.tag == "Tetromino" && otherObjectMeshParent.TryGetComponent<CheckMovement>(out CheckMovement otherObjectMovement) && otherObjectMovement.IsMoving() == false && collisionPoint.y <= 20f)
            {
                thisObject.transform.position += Vector3.up;

                _gameQueue.UpdateQueue();
                _scoreManager.UpdateBlocksDropped();
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