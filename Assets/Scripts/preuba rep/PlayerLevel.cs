using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerLevel : MonoBehaviour
{
    [SerializeField] private int level = 1;

    [SerializeField] private int exp = 0;

    [SerializeField] private int nextToLv = 10;

    [SerializeField] private TextMeshProUGUI levelInfo;
    [SerializeField] private Image bar;
    [SerializeField] private GameObject infoExp;
    [SerializeField] private Transform spawnPoint;
    public void AddExperience(int expAdd)
    {
        exp += expAdd;
        GameObject textInfo = Instantiate(infoExp, spawnPoint);
        textInfo.transform.localPosition = new Vector3(Random.Range(-2, 2), 0, 0);
        textInfo.
            GetComponentInChildren<TextMeshProUGUI>().text = "+" + expAdd;
        textInfo.transform.
            DOLocalMoveY(transform.position.y + 3, 0.2f);
        Destroy(textInfo, 0.2f);
        if (exp >= nextToLv)
        {
            exp -= nextToLv;
            LevelUp();
        }
        UpdateInfo();
    }

    private void LevelUp()
    {
        level++;
        nextToLv = (int)(nextToLv * 1.25f + nextToLv);
    }

    public void UpdateInfo()
    {
        levelInfo.text = "Level: " + level;
        bar.fillAmount = (float)exp / (float)nextToLv;
    }

    private void Start()
    {
        UpdateInfo();
    }
}
