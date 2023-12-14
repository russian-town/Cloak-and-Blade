using UnityEngine;
using UnityEngine.Splines;

public class MoveAlongSpline : MonoBehaviour
{
    public float Speed = 1f;
    [SerializeField] private SplineContainer _spline;
    [SerializeField] private float _rotationSpeed = 5f;

    private float _currentDistance = 0f;

    void Update()
    {
        Vector3 targetPosition = _spline.EvaluatePosition(_currentDistance);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);

        Vector3 targetDirection = _spline.EvaluateTangent(_currentDistance);

        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        if (_currentDistance >= 1f)
        {
            _currentDistance = 0f;
        }
        else
        {
            float splineLength = _spline.CalculateLength();
            float movement = Speed * Time.deltaTime / splineLength;
            _currentDistance += movement;
        }
    }
}
