using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreModel : MonoBehaviour {

    
    private int _score = 100;

    // Events
    public delegate void ScoreUpdateHandler(int score);
    public static event ScoreUpdateHandler OnScoreUpdate;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => DataStore.Instance.isLoaded);
        InvokeRepeating("SimulateScoreUpdate", 0.0f, 1.0f);
    }

    public int score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            if (OnScoreUpdate != null)
            {
                OnScoreUpdate(_score);
            }
        }
    }

    private void SimulateScoreUpdate()
    {
        score++;
    }   

}
