using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

    public int id { get; set; }
    public string worldName { get; set; }
    public List<Challenge> challenges { get; set; }

    private void Start()
    {
        //add events here
    }

    private void OnDisable()
    {
        //remove events here
    }

}
