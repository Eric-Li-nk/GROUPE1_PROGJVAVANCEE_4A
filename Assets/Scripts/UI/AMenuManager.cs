using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AMenuManager : MonoBehaviour
{

    public void SwithMenu(GameObject menu)
    {
        gameObject.SetActive(false);
        menu.SetActive(true);
    }
    
}
