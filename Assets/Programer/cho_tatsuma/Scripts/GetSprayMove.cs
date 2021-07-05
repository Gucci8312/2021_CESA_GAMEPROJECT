using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//くそ雑魚スクリプト
public class GetSprayMove : MonoBehaviour
{
    Transform targetObjPos;
    Vector3 targetUpMove;
    Vector3 m_initPos;
    Score m_scoreScript;
    public float upSpeed;
    public float speed;
    // Start is called before the first frame update
    void OnEnable()
    {
        targetObjPos = GameObject.Find("Score").GetComponent<Transform>();
        targetUpMove = this.gameObject.transform.position;
        targetUpMove.y += 30.0f;
        m_initPos = this.gameObject.transform.position;
        m_scoreScript = gameObject.GetComponent<Score>();
        //StartCoroutine("Step1_UpMove");
        StartCoroutine("Step2_TargetMove");
    }

    IEnumerator Step1_UpMove()
    {
        while (true)
        {
            if (m_scoreScript.col && gameObject.transform.position != targetUpMove)
            {
                this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, targetUpMove, upSpeed * 10f);
                if (gameObject.transform.position == targetUpMove)
                {
                    yield return new WaitForSeconds(0.2f);
                    StartCoroutine("Step2_TargetMove");
                    break;
                }
            }
            yield return null;
        }
    }

    IEnumerator Step2_TargetMove()
    {
        while (true)
        {
            if (m_scoreScript.col)
            {
                //現在位置を更新
                this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, targetObjPos.position, speed * 10f);
                if (gameObject.transform.position == targetObjPos.position)
                {
                    gameObject.SetActive(false);
                    break;
                }
            }
            yield return null;
        }
    }
}
