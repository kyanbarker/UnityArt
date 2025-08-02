using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationAxis
{
    X,
    Y,
    Z,
}

public class CopyAndTransform1 : MonoBehaviour
{
    private readonly Dictionary<RotationAxis, Vector3> rotationAxisVectors = new()
    {
        [RotationAxis.X] = Vector3.right,
        [RotationAxis.Y] = Vector3.up,
        [RotationAxis.Z] = Vector3.forward,
    };

    [SerializeField]
    private int numCopies = 0;

    [SerializeField]
    private Vector3 deltaPosition = Vector3.zero;

    [SerializeField]
    private Vector3 deltaScale = Vector3.one;

    [SerializeField]
    private float rotationDegrees = 0;

    [SerializeField]
    private RotationAxis rotationAxis = RotationAxis.X;

    [SerializeField]
    private bool useParentBPM = true;

    [SerializeField]
    private float bpm = 120f;

    [SerializeField]
    private float delayBeats = 1f;

    private BPMTime cachedBPMComponent;

    public float BPM
    {
        get
        {
            if (useParentBPM)
            {
                if (cachedBPMComponent == null)
                {
                    CacheBPMComponent();
                }

                if (cachedBPMComponent != null)
                {
                    return cachedBPMComponent.BPM;
                }
            }

            return bpm;
        }
        set => bpm = Mathf.Max(Mathf.Epsilon, value);
    }

    public bool UseParentBPM
    {
        get => useParentBPM;
        set
        {
            useParentBPM = value;
            if (value)
                CacheBPMComponent();
        }
    }

    [SerializeField]
    private List<GameObject> gameObjectsToClone;

    private void OnValidate()
    {
        bpm = Mathf.Max(Mathf.Epsilon, bpm);
        delayBeats = Mathf.Max(0, delayBeats);
        numCopies = Mathf.Max(0, numCopies);
    }

    private void Start()
    {
        StartCoroutine(CreateCopiesOverTime());
    }

    private void CacheBPMComponent()
    {
        cachedBPMComponent = GetComponentInParent<BPMTime>();
    }

    private float GetSecondsFromBeats(float beats)
    {
        float beatsPerSecond = BPM / 60;
        return beats / beatsPerSecond;
    }

    private void CreateCopy(int copyIndex)
    {
        foreach (var gameObjectToClone in gameObjectsToClone)
        {
            if (gameObjectToClone.CompareTag("Clone"))
                return;

            var clone = Instantiate(gameObjectToClone, transform, true);
            clone.name += " " + copyIndex;
            clone.transform.localPosition = deltaPosition * copyIndex;
            clone.transform.rotation *= Quaternion.AngleAxis(
                rotationDegrees * copyIndex,
                rotationAxisVectors[rotationAxis]
            );
            clone.transform.localScale = Vector3.Scale(
                gameObjectToClone.transform.localScale,
                new Vector3(
                    Mathf.Pow(deltaScale.x, copyIndex),
                    Mathf.Pow(deltaScale.y, copyIndex),
                    Mathf.Pow(deltaScale.z, copyIndex)
                )
            );
            clone.tag = "Clone";
        }
    }

    private IEnumerator CreateCopiesOverTime()
    {
        int remainingCopies = numCopies;
        int copyIndex = 1;
        while (remainingCopies > 0)
        {
            // Wait first, then create (for consistent timing)
            float delayInSeconds = GetSecondsFromBeats(delayBeats);
            yield return new WaitForSeconds(delayInSeconds);

            CreateCopy(copyIndex);
            copyIndex++;
            remainingCopies--;
        }
    }
}
