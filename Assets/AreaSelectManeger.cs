using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSelectManeger : MonoBehaviour
{
    bool MenuFlg;
    public GameObject Menu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Controler.GetMenuButtonFlg())
        {
            MenuFlg = !MenuFlg;
            Menu.SetActive(MenuFlg);
        }
    }

    public bool GetMenuFlg()
    {
        if(Menu.activeSelf== false)
        {
            MenuFlg = false;
        }
        return MenuFlg;
    }
}
