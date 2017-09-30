using System.Collections.Generic;
using UnityEngine;

public class Challenge : MonoBehaviour {

    public int id { get; set; }
    public string challengeName { get; set; }
    public List<Activity> activities { get; set; }

    private void Start()
    {
        //add events here
    }

    private void OnDisable()
    {
        //remove events here
    }

}
