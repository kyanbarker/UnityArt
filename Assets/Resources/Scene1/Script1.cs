using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Script1 : MonoBehaviour
{
    private LFOWaveform sineWaveform;
    private LFOWaveform linearWaveform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        GameObject sinePrefab = Resources.Load<GameObject>("Waveforms/Sine");
        sineWaveform = sinePrefab.GetComponent<LFOWaveform>();

        GameObject linearPrefab = Resources.Load<GameObject>("Waveforms/Linear");
        linearWaveform = linearPrefab.GetComponent<LFOWaveform>();

        GameObject CreatePrismY(string name = "Prism")
        {
            GameObject prism = CreatePrism(name);
            prism.transform.localRotation = Quaternion.Euler(90, 0, 0);
            return prism;
        }

        GameObject CreatePrismX(string name = "Prism")
        {
            GameObject prism = CreatePrism(name);
            prism.transform.localRotation = Quaternion.Euler(0, 90, 0);
            return prism;
        }

        GameObject CreatePrismZ(string name = "Prism")
        {
            return CreatePrism(name);
        }

        GameObject CreatePrism(string name = "Prism")
        {
            GameObject prism = new(name);
            GameObject rotation = new("Rotation");
            rotation.transform.parent = prism.transform;

            GameObject CreatePlane(string name = "Plane")
            {
                GameObject plane = new(name);

                GameObject linePrefab = Resources.Load<GameObject>("Scene1/Line");
                ClonePattern clonePattern = plane.AddComponent<ClonePattern>();

                clonePattern.OriginalGameObject = linePrefab;
                clonePattern.NumClones = 51;
                clonePattern.DeltaPosition = new Vector3(2, 0, 0);

                clonePattern.ColorMode = ColorMode.Gradient;
                clonePattern.GradientLength = 51;
                clonePattern.Gradient.colorKeys = new GradientColorKey[]
                {
                    new(Color.yellow, 0),
                    new(Color.cyan, 1 / 6f),
                    new(Color.blue, 2 / 6f),
                    new(Color.magenta, 3 / 6f),
                    new(Color.blue, 4 / 6f),
                    new(Color.cyan, 5 / 6f),
                    new(Color.yellow, 6 / 6f),
                };

                // lerp gradient length between 51 and 102 using a sine wave over 32 seconds
                // starting at minimum length
                void SetupGradientController()
                {
                    LFOIntTarget target = plane.AddComponent<LFOIntTarget>();
                    target.Min = 51;
                    target.Max = 102;
                    target.action = new UnityEvent<int>();
                    target.action.AddListener(length =>
                    {
                        clonePattern.GradientLength = length;
                    });
                    LFOController controller = plane.AddComponent<LFOController>();
                    controller.Waveform = sineWaveform;
                    controller.Frequency = 1f / 32f; // one cycle every 32 seconds
                    controller.PhaseOffset = 0.75f; // start at minimum length
                    controller.Targets = new LFOTarget[] { target };
                }
                SetupGradientController();

                // lerp delta x between 1 and 2 using a sine wave over 8 seconds
                // lfo(t = 0) = 0.5 ==> delta x = 1.5
                void SetupDeltaXController()
                {
                    LFOFloatTarget target = plane.AddComponent<LFOFloatTarget>();
                    target.Min = 1f;
                    target.Max = 2f;
                    target.action = new UnityEvent<float>();
                    target.action.AddListener(deltaX =>
                    {
                        clonePattern.DeltaPosition = new Vector3(deltaX, 0, 0);
                    });
                    LFOController controller = plane.AddComponent<LFOController>();
                    controller.Waveform = sineWaveform;
                    controller.Frequency = 1f / 8f; // one cycle every 8 seconds
                    controller.Targets = new LFOTarget[] { target };
                }
                SetupDeltaXController();

                return plane;
            }

            GameObject bottomPlane = CreatePlane("Bottom Plane");
            GameObject topPlane = CreatePlane("Top Plane");
            GameObject leftPlane = CreatePlane("Left Plane");
            GameObject rightPlane = CreatePlane("Right Plane");

            // for each plane, set parent to rotation
            new List<GameObject>() { bottomPlane, topPlane, leftPlane, rightPlane }.ForEach(plane =>
                plane.transform.parent = rotation.transform
            );

            // for each plane, set local position and rotation
            bottomPlane.transform.localPosition = new Vector3(-25, -25, 0);

            topPlane.transform.localPosition = new Vector3(25, 25, 0);
            topPlane.transform.localScale = new Vector3(-1, 1, 1);

            rightPlane.transform.localPosition = new Vector3(25, 25, 0);
            rightPlane.transform.localRotation = Quaternion.Euler(0, 0, 90);
            rightPlane.transform.localScale = new Vector3(-1, 1, 1);

            leftPlane.transform.localPosition = new Vector3(-25, -25, 0);
            leftPlane.transform.localRotation = Quaternion.Euler(0, 0, 90);

            rotation.transform.localRotation = Quaternion.Euler(0, 0, -45);

            return prism;
        }

        void SetupPrisms()
        {
            float prismDistance = 500;

            GameObject centerY = CreatePrismY("Center Y");

            GameObject leftY = CreatePrismY("Left Y");
            leftY.transform.position = Vector3.left * prismDistance;

            GameObject rightY = CreatePrismY("Right Y");
            rightY.transform.position = Vector3.right * prismDistance;

            GameObject backY = CreatePrismY("Back Y");
            backY.transform.position = Vector3.back * prismDistance;

            GameObject frontY = CreatePrismY("Front Y");
            frontY.transform.position = Vector3.forward * prismDistance;

            GameObject Z = CreatePrismZ("Z");

            GameObject X = CreatePrismX("X");

            void SetupPrismRotationController()
            {
                GameObject prisms = new("Prisms");
                new List<GameObject>() { centerY, leftY, rightY, backY, frontY, Z, X }.ForEach(
                    prism => prism.transform.parent = prisms.transform
                );
                LFOFloatTarget target = prisms.AddComponent<LFOFloatTarget>();
                target.Min = -180f;
                target.Max = 180f;
                target.action = new UnityEvent<float>();
                target.action.AddListener(yRotation =>
                {
                    prisms.transform.localRotation = Quaternion.Euler(0, yRotation, 0);
                });
                LFOController controller = prisms.AddComponent<LFOController>();
                controller.Waveform = linearWaveform;
                controller.Frequency = 1f / 8f;
                controller.Targets = new LFOTarget[] { target };
            }
            SetupPrismRotationController();
        }
        SetupPrisms();

        void SetupCameraFovController()
        {
            Camera mainCamera = Camera.main;
            LFOFloatTarget target = mainCamera.gameObject.AddComponent<LFOFloatTarget>();
            target.Min = 30f;
            target.Max = 60f;
            target.action = new UnityEvent<float>();
            target.action.AddListener(fov =>
            {
                mainCamera.fieldOfView = fov;
            });
            LFOController controller = mainCamera.gameObject.AddComponent<LFOController>();
            controller.Waveform = sineWaveform;
            controller.Frequency = 1f / 4f; // one cycle every 4 seconds
            controller.PhaseOffset = 0.75f; // start at minimum fov
            controller.Targets = new LFOTarget[] { target };
        }
        SetupCameraFovController();
    }
}
