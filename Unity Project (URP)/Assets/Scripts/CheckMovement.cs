using UnityEngine;

public class CheckMovement : MonoBehaviour
{
    private bool _isMoving;
    private float _previousHeight;

    void Awake()
    {
        _isMoving = true;
        _previousHeight = this.transform.position.y;
    
        InvokeRepeating("UpdateHeight", 0.25f, 0.25f);
    }

    private void UpdateHeight()
    {
        float currentHeight = this.transform.position.y;

        if(_previousHeight - 0.025f < currentHeight && currentHeight < _previousHeight + 0.025f)
        {
            _isMoving = false;
        }
        else
        {
            _isMoving = true;
        }

        _previousHeight = currentHeight;
    }

    public bool IsMoving()
    {
        return _isMoving;
    }
}