using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float _playTime;
    private float _totalPlayTime;
    private bool _gameStart;

    public float PlayTime
    {
        get
        {
            return _playTime;
        }
    }
    public float TotalPlayTime
    {
        get
        {
            return _totalPlayTime;
        }
    }
    public bool GameStart
    {
        get
        {
            return _gameStart;
        }
    }


    public static GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }


        _playTime = 0f;
        _totalPlayTime = 60f;
        _gameStart = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Floor(_playTime) != 60f)
        {
            _playTime += Time.deltaTime;

            Debug.Log(Mathf.Floor(_playTime));
        }
        else
        {
            _gameStart = false;
        }
    }
}
