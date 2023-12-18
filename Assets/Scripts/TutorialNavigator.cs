using UnityEngine;
using UnityEngine.UI;

public class TutorialNavigator : MonoBehaviour
{
    [SerializeField] Transform[] pages;
    int pageCounter = 0;

    [SerializeField] GameObject NextButton;
    [SerializeField] GameObject CloseButton;
    [SerializeField] SoundController soundController;
    void Start()
    {
        pages[0].gameObject.SetActive(true);
        CloseButton.gameObject.SetActive(false);
    }

   
    //По кнопке
    public void NextPage()
    {
        pageCounter++;
        if(pageCounter < pages.Length)
        {
            pages[pageCounter].gameObject.SetActive(true);
            pages[pageCounter-1].gameObject.SetActive(false);
        }
        if(pageCounter == pages.Length - 1)
        {
            NextButton.SetActive(false);
            CloseButton.SetActive(true);
        }
        soundController.MakeClickSound();
    }

    public void CloseTutorial()
    {
        soundController.MakeClickSound();
        foreach(Transform page in pages)
            page.gameObject.SetActive(false);
        pageCounter = 0;
        pages[pageCounter].gameObject.SetActive(true);
        NextButton.SetActive(true);
        CloseButton.SetActive(false);
    }
}
