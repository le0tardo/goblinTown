using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager inst;
    [SerializeField] AudioSource source;

    [Header("Sounds")]
    [SerializeField] AudioClip[] pops;
    [SerializeField] AudioClip multiPop;
    private void Awake()
    {
        inst = this;
        source = GetComponent<AudioSource>();
    }

    public void PlayGlobalSound(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    public void SelectUnit()
    {
        int r = Random.Range(0, pops.Length);
        source.PlayOneShot(pops[r]);
    }

    public void SelectUnits()
    {
        source.PlayOneShot(multiPop);
    }

}
