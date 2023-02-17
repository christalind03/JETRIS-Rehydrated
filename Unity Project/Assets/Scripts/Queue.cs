using System;
using UnityEngine;
using System.Collections.Generic;

public class Queue : MonoBehaviour
{
    private Permutation permutation = new Permutation();
    private int _generationSeed = DateTime.Now.Millisecond;
    private int[] _allTetrominoes = new int[]{1, 2, 3, 4, 5, 6, 7};

    private bool _holdActivated = false;
    private List<int> _currentBag = new List<int>();

    private GameObject _playablePiece = null;
    private GameObject _queuedPiece1 = null;
    private GameObject _queuedPiece2 = null;
    private GameObject _queuedPiece3 = null;
    private GameObject _queuedPiece4 = null;
    private GameObject _holdPiece = null;

    void Awake()
    {
        int piecesToDisplay = 5;

        for(int i = 0; i < piecesToDisplay; i++)
        {
            UpdateQueue();
        }
    }

    public GameObject GetPlayablePiece()
    {
        return _playablePiece;
    }
    
    public void UpdateQueue()
    {
        if(_currentBag.Count == 0)
        {
            // Generate a random permutation of the given Tetromino set then copy it to the current bag
            permutation.GetRandom(_allTetrominoes, _generationSeed);
            _currentBag.AddRange(_allTetrominoes);
        }

        int objectToSpawn = _currentBag[0];
        _currentBag.RemoveAt(0);

        UpdateObjects(objectToSpawn);
        UpdateTransforms();
    }

    public void UpdateHold()
    {
        if(!_holdActivated)
        {
            if(_holdPiece == null)
            {
                _holdPiece = _playablePiece;
                _playablePiece = null;
                UpdateQueue();
            }
            else
            {
                // Swapping the current piece and the hold piece
                GameObject tempObject = _holdPiece;
                _holdPiece = _playablePiece;
                _playablePiece = tempObject;

                UpdateTransforms();
            }

            _holdActivated = true;
        }
    }

    private void UpdateObjects(int objectToSpawn)
    {
        // Drops the current piece the player has
        if(_playablePiece != null)
        {
            GameObject renderedMesh = _playablePiece.transform.GetChild(1).gameObject;
            renderedMesh.GetComponent<Rigidbody>().useGravity = true;

            // Destroying now useless components
            Destroy(renderedMesh.GetComponent<CheckBounds>());
            foreach(BoxCollider collider in renderedMesh.GetComponents<BoxCollider>())
            {
                Destroy(collider);
            }

            GameObject playablePieceCubes = _playablePiece.transform.GetChild(0).gameObject;
            playablePieceCubes.AddComponent<SoftbodyTetromino>();

            // Creating a fixed joint to make the mesh collider follow the rendered mesh using the rigidbodies connected to the softbody cubes
            // This is important for mesh slicing when clearing lines
            FixedJoint renderedMeshJoint = renderedMesh.AddComponent<FixedJoint>();
            renderedMeshJoint.connectedBody = _playablePiece.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Rigidbody>();

            _holdActivated = false;
        }

        // Updates the queue
        _playablePiece = _queuedPiece1;
        _queuedPiece1 = _queuedPiece2;
        _queuedPiece2 = _queuedPiece3;
        _queuedPiece3 = _queuedPiece4;
        _queuedPiece4 = Spawn(objectToSpawn);
    }

    private void UpdateTransforms()
    {
        if(_playablePiece != null)
        {
            _playablePiece.transform.localScale = new Vector3(1f, 1f, 1f);

            // Adjusting the playable piece position based on its block type
            if(_playablePiece.name == "Zero(Clone)")
            {
                _playablePiece.transform.position = new Vector3(0f, 22f, 0f);
            }
            else if(_playablePiece.name == "Hero(Clone)")
            {
                _playablePiece.transform.position = new Vector3(0f, 21f, 0f);
            }
            else if(_playablePiece.name != "Zero(Clone)" && _playablePiece.name != "Hero(Clone)")
            {
                _playablePiece.transform.position = new Vector3(-0.5f, 21.5f, 0f);
            }

            _queuedPiece1.transform.position = new Vector3(9.5f, 18.5f, -1f);
            _queuedPiece2.transform.position = new Vector3(9.5f, 15.5f, -1f);
            _queuedPiece3.transform.position = new Vector3(9.5f, 12.5f, -1f);
            _queuedPiece4.transform.position = new Vector3(9.5f, 9.5f, -1f);

            // Adjusting the queued piece scale and positions based on its block type
            GameObject[] queuedPieces = {_queuedPiece1, _queuedPiece2, _queuedPiece3, _queuedPiece4};

            foreach(GameObject queuedPiece in queuedPieces)
            {
                queuedPiece.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                if(queuedPiece != null && queuedPiece.name != "Zero(Clone)")
                {
                    queuedPiece.transform.position += new Vector3(0f, -0.25f, 0f);
                }
            }
        }

        if(_holdPiece != null)
        {
            _holdPiece.transform.eulerAngles = new Vector3(0f, 90f, 0f);
            _holdPiece.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            // Adjusting the playable piece position based on its block type
            if(_holdPiece.name == "Zero(Clone)")
            {
                _holdPiece.transform.position = new Vector3(-9.5f, 18.5f, -1f);
            }
            else
            {
                _holdPiece.transform.position = new Vector3(-9.5f, 18.25f, -1f);
            }
        }
    }

    private GameObject Spawn(int objectToSpawn)
    {
        string targetPath = "Prefabs/";

        switch(objectToSpawn)
        {
            case 1:
                targetPath += "Hero";
                break;

            case 2:
                targetPath += "Blue Hook";
                break;

            case 3:
                targetPath += "Orange Hook";
                break;

            case 4:
                targetPath += "Red Skew";
                break;

            case 5:
                targetPath += "Green Skew";
                break;

            case 6:
                targetPath += "Tee";
                break;

            case 7:
                targetPath += "Zero";
                break;
        }

        return Instantiate(Resources.Load<GameObject>(targetPath)); 
    }
}