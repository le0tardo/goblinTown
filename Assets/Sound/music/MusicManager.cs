using UnityEngine;
using System.Collections;
public class MusicManager : MonoBehaviour
{
    AudioSource source;
    [SerializeField] AudioClip[] tracks;

    int currentTrack;
    int pauseTime=10;
    private void Start()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(MusicLoop());
    }

    private IEnumerator MusicLoop()
    {
        while (true)
        {
            source.clip = tracks[currentTrack];
            source.Play();

            yield return new WaitWhile(() => source.isPlaying);

            yield return new WaitForSeconds(pauseTime);

            currentTrack++;
            if (currentTrack >= tracks.Length)
            {
                currentTrack = 0;
            }
        }
    }
}
