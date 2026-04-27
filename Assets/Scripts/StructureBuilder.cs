using UnityEngine;

// Procedurally stacks blocks into a grid tower at runtime
public class StructureBuilder : MonoBehaviour
{
    [SerializeField] GameObject blockPrefab;
    [SerializeField] int columns = 4;
    [SerializeField] int rows = 6;
    [SerializeField] int depth = 2;
    [SerializeField] Vector3 blockSize = Vector3.one;
    [SerializeField] float spacing = 0.05f;

    void Start() => Build();

    void Build()
    {
        Vector3 step = blockSize + Vector3.one * spacing;
        Vector3 origin = transform.position;

        for (int x = 0; x < columns; x++)
        for (int y = 0; y < rows; y++)
        for (int z = 0; z < depth; z++)
        {
            // y offset so the bottom row sits on the ground at origin.y
            Vector3 pos = origin + new Vector3(x * step.x, blockSize.y * 0.5f + y * step.y, z * step.z);
            Instantiate(blockPrefab, pos, Quaternion.identity, transform);
        }
    }
}
