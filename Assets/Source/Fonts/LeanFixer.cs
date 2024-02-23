using UnityEngine;
using Agava.YandexGames;

public class LeanFixer : MonoBehaviour
{
    [SerializeField] private string _englishText;
    [SerializeField] private string _russianText;
    [SerializeField] private string _turkishText;
    [SerializeField] private string _key;

    public string GetLocalisedText()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
            _key = YandexGamesSdk.Environment.i18n.ToString();
#endif

        switch (_key)
        {
            case (Constants.En):
                return _englishText;

            case (Constants.Ru):
                return _russianText;

            case (Constants.Tr):
                return _turkishText;

            default: return _englishText;
        }
    }
}
