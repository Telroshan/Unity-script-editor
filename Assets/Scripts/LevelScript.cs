using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public int experience;
    public int Level => experience / 750;
}
