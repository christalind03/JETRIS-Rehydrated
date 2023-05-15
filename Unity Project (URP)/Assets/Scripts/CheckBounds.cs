using UnityEngine;
using System.Collections;

public class CheckBounds : MonoBehaviour
{
    private AudioManager _audioManager;

    private GameQueue _gameQueue;
    private ScoreManager _scoreManager;

    private bool _isColliding = false;

    void Awake()
    {
        _audioManager = AudioManager.instance;
        
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
                _audioManager.PlaySound("Jelly Collision");
            }
            
            if(otherCollider.name == "Grid Floor")
            {
                thisObject.transform.position += Vector3.up;

                _gameQueue.UpdateQueue();
                _audioManager.PlaySound("Jelly Collision");
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