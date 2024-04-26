using TMPro;
using UnityEngine;

namespace Source.StepCounter
{
    public class StepCounterView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _stepCountText;

        public void UpdateView(int stepCount) => _stepCountText.text = stepCount.ToString();
    }
}
