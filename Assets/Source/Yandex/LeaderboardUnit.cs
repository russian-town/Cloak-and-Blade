using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Source.Yandex
{
    public class LeaderBoardUnit : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _avatars;
        [SerializeField] private Image _avatar;
        [SerializeField] private TMP_Text _rank;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _score;

        public Image Avatar => _avatar;

        public void SetValues(Image avatar, float rank, string name, float score)
        {
            _rank.text = rank.ToString();
            _avatar = avatar;
            _name.text = name;
            _score.text = score.ToString();
        }

        public void SetProfileImage(string imageUrl) => StartCoroutine(SetProfileImageCoroutine(imageUrl));

        public void SetDefaultProfilePicture() => _avatar.sprite = SetRandomProfileImage();

        private Sprite SetRandomProfileImage()
        {
            int randomAvatarIndex = 0;
            randomAvatarIndex = Random.Range(0, _avatars.Count);

            for (int i = 0; i < _avatars.Count; i++)
            {
                if (i == randomAvatarIndex)
                    return _avatars[i];
            }

            return null;
        }

        private IEnumerator SetProfileImageCoroutine(string url)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                yield break;

            var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            _avatar.sprite = sprite;
        }
    }
}
