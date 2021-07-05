using Cinemachine;
using UnityEngine;

public class DollyDriver : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    // [SerializeField] private float cycleTime = 10.0f;

    private CinemachineTrackedDolly dolly;
    private float pathPositionMax;
    private float pathPositionMin;


    public int stagenumber = 0;
    public bool plus = false;
    public bool minus = false;



    private void Start()
    {
        // バーチャルカメラがセットされていなければ中止
        if (this.virtualCamera == null)
        {
            this.enabled = false;
            return;
        }

        // ドリーコンポーネントを取得できなければ中止
        this.dolly = this.virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        if (this.dolly == null)
        {
            this.enabled = false;
            return;
        }

        // Positionの単位をトラック上のウェイポイント番号基準にするよう設定
        this.dolly.m_PositionUnits = CinemachinePathBase.PositionUnits.PathUnits;

        // ウェイポイントの最大番号・最小番号を取得
        this.pathPositionMax = this.dolly.m_Path.MaxPos;
        this.pathPositionMin = this.dolly.m_Path.MinPos;

    }

    private void Update()
    {
        // cycleTime秒かけてトラック上を往復させる
        //var t = 0.5f - (0.5f * Mathf.Cos((Time.time * 2.0f * Mathf.PI) / this.cycleTime));
        //this.dolly.m_PathPosition = Mathf.Lerp(this.pathPositionMin, this.pathPositionMax, t);
        if (stagenumber == 0)
        {
            this.dolly.m_PathPosition = this.pathPositionMin;
        }

        if (stagenumber == 1)
        {
            this.dolly.m_PathPosition = this.pathPositionMax - 3;
        }

        if (stagenumber == 2)
        {
            this.dolly.m_PathPosition = this.pathPositionMax - 2;
        }

        if (stagenumber == 3)
        {
            this.dolly.m_PathPosition = this.pathPositionMax - 1;
        }

        if (stagenumber == 4)
        {
            this.dolly.m_PathPosition = this.pathPositionMax;
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
        this.dolly.m_PathPosition = this.pathPositionMin;
    }
    public void StageNum1()
    {
        stagenumber = 1;
        this.dolly.m_PathPosition = this.pathPositionMax - 3;
    }
    public void StageNum2()
    {
        stagenumber = 2;
        this.dolly.m_PathPosition = this.pathPositionMax - 2;
    }
    public void StageNum3()
    {
        stagenumber = 3;
        this.dolly.m_PathPosition = this.pathPositionMax - 1;
    }
    public void StageNum4()
    {
        stagenumber = 4;
        this.dolly.m_PathPosition = this.pathPositionMax;
    }
}