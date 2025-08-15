using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityColorEvent : UnityEvent<Color> { }

[System.Serializable]
public class UnityIntEvent : UnityEvent<int> { }

[System.Serializable]
public class UnityFloatEvent : UnityEvent<float> { }

[System.Serializable]
public class UnityVector3Event : UnityEvent<Vector3> { }
