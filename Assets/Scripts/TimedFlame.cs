using UnityEngine;

public class TimedFlame : MonoBehaviour
{
    public float sec = 10;
    public GameObject effect;
    
    // Update is called once per frame
    void Update()
    {
        sec -= Time.deltaTime;
        if (sec < 0) {
            effect.SetActive(true);
        }
    }
}
