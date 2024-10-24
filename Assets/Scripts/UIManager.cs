using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
   public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowCharacterInfo()
    {

    }

}
