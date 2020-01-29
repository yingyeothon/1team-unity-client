using UnityEngine;

public class TimedFlameSfx : MonoBehaviour
{
    public float sec1 = 10;
    public float sec2 = 20;
    public float sec3 = 30;
    public AudioSource audioSource;
    public GameObject sfx;
    
    // Update is called once per frame
    void Update()
    {
        sec1 -= Time.deltaTime;
        sec2 -= Time.deltaTime;
        sec3 -= Time.deltaTime;
        if (sec1 < 0) {
            sfx.SetActive(true);
            audioSource.volume = 0.3f;
        }
        if (sec2 < 0) {
            audioSource.volume = 0.5f;
        }
        if (sec3 < 0) {
            audioSource.volume = 0.9f;
        }
    }
}
