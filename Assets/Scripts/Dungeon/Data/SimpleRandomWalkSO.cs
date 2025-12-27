using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRandomWalkParamters_", menuName = "PCG/SimpleRandomWalkDatta")]
public class SimpleRandomWalkSO : ScriptableObject
{
    public int iterations = 10, walkLength = 10;
    public bool startRandomlyEachIteration = true;
}
