using UnityEngine;
using UnityEngine.UI;

public class InternationalImage : MonoBehaviour
{
    [SerializeField] Texture rusSprite;
    [SerializeField] Texture enSprite;
    void Start()
    {
        if(Language.Instance.currentLanguage == "ru")
            GetComponent<RawImage>().texture = rusSprite;
        else GetComponent<RawImage>().texture = enSprite;
    }

}
