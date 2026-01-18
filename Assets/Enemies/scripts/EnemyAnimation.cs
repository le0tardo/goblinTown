using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void AttackAnimation()
    {
        anim.Play("attack");
    }
    public void IdleAnimation()
    {
        anim.Play("idle");
    }
    public void MoveAnimation()
    {
        anim.Play("move");
    }

    public void DieAnimation()
    {
        anim.Play("die");
    }
}
