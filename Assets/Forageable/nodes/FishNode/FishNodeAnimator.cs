using UnityEngine;

public class FishNodeAnimator : MonoBehaviour
{
    NodeBehaviour node;
    Animator anim;
    bool depleated=false;
    private void Start()
    {
        node = GetComponentInParent<NodeBehaviour>();
        anim = GetComponent<Animator>();
        anim.Play("idle");
    }

    private void Update()
    {
        if (node.isDepleted && !depleated)
        {
            anim.Play("depleat");
            depleated = true;
        }

        if(!node.isDepleted&& depleated)
        {
            anim.Play("idle");
            depleated=false;
        }
    }

}
