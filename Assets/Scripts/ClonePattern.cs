using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ClonePattern : MonoBehaviour
{
    public GameObject OriginalGameObject { get; set; }

    public int NumClones { get; set; } = 1;

    public List<ICloneEffect> CloneEffects { get; set; } = new();

    void Start()
    {
        Init();
    }

    void Update()
    {
        Assert.AreEqual(transform.childCount, NumClones);
        for (int i = 0; i < NumClones; i++)
        {
            GameObject clone = transform.GetChild(i).gameObject;
            ApplyEffects(i, clone);
        }
    }

    public void Init()
    {
        Assert.IsNotNull(OriginalGameObject);
        Clear();
        for (int i = 0; i < NumClones; i++)
        {
            CreateClone(i);
        }
    }

    public void Clear()
    {
        // The foreach loop version does not work for whatever reason
        // foreach (Transform child in transform)
        // {
        //     DestroyImmediate(child.gameObject);
        // }

        // Iterate backwards to avoid issues with changing indices while deleting.
        // Iterating forward effectively skips some children
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    private GameObject CreateClone(int index)
    {
        var clone = Instantiate(OriginalGameObject, transform, true);
        clone.name = OriginalGameObject.name + " Clone " + index;
        ApplyEffects(index, clone);
        return clone;
    }

    private void ApplyEffects(int index, GameObject clone)
    {
        Assert.IsNotNull(CloneEffects);
        foreach (var effect in CloneEffects)
        {
            Assert.IsNotNull(effect);
            effect.Apply(index, clone);
        }
    }
}
