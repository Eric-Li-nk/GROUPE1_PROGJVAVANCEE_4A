using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IaController : MonoBehaviour
{
    private int randAct;
    [SerializeField] private GameObject placeBomb;
    [SerializeField] private GameObject Bomb;
    
    void Update()
    {
        randAct = Random.Range(0, 5);
        switch (randAct)
        {
            case 0: 
                this.transform.position += new Vector3(-0.55f, 0f, 0f);
                break;
            
            case 1:
                this.transform.position += new Vector3(0.55f, 0f, 0f);
                break;
            case 2:
                this.transform.position += new Vector3(0f, 0f, 0.55f);
                break;
            case 3:
                this.transform.position += new Vector3(0f, 0f, -0.55f);
                break;
            case 4:
                GameObject newBomb = Instantiate(Bomb);
                newBomb.transform.position = placeBomb.transform.position;
                Destroy(newBomb,2.6f);
                break;
                
        }
    }
}
