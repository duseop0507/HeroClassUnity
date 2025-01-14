﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NullSave.TOCK.Stats;

public class Hero : Human
{
    public int level;//Level
    public int exp;//Exp
    public string skin;//Skin
    public List<string> skinList = new List<string>();//현재 가지고 있는 SkinList
    public List<Soldier> soldierList = new List<Soldier>();//현재 가지고 있는 병사 list

    public List<HeroDB> heroDBList;//heroDB리스트
    private StatsCog statsCog;//statsCog
    private StatValue damageStat, hPStat, speedStat, damageAreaStat, moveAreaStat, levelStat;//StatValues


    private void Awake()
    {
        moveTargetDistance = transform.GetChild(0).GetComponent<SphereCollider>();//시작할때 각각의 자식 콜라이더 받아옴 공격범위와 이동범위
        attackTargetDistance = transform.GetChild(1).GetComponent<SphereCollider>();
        statsCog = GetComponent<StatsCog>();
        FindStats();
        UpdateStatus();//status업데이트
        LoadDesign();//design불러옴
        AddValueChangedToStatValue();
    }

    private void Update()
    {
        if (attackTarget.Count != 0 && !isAttack)//공격큐에 무언가가 있다면 공격함
        {
            Attack();
        }
        else if (attackTarget.Count == 0 && !isAttack)//없으면 움직임
        {
            Move();
        }
        else
            return;
    }
    #region Public_Methods
    public override void UpdateStatus()//status 업데이트 해줌
    {
        for(int i=0;i<heroDBList.Count;i++)
        {
            if(heroDBList[i].Code==this.code)
            {
                HeroDB thisHero = heroDBList[i];
                float itemHP = 0;
                float itemDmg = 0;
                float itemSpeed = 0;
                for (int j = 0; j < itemList.Count; j++)
                {
                    itemHP += itemList[j].plusHP;
                    itemDmg += itemList[j].plusDamage;
                    itemSpeed += itemList[j].plusSpeed;
                    Debug.Log("아이템");
                }
                hPStat.value = thisHero.LevelHP + (" + " + itemHP.ToString());
                damageStat.value = thisHero.LevelDamage + (" + " + itemDmg.ToString());
                damageAreaStat.value = thisHero.AttackDistance;
                moveAreaStat.value = thisHero.MoveDistance;
                speedStat.value = thisHero.LevelSpeed + (" + " + itemSpeed.ToString());
                this.design = thisHero.Character;
                levelStat.SetValue(level);
                break;
            }
        }
        speed = speedStat.CurrentValue;
        damage = damageStat.CurrentValue;
        hp = hPStat.CurrentValue;
        attackDistance = damageAreaStat.CurrentValue;
        moveDistance = moveAreaStat.CurrentValue;
        moveTargetDistance.radius = moveAreaStat.CurrentValue;
        attackTargetDistance.radius = damageAreaStat.CurrentValue;
    }

    public void ApplySkin(Skin skinName)//skin바꿔줌
    {
        skin = skinName.code;
    }
    #endregion

    #region ManagingStatValueEvents

    public void FindStats()
    {
        levelStat = statsCog.FindStat("Level");
        damageStat = statsCog.FindStat("Damage");
        hPStat = statsCog.FindStat("HP");
        speedStat = statsCog.FindStat("Speed");
        damageAreaStat = statsCog.FindStat("DamageArea");
        moveAreaStat = statsCog.FindStat("MoveArea");
    }
    public void OnLevelValueChanged(float oldValue, float newValue)
    {
        level = (int)levelStat.CurrentValue;
    }

    public void OnDamageValueChanged(float oldValue, float newValue)
    {
        damage = damageStat.CurrentValue;
    }

    public void OnDamageAreaValueChanged(float oldValue, float newValue)
    {
        attackDistance = damageAreaStat.CurrentValue;
    }

    public void OnMoveAreaChanged(float oldValue, float newValue)
    {
        moveDistance = moveAreaStat.CurrentValue;
    }

    public void OnHPlValueChanged(float oldValue, float newValue)
    {
        hp = hPStat.CurrentValue;
    }

    public void OnSpeedValueChanged(float oldValue, float newValue)
    {
        speed = speedStat.CurrentValue;
    }

    public void AddValueChangedToStatValue()
    {
        levelStat.onValueChanged.AddListener(OnLevelValueChanged);
        damageStat.onValueChanged.AddListener(OnDamageValueChanged);
        damageAreaStat.onValueChanged.AddListener(OnDamageAreaValueChanged);
        moveAreaStat.onValueChanged.AddListener(OnMoveAreaChanged);
        hPStat.onValueChanged.AddListener(OnHPlValueChanged);
        speedStat.onValueChanged.AddListener(OnSpeedValueChanged);
    }
    #endregion

}
