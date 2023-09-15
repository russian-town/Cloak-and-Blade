using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class EnemyZoneDrawer : MonoBehaviour
{
    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private Vector3[] _vertices;
    private int[] _triangles;

    private readonly int _angleCount = 4;
    private readonly float _offSet = 0.5f;

    private void Awake()
    {
        _mesh = new Mesh();
        _meshFilter = GetComponent<MeshFilter>();
    }

    public void GenerateMesh(List<Cell> cellsInSight)
    {
        if (cellsInSight.Count == 0)
            return;

        _vertices = new Vector3[cellsInSight.Count * 4 + 1];

        for (int j = 0; j < _vertices.Length; j++)
        {
            for (int i = 0; i < cellsInSight.Count; i++)
            {
                _vertices[j] = new Vector3(cellsInSight[i].transform.localPosition.x + _offSet, 0f, cellsInSight[i].transform.localPosition.z - _offSet);
                j++;
                _vertices[j] = new Vector3(cellsInSight[i].transform.localPosition.x - _offSet, 0f, cellsInSight[i].transform.localPosition.z + _offSet);
                j++;
                _vertices[j] = new Vector3(cellsInSight[i].transform.localPosition.x - _offSet, 0f, cellsInSight[i].transform.localPosition.z - _offSet);
                j++;
                _vertices[j] = new Vector3(cellsInSight[i].transform.localPosition.x + _offSet, 0f, cellsInSight[i].transform.localPosition.z + _offSet);
                j++;
            }
        }

        _triangles = new int[(cellsInSight.Count * _angleCount + 1) * 6];

        int indexOfVertice = 0;
        int indexOfTriangle = 0;

        for (int j = 0; j < cellsInSight.Count; j ++)
        {
            _triangles[indexOfTriangle] = indexOfVertice;
            _triangles[indexOfTriangle + 1] = indexOfVertice + 2;
            _triangles[indexOfTriangle + 2] = indexOfVertice + 1;

            _triangles[indexOfTriangle + 3] = indexOfVertice;
            _triangles[indexOfTriangle + 4] = indexOfVertice + 1;
            _triangles[indexOfTriangle + 5] = indexOfVertice + 3;

            _triangles[indexOfTriangle + 6] = indexOfVertice;
            _triangles[indexOfTriangle + 7] = indexOfVertice + 3;
            _triangles[indexOfTriangle + 8] = indexOfVertice + 2;

            _triangles[indexOfTriangle + 9] = indexOfVertice + 1;
            _triangles[indexOfTriangle + 10] = indexOfVertice + 2;
            _triangles[indexOfTriangle + 11] = indexOfVertice + 3;

            indexOfVertice += 4;
            indexOfTriangle += 12;
        }

        AssignMesh();
    }

    public void ClearMesh() => _mesh?.Clear();

    private void AssignMesh()
    {
        _mesh.Clear();
        _mesh.name = "New mesh";
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _meshFilter.mesh = _mesh;
    }
}
