using UnityEngine;

public class ButterflyBehaviour : MonoBehaviour
{
    [SerializeField] Animator anim;

    private void Start()
    {
        float r = Random.value;
        anim.Play("fly",0,r);
        Invoke("Kill", 2f);
    }

    void Kill()
    {
        this.gameObject.SetActive(false);
    }

}
