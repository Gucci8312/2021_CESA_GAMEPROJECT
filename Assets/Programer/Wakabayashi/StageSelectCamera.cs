using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectCamera : MonoBehaviour
{

    public int stagenumber = 0;

    public Camera Cam;

    public int position = 0;

    public bool plus = false;
    public bool minus = false;

    int Stop = 0;

    bool back = false;
    bool back1 = false;
    bool back2 = false;
    bool back3 = false;
    bool back4 = false;

    public int Speed = 1;

    // start is called before the first frame update
    void Start()
    {
        Cam = Camera.main;
    }

    // Update is called once per frame
    public void Update()
    {
        if (stagenumber == 0)
        {
            if (back)
            {
                position -= Speed;
                if (position <= 0)
                {
                    position = 0;
                    back1 = false;
                }
            }
            else
            {
                if (position >= 0)
                {
                    position = 0;
                    back = true;
                }
                else
                {
                    position += Speed;
                }
            }
            transform.position = new Vector3(position, Cam.transform.position.y, -11);
        }

        if (stagenumber == 1)
        {
            if (back1)
            {
                position -= Speed;
                if (position <= 50)
                {
                    position = 50;
                    back1 = false;
                }
            }
            else
            {
                if (position >= 50)
                {
                    position = 50;
                    back = true;
                }
                else
                {
                    position += Speed;
                }
            }
            transform.position = new Vector3(position, Cam.transform.position.y, -11);
        }

        if (stagenumber == 2)
        {

            if (back2)
            {
                position -= Speed;
                if (position <= 100)
                {
                    position = 100;
                    back2 = false;
                }
            }
            else
            {
                if (position >= 100)
                {
                    position = 100;
                    back1 = true;
                }
                else
                {
                    position += Speed;
                }
            }
            transform.position = new Vector3(position, Cam.transform.position.y, -11);
        }

        if (stagenumber == 3)
        {
            if (back3)
            {
                position -= Speed;
                if (position <= 150)
                {
                    position = 150;
                    back3 = false;
                }
            }
            else
            {
                if (position >= 150)
                {
                    position = 150;
                    back2 = true;
                }
                else
                {
                    position += Speed;
                }
            }
            transform.position = new Vector3(position, Cam.transform.position.y, -11);
        }

        if (stagenumber == 4)
        {
            if (back4)
            {
                position -= Speed;
                if (position <= 200)
                {
                    position = 150;
                    back4 = false;
                }
            }
            else
            {
                if (position >= 200)
                {
                    position = 200;
                    back3 = true;
                }
                else
                {
                    position += Speed;
                }
            }
            transform.position = new Vector3(position, Cam.transform.position.y, -11);
        }

        if (plus)
        {
            if (stagenumber != 4)
            {
                stagenumber++;
            }
            plus = false;
        }
        if (minus)
        {
            if (stagenumber != 0)
            {
                stagenumber--;
            }
            minus = false;
        }
    }

    public void OnPlus()
    {
        plus = true;
    }
    public void OnMinus()
    {
        minus = true;
    }

    public void StageNum0()
    {
        stagenumber = 0;
        position = 0;
    }
    public void StageNum1()
    {
        stagenumber = 1;
        position = 50;
    }
    public void StageNum2()
    {
        stagenumber = 2;
        position = 100;
    }
    public void StageNum3()
    {
        stagenumber = 3;
        position = 150;
    }
    public void StageNum4()
    {
        stagenumber = 4;
        position = 200;
    }

}
