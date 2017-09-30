using UnityEngine;

public class Activity : MonoBehaviour {

    public enum ActivityType { Match, SingleSelect, MultipleChoice  };
    public ActivityType type { get; set; }
    public string id { get; set; }
    public string question { get; set; }
    public int attempts { get; set; }

    private void Start()
    {
        //add events here
    }

    private void OnDisable()
    {
        //remove events here
    }

}
