using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    [SerializeField] private GameObject currentOpenTab;
    [SerializeField] private Button pressedButton;

    public void SwitchTab(Tab tab)
    {
        // Si c'est le même panel on fait rien, normalement cela n'arrive jamais car le bouton est désactivé
        if (tab.panel == currentOpenTab)
            return;
        // On ferme le panel qui est active
        currentOpenTab.SetActive(false);
        // On réactive le bouton qui active le panel précedent
        pressedButton.interactable = true;
        // On affiche le nouveau panel
        tab.panel.SetActive(true);
        // On désactive le bouton du nouveau panel
        tab.button.interactable = false;
        // On met les nouveaux variables en stock
        currentOpenTab = tab.panel;
        pressedButton = tab.button;
    }
}
