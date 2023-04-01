using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    private PlayerControls _playerControls;
    private LevelManager _levelManager;
    private Queue _gameQueue;

    private GameObject _playablePiece;
    private float _timer = 0f;

    void Awake()
    {
        _playerControls = new PlayerControls();
        _levelManager = GameObject.FindGameObjectWithTag("UI").GetComponent<LevelManager>();
        _gameQueue = this.gameObject.GetComponent<Queue>();
    }

    void OnEnable()
    {
        _playerControls.Enable();    

        _playerControls.Game.MoveLeft.performed += MoveLeft;
        _playerControls.Game.MoveRight.performed += MoveRight;
        _playerControls.Game.HardDrop.performed += HardDrop;
        _playerControls.Game.SoftDrop.performed += SoftDrop;
        _playerControls.Game.Rotate.performed += Rotate;
        _playerControls.Game.Hold.performed += Hold;
    }

    void Update()
    {
        _playablePiece = _gameQueue.GetPlayablePiece();
        IdleDrop();
    }

    private void IdleDrop()
    {
        if(_timer >= _levelManager.GetDropSpeed())
        {
            _playablePiece.transform.position += Vector3.down;
            _timer = 0f;
            return;
        }

        _timer += 1 * Time.deltaTime;
    }

    private void MoveLeft(InputAction.CallbackContext context)
    {
        _playablePiece.transform.position += new Vector3(-1f, 0f, 0f);
    }

    private void MoveRight(InputAction.CallbackContext context)
    {
        _playablePiece.transform.position += new Vector3(1f, 0f, 0f);
    }

    private void HardDrop(InputAction.CallbackContext context)
    {
        _gameQueue.UpdateQueue();
        _timer = 0f;
    }

    private void SoftDrop(InputAction.CallbackContext context)
    {
        _playablePiece.transform.position += new Vector3(0f, -1f, 0f);
        _timer = 0f;
    }

    private void Rotate(InputAction.CallbackContext context)
    {
        _playablePiece.transform.RotateAround(_playablePiece.transform.position, Vector3.back, 90f);
    }

    private void Hold(InputAction.CallbackContext context)
    {
        _gameQueue.UpdateHold();
        _timer = 0f;
    }

    void OnDisable()
    {
        _playerControls.Disable();

        _playerControls.Game.MoveLeft.performed -= MoveLeft;
        _playerControls.Game.MoveRight.performed -= MoveRight;
        _playerControls.Game.HardDrop.performed -= HardDrop;
        _playerControls.Game.SoftDrop.performed -= SoftDrop;
        _playerControls.Game.Rotate.performed -= Rotate;
        _playerControls.Game.Hold.performed -= Hold;
    }
}
