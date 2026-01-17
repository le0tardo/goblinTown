using UnityEngine;
using UnityEngine.UI;

public class MiniProductionBar : MonoBehaviour
{
    [SerializeField] Image fill;

    IProducer producer;
    Camera cam;

    public void Bind(IProducer producer, Camera cam)
    {
        this.producer = producer;
        this.cam = cam;
        gameObject.SetActive(true);
    }

    public void Unbind()
    {
        producer = null;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (producer == null || !producer.IsProducing)
        {
            Unbind();
            return;
        }

        fill.fillAmount = producer.Progress01;

        Vector3 worldPos = producer.WorldTransform.position + Vector3.up * 2f;
        transform.position = cam.WorldToScreenPoint(worldPos);
    }
}
