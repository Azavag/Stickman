using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameController gameController;
    [SerializeField] PlayerMovement playerMovement;
    public void OnPointerDown(PointerEventData eventData)
    {
        gameController.StartLevel();
        playerMovement.SetActiveJoystick(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        playerMovement.SetActiveJoystick(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            gameController.StartLevel();
    }
}
