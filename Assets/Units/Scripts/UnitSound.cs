using UnityEngine;

public class UnitSound : MonoBehaviour
{
    [SerializeField]AudioSource source;

    [Header("Sounds")]
    [SerializeField] AudioClip[] AxeChopSounds;
    [SerializeField] AudioClip[] PickAxeSounds;

    public void PlayAxeChop()
    {
        int r=Random.Range(0,AxeChopSounds.Length);
        source.PlayOneShot(AxeChopSounds[r]);
    }

    public void PlayPickAxe()
    {
        int r = Random.Range(0, PickAxeSounds.Length);
        source.PlayOneShot(PickAxeSounds[r]);
    }
}
