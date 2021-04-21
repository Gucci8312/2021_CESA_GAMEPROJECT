// @file   SingletonMonobehaviour.cs
// @brief  Unity用シングルトンテンプレートクラスの定義
// @author T.Cho
// @date   2021/04/19 作成
using UnityEngine;
using System;

// @name   SingletonMonobehaviour
// @brief  Unity用シングルトンテンプレート
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Type t = typeof(T);
                instance = (T)FindObjectOfType(t);
                if (instance == null)
                {
                    // シングルトンのスクリプトが存在しないとき、そのスクリプトがアタッチされているオブジェクトを生成する
                    GameObject singletonObject = Resources.Load<GameObject>("Managers/"+ t.ToString());
                    GameObject obj = Instantiate(singletonObject, Vector3.zero, Quaternion.identity);
                    instance = obj.GetComponent<T>();
                }
            }

            return instance;
        }
    }

    virtual protected void Awake()
    {
        // 他のゲームオブジェクトにアタッチされているか調べる
        // アタッチされている場合は破棄する。
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        if (instance == null)
        {
            instance = this as T;
            return true;
        }
        else if (Instance == this)
        {
            return true;
        }
        Destroy(this);
        return false;
    }
}