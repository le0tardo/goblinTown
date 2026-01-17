using System.Collections.Generic;
using UnityEngine;

public class MiniProductionBarManager : MonoBehaviour
{
    public static MiniProductionBarManager inst;

    [SerializeField] MiniProductionBar barPrefab;
    [SerializeField] Transform canvasRoot;

    Queue<MiniProductionBar> pool = new();
    Dictionary<IProducer, MiniProductionBar> active = new();

    void Awake()
    {
        inst = this;
    }

    public void Show(IProducer producer)
    {
        if (active.ContainsKey(producer))
            return;

        MiniProductionBar bar =
            pool.Count > 0 ? pool.Dequeue() :
            Instantiate(barPrefab, canvasRoot);

        bar.Bind(producer, Camera.main);
        active.Add(producer, bar);
    }

    public void Hide(IProducer producer)
    {
        if (!active.TryGetValue(producer, out var bar))
            return;

        bar.Unbind();
        pool.Enqueue(bar);
        active.Remove(producer);
    }
}
