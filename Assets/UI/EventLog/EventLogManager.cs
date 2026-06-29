using UnityEngine;

public class EventLogManager : MonoBehaviour
{
    public static EventLogManager inst;
    [SerializeField] EventLog log;

    [SerializeField] AudioClip pling;
    private void Awake()
    {
        inst = this;
    }

    public void AddToLog(string str)
    {
        log.AddString(str);
        AudioManager.inst.PlayGlobalSound(pling);
    }
}
