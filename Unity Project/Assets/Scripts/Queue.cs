using System;
using UnityEngine;
using System.Collections.Generic;

public class Queue : MonoBehaviour
{
    private Permutation permutation = new Permutation();
    private int _generationSeed = DateTime.Now.Millisecond;
    private int[] _allTetrominoes = new int[]{1, 2, 3, 4, 5, 6, 7};

    private int objectToSpawn = 0;
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

        objectToSpawn = _currentBag[0];
        _currentBag.RemoveAt(0);
        UpdateQueueObjects();
        UpdateQueueTransform();
    }

    private void UpdateQueueObjects()
    {
        if(_playablePiece != null)
        {
            // Enable gravity here
            Debug.Log("Dropped " + _playablePiece.name);
        }

        _playablePiece = _queuedPiece1;
        _queuedPiece1 = _queuedPiece2;
        _queuedPiece2 = _queuedPiece3;
        _queuedPiece3 = _queuedPiece4;
        _queuedPiece4 = Spawn();
    }

    private void UpdateQueueTransform()
    {
        if(_playablePiece != null)
        {
            _playablePiece.transform.localScale = new Vector3(1f, 1f, 1f);
            _playablePiece.transform.position = new Vector3(0f, 22f, 0f);
            
            // Adjusting the position to be within the grid's boundaries
            if(_playablePiece.name == "Hero(Clone)")
            {
                _playablePiece.transform.position = new Vector3(0f, 21f, 0f);
            }
            else if(_playablePiece.name != "Zero(Clone)" && _playablePiece.name != "Hero(Clone)")
            {
                _playablePiece.transform.position = new Vector3(-0.5f, 21.5f, 0f);
            }

            _queuedPiece1.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            _queuedPiece1.transform.position = new Vector3(9.5f, 18.5f, -1f);

            _queuedPiece2.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            _queuedPiece2.transform.position = new Vector3(9.5f, 15.5f, -1f);

            _queuedPiece3.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            _queuedPiece3.transform.position = new Vector3(9.5f, 12.5f, -1f);

            _queuedPiece4.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            _queuedPiece4.transform.position = new Vector3(9.5f, 9.5f, -1f);
        }
    }

    private GameObject Spawn()
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
                targetPath += "Zero";
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
