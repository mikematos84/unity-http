using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uPromise;

public class ScoreComponent : MonoBehaviour {

    private void Start () {
        //ScoreModel.OnScoreUpdate += ScoreModel_ScoreUpdate;
	}

    private void OnDisable()
    {
        ScoreModel.OnScoreUpdate -= ScoreModel_ScoreUpdate;
    }

    private void ScoreModel_ScoreUpdate(int score)
    {
        Debug.Log(score);
    }

}
