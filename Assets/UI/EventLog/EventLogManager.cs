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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            AddToLog("User pressed a key!");
        }
    }

    public void AddToLog(string str)
    {
        log.AddString(str);
        AudioManager.inst.PlayGlobalSound(pling);
    }
}
