using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCollectingAnimForUI : MonoBehaviour
{
    RectTransform Target;
    private RectTransform Self;
    UpgradeSystem upgradeSystem;
    GameObject player;
    public int UIlerpSpeed = 8;
    VipStudentsPick[]  vipStudentsPick;
    bool isItOpen = false;

    void Start()
    {
        Self = GetComponent<RectTransform>();
        upgradeSystem = GameObject.FindObjectOfType<UpgradeSystem>();
        player = GameObject.FindWithTag("Player");
        Target = GameObject.Find("MoneyPos").GetComponent<RectTransform>();
        Vector2 UItransform = new Vector2(Camera.main.WorldToViewportPoint(player.transform.position).x, Camera.main.WorldToViewportPoint(player.transform.position).y);
        Self.localPosition = new Vector3(UItransform.x, UItransform.y, 0);
        vipStudentsPick = FindObjectsOfType<VipStudentsPick>();

    }

    void Update()
    {
        Self.localPosition = Vector3.Lerp(Self.localPosition, new Vector3(Target.localPosition.x, Target.localPosition.y, Target.localPosition.z), Time.deltaTime * UIlerpSpeed);
        if (Vector3.Distance(Self.localPosition, new Vector3(Target.localPosition.x, Target.localPosition.y, Target.localPosition.z)) < 4f)
        {
            Reached();
        }
        if(PlayerPrefs.GetInt("Map_2", 0) == 1 && !isItOpen)
        {
            vipStudentsPick = FindObjectsOfType<VipStudentsPick>();

            isItOpen = true;
        }
    }

    public void Reached()
    {

        if (vipStudentsPick[0].vipStudentCount > 0 )
        {
            var vip = PlayerPrefs.GetInt("VipMoney", upgradeSystem.VipStudentStartIncome);

            upgradeSystem.ChangeMoneyAmount(vip);
            vipStudentsPick[0].vipStudentCount--;
            Destroy(gameObject);

        }
        else if (vipStudentsPick[1]!=null && vipStudentsPick[1].vipStudentCount > 0)
        {
            var vip = PlayerPrefs.GetInt("VipMoney", upgradeSystem.VipStudentStartIncome);

            upgradeSystem.ChangeMoneyAmount(vip);
            vipStudentsPick[1].vipStudentCount--;
            Destroy(gameObject);

        }
        else {
            var std = PlayerPrefs.GetInt("StudentMoney", upgradeSystem.StudentStartIncome);

            upgradeSystem.ChangeMoneyAmount(std);

            Destroy(gameObject);
        }
    }
}
