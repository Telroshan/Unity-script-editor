using UnityEngine;

public class Scaler : MonoBehaviour
{
    [Range(0f, 2f)] public float size = 1f;
    
    void Update()
    {
        float anim = size + Mathf.Sin(Time.time * 8f) * size / 7f;
        transform.localScale = Vector3.one * anim;
    }
}
