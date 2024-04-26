using UnityEngine;
using UnityEngine.Splines;

namespace Source.Models
{
    public class MoveAlongSpline : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private SplineContainer _spline;
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private bool _playOnAwake;

        private float _currentDistance = 0f;

        public float Speed => _speed;

        private void Update()
        {
            if (_playOnAwake)
            {
                Vector3 targetPosition = _spline.EvaluatePosition(_currentDistance);

                transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

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
                    float movement = _speed * Time.deltaTime / splineLength;
                    _currentDistance += movement;
                }
            }
        }

        public void ChangeSlineSpeed(float value, float duration, float initialSpeed)
            => _speed = Mathf.MoveTowards(_speed, value, duration * initialSpeed * Time.deltaTime);

        public void Play()
            => _playOnAwake = true;
    }
}
