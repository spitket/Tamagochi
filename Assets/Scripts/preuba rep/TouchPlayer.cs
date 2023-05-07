using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TouchPlayer : MonoBehaviour
{
    private Tween _tween;
    [SerializeField]
    private float timeFeedback = .5f;

    [SerializeField] private int expToAddLevel = 1;

    private PlayerLevel _playerLevel;
    private void Start()
    {
        _playerLevel = GetComponent<PlayerLevel>();
        _tween = transform.DOShakeScale(timeFeedback).SetAutoKill(false).
            SetRecyclable(false).Pause();
    }

    private void OnMouseDown()
    {
        transform.localScale = new Vector3(1, 1, 1);
        _playerLevel.AddExperience(expToAddLevel);
        _tween.Restart();
    }

    public void AddExpPerClick(int expToAdd)
    {
        expToAddLevel += expToAdd;
    }
}
