using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource introSource, loopSourse;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        introSource.Play();
        loopSourse.PlayScheduled(AudioSettings.dspTime + introSource.clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
