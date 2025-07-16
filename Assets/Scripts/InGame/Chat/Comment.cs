using UnityEngine;
using UnityEngine.UI;

public class Comment : MonoBehaviour
{
    [SerializeField] Text _userNameText;
    [SerializeField] Text _commentText;
    [SerializeField] Image _image;
    public void Initialize(string u,string c, Color imageColor)
    {
        _userNameText.text = u;
        _commentText.text = c;
        _image.color = imageColor;
    }
}
