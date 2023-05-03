using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lofelt.NiceVibrations;

public class GemCollectingAnimForUI : MonoBehaviour
{
    RectTransform Target;
    private RectTransform Self;
    UpgradeSystem inventory;
    GameObject player;
    CardMenu cardMenu;
    public int UIlerpSpeed = 8;

    void Start()
    {
        FindObjectOfType<AudioPlayer>().PlayGemSound();
        if (GameManager.hapticsSupported)
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
        }
        Self = GetComponent<RectTransform>();
        inventory = GameObject.FindObjectOfType<UpgradeSystem>();
        cardMenu = GameObject.FindObjectOfType<CardMenu>();
        player = GameObject.FindWithTag("Player");
        Target = GameObject.Find("GemPos").GetComponent<RectTransform>();
        Vector2 UItransform = new Vector2(Camera.main.WorldToViewportPoint(player.transform.position).x, Camera.main.WorldToViewportPoint(player.transform.position).y);
        Self.localPosition = new Vector3(UItransform.x, UItransform.y, 0);

    }

    void Update()
    {
        Self.localPosition = Vector3.Lerp(Self.localPosition, new Vector3(Target.localPosition.x, Target.localPosition.y, Target.localPosition.z), Time.deltaTime * UIlerpSpeed);
        if (Vector3.Distance(Self.localPosition, new Vector3(Target.localPosition.x, Target.localPosition.y, Target.localPosition.z)) < 4f)
        {
            Reached();
        }
    }

    public void Reached()
    {
        inventory.ChangeGemAmount(PlayerPrefs.GetInt("RoadGemAmount", cardMenu.BaslangicYolGemMiktari));
        Destroy(gameObject);
    }
}
