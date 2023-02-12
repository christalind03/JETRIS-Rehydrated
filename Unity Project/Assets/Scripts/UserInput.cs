using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControls _playerControls;
    private GameObject _playablePiece;
    private Queue _gameQueue;

    void Awake()
    {
        _playerControls = new PlayerControls();
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
        Debug.Log("Dropping Tetromino...");
    }

    private void SoftDrop(InputAction.CallbackContext context)
    {
        Debug.Log("Moving down...");
    }

    private void Rotate(InputAction.CallbackContext context)
    {
        _playablePiece.transform.RotateAround(_playablePiece.transform.position, Vector3.back, 90f);
    }

    private void Hold(InputAction.CallbackContext context)
    {
        Debug.Log("Holding Tetromino...");
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
