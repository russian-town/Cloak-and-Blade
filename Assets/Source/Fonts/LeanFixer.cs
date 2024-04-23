using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

public class LeanFixer : MonoBehaviour
{
    [SerializeField] private Text _englishText;
    [SerializeField] private Text _russianText;
    [SerializeField] private Text _turkishText;
    [SerializeField] private string _key;

    public string GetLocalisedText()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
            _key = YandexGamesSdk.Environment.i18n.lang;
#endif

        switch (_key)
        {
            case (Constants.En):
                return _englishText.text;

            case (Constants.Ru):
                return _russianText.text;

            case (Constants.Tr):
                return _turkishText.text;

            default: return _englishText.text;
        }
    }
}
