using UnityEngine;

public class AnimalAnimation : MonoBehaviour
{
    Animator anim;
 
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        float r =Random.Range(0f, 1f);
        anim.Play("idle",0,r);
    }

    public void Flee()
    {
        anim.SetTrigger("run");
    }
    public void Graze()
    {
        anim.SetTrigger("idle");
    }

    public void Hurt()
    {
        anim.SetTrigger("hurt");
    }

    public void Die()
    {
        anim.SetTrigger("die");
    }
}
