using UnityEngine;

public class Sample1 : MonoBehaviour
{
    [Header("Tweaks")]
    [SerializeField] public bool useFriction;
    [Space(10)]
    public bool isFrictionMultiplied = true;
    [SerializeField, Range(0f, 10f)] public float frictionFactor = 2f;
    public int rangeFriction = 1;
}
