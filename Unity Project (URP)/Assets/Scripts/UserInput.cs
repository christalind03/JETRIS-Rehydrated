using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    private AudioManager _audioManager;

    private PlayerControls _playerControls;
    
    private Game _game;
    private GameQueue _gameQueue;
    private LevelManager _levelManager;
    private ScoreManager _scoreManager;

    private PauseMenu _pauseMenu;

    private GameObject _playablePiece;
    private bool _isGamePaused = false;
    private float _timer = 0f;

    void Awake()
    {
        _audioManager = AudioManager.instance;

        _playerControls = new PlayerControls();

        _game = this.gameObject.GetComponent<Game>();
        _levelManager = this.gameObject.GetComponent<LevelManager>();
        _scoreManager = this.gameObject.GetComponent<ScoreManager>();
        _gameQueue = this.gameObject.GetComponent<GameQueue>();

        _pauseMenu = GameObject.Find("Pause Menu (Canvas)").GetComponent<PauseMenu>();
    }

    void OnEnable()
    {
        _playerControls.Enable();    

        _playerControls.Movement.MoveLeft.performed += MoveLeft;
        _playerControls.Movement.MoveRight.performed += MoveRight;
        _playerControls.Movement.HardDrop.performed += HardDrop;
        _playerControls.Movement.SoftDrop.performed += SoftDrop;
        _playerControls.Movement.Rotate.performed += Rotate;
        _playerControls.Movement.Hold.performed += Hold;

        _playerControls.Movement.Pause.performed += Pause;
    }

    void Update()
    {
        _isGamePaused = _game.GetState();
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
        if(!_isGamePaused)
        {
            _playablePiece.transform.position += new Vector3(-1f, 0f, 0f);
        }
    }

    private void MoveRight(InputAction.CallbackContext context)
    {
        if(!_isGamePaused)
        {
            _playablePiece.transform.position += new Vector3(1f, 0f, 0f);
        }
    }

    private void HardDrop(InputAction.CallbackContext context)
    {
        if(!_isGamePaused)
        {
            _gameQueue.UpdateQueue();
            _scoreManager.UpdateBlocksDropped();
            _audioManager.PlaySound("Jelly Collision");
            
            _timer = 0f;
        }
    }

    private void SoftDrop(InputAction.CallbackContext context)
    {
        if(!_isGamePaused)
        {
            _playablePiece.transform.position += new Vector3(0f, -1f, 0f);
            _timer = 0f;
        }
    }

    private void Rotate(InputAction.CallbackContext context)
    {
        if(!_isGamePaused)
        {
            _playablePiece.transform.RotateAround(_playablePiece.transform.position, Vector3.back, 90f);
        }
    }

    private void Hold(InputAction.CallbackContext context)
    {
        if(!_isGamePaused)
        {            
            _gameQueue.UpdateHold();
            _timer = 0f;
        }
    }

    private void Pause(InputAction.CallbackContext context)
    {
        if(_game.GetCountdownState())
        {
            // If the game is not paused and the pause button is pressed, open the menu.
            if(!_isGamePaused)
            {
                _game.PauseGame();
                _pauseMenu.OpenMenu();
            }
            
            // If the game is paused and the pause button is pressed, close the menu.
            if(_isGamePaused)
            {
                _game.ResumeGame();
                _pauseMenu.CloseMenu();
            }
        }
    }

    void OnDisable()
    {
        _playerControls.Disable();

        _playerControls.Movement.MoveLeft.performed -= MoveLeft;
        _playerControls.Movement.MoveRight.performed -= MoveRight;
        _playerControls.Movement.HardDrop.performed -= HardDrop;
        _playerControls.Movement.SoftDrop.performed -= SoftDrop;
        _playerControls.Movement.Rotate.performed -= Rotate;
        _playerControls.Movement.Hold.performed -= Hold;
        
        _playerControls.Movement.Pause.performed -= Pause;
    }
}