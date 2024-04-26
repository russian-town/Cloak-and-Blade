using System.Collections.Generic;
using Source.Gameboard.Cell;
using Source.Root;
using UnityEngine;

namespace Source.Enemy
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class EnemyZoneDrawer : MonoBehaviour
    {
        private readonly int _angleCount = 4;
        private readonly int _verticiesRatio = 6;
        private readonly int _startTriangleIndexRatio = 1;
        private readonly int _triangleVerticeRatio = 4;
        private readonly int _triangleTopVertexNumber = 2;
        private readonly int _triangleLeftVertexNumber = 1;
        private readonly int _triangleRightVertexNumber = 3;
        private readonly float _offSet = 0.5f;

        private Mesh _mesh;
        private MeshFilter _meshFilter;
        private Vector3[] _vertices;
        private int[] _triangles;
        private int _triangleIndexRatio;

        private void Awake()
        {
            _mesh = new ();
            _meshFilter = GetComponent<MeshFilter>();
        }

        public void GenerateMesh(List<Cell> cellsInSight)
        {
            if (cellsInSight.Count == 0)
                return;

            _vertices = new Vector3[cellsInSight.Count * _angleCount + _startTriangleIndexRatio];

            for (int j = 0; j < _vertices.Length; j++)
            {
                for (int i = 0; i < cellsInSight.Count; i++)
                {
                    _vertices[j] = new Vector3(
                        cellsInSight[i].transform.localPosition.x + _offSet,
                        0f,
                        cellsInSight[i].transform.localPosition.z - _offSet);
                    j++;
                    _vertices[j] = new Vector3
                    (cellsInSight[i].transform.localPosition.x - _offSet,
                        0f,
                        cellsInSight[i].transform.localPosition.z + _offSet);
                    j++;
                    _vertices[j] = new Vector3(
                        cellsInSight[i].transform.localPosition.x - _offSet,
                        0f,
                        cellsInSight[i].transform.localPosition.z - _offSet);
                    j++;
                    _vertices[j] = new Vector3(
                        cellsInSight[i].transform.localPosition.x + _offSet,
                        0f,
                        cellsInSight[i].transform.localPosition.z + _offSet);
                    j++;
                }
            }

            _triangles = new int[(cellsInSight.Count * _angleCount + _startTriangleIndexRatio) * _verticiesRatio];

            int indexOfVertice = 0;
            int indexOfTriangle = 0;

            for (int j = 0; j < cellsInSight.Count; j++)
            {
                _triangleIndexRatio = _startTriangleIndexRatio;
                _triangles[indexOfTriangle] = indexOfVertice;
                _triangles[indexOfTriangle + _triangleIndexRatio] = indexOfVertice + _triangleTopVertexNumber;
                _triangleIndexRatio++;
                _triangles[indexOfTriangle + _triangleIndexRatio] = indexOfVertice + _triangleLeftVertexNumber;
                _triangleIndexRatio++;
                _triangles[indexOfTriangle + _triangleIndexRatio] = indexOfVertice;
                _triangleIndexRatio++;
                _triangles[indexOfTriangle + _triangleIndexRatio] = indexOfVertice + _triangleLeftVertexNumber;
                _triangleIndexRatio++;
                _triangles[indexOfTriangle + _triangleIndexRatio] = indexOfVertice + _triangleRightVertexNumber;
                _triangleIndexRatio++;
                _triangles[indexOfTriangle + _triangleIndexRatio] = indexOfVertice;
                _triangleIndexRatio++;
                _triangles[indexOfTriangle + _triangleIndexRatio] = indexOfVertice + _triangleRightVertexNumber;
                _triangleIndexRatio++;
                _triangles[indexOfTriangle + _triangleIndexRatio] = indexOfVertice + _triangleTopVertexNumber;
                _triangleIndexRatio++;
                _triangles[indexOfTriangle + _triangleIndexRatio] = indexOfVertice + _triangleLeftVertexNumber;
                _triangleIndexRatio++;
                _triangles[indexOfTriangle + _triangleIndexRatio] = indexOfVertice + _triangleTopVertexNumber;
                _triangleIndexRatio++;
                _triangles[indexOfTriangle + _triangleIndexRatio] = indexOfVertice + _triangleRightVertexNumber;
                _triangleIndexRatio++;

                indexOfVertice += _triangleVerticeRatio;
                indexOfTriangle += _triangleIndexRatio;
            }

            AssignMesh();
        }

        public void ClearMesh()
            => _mesh?.Clear();

        private void AssignMesh()
        {
            _mesh.Clear();
            _mesh.name = Constants.DefaultMeshName;
            _mesh.vertices = _vertices;
            _mesh.triangles = _triangles;
            _meshFilter.mesh = _mesh;
        }
    }
}
