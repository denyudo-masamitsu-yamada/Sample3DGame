using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// キャラクターのHPバー表示
/// </summary>
public class UICharaHpBar : MonoBehaviour
{
    class HpBarData
    {
        public Character ownerChara = null;
        public RectTransform rectTrans = null;
        public Image barImage = null;
    }

    readonly Vector3 DisablePos = Vector3.one * 1000.0f;

    [SerializeField]
    GameObject baseObj = null;

    [SerializeField]
    int poolCount = 20;

    [SerializeField]
    Vector3 offsetPos = new Vector3(0.0f, 0.3f, 0.0f);

    [SerializeField]
    Sprite playerBarSprite = null;

    [SerializeField]
    Sprite enemyBarSprite = null;

    List<HpBarData> hpBarDatas = new List<HpBarData>();

    void Awake()
    {
        for (int i = 0; i < poolCount; i++)
        {
            HpBarData data = new HpBarData();
            GameObject instance = Instantiate(baseObj, transform);
            data.rectTrans = instance.GetComponent<RectTransform>();
            data.barImage = instance.transform.Find("Image_Bar").GetComponent<Image>();

            data.rectTrans.position = DisablePos;
            data.ownerChara = null;

            hpBarDatas.Add(data);
        }

        // ベースオブジェクトは使わないので、非表示
        baseObj.SetActive(false);
    }

    void Update()
    {
        for (int i = 0; i < hpBarDatas.Count; i++)
        {
            HpBarData data = hpBarDatas[i];
            if (data.ownerChara == null)
            {
                continue;
            }

            // カメラの方向に向く（ビルボード）
            data.rectTrans.LookAt(Camera.main.transform);

            // Z軸を180度にすると、表示が正常になる
            Vector3 angle = data.rectTrans.localEulerAngles;
            angle.z = 180.0f;
            data.rectTrans.localEulerAngles = angle;

            // キャラの頭上に表示する
            data.rectTrans.position = data.ownerChara.GetHeadBoneTransform().position + offsetPos;

            // ゲージ処理
            data.barImage.fillAmount = (float)data.ownerChara.CurrentHP / data.ownerChara.GetStatus().HP;
        }
    }

    /// <summary>
    /// 登録
    /// </summary>
    /// <param name="chara"></param>
    public void Register(Character chara)
    {
        for (int i = 0; i < hpBarDatas.Count; i++)
        {
            if (hpBarDatas[i].ownerChara == null)
            {
                hpBarDatas[i].ownerChara = chara;

                // プレイヤーとエネミーのSpriteを変更する（見た目を変えたいので）
                if (chara.CharaType == CharaType.Player)
                {
                    hpBarDatas[i].barImage.sprite = playerBarSprite;
                }
                else if (chara.CharaType == CharaType.Enemy)
                {
                    hpBarDatas[i].barImage.sprite = enemyBarSprite;
                }
                return;
            }
        }
    }

    /// <summary>
    /// 解除
    /// </summary>
    /// <param name="chara"></param>
    public void Unregister(Character chara)
    {
        for (int i = 0; i < hpBarDatas.Count; i++)
        {
            if (hpBarDatas[i].ownerChara == chara)
            {
                hpBarDatas[i].ownerChara = null;
                hpBarDatas[i].rectTrans.position = DisablePos;
                return;
            }
        }
    }
}
