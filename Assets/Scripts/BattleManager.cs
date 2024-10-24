using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState {Start, NextTurn, PlayerTurn, EnemyTurn, Win, Lose }

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public BattleState state;
    private void Awake()
    {
        instance = this;
    }

    public bool madeMove;
    public List<Skill> tempSkillList;
    public Character tempCharacter;
    public Character tempInfoCharacter;

    public GameObject characterInfoBox;
    TextMeshProUGUI characterInfo;

    public List<Character> characterList;

    public Transform hero0Transform;
    public Transform hero1Transform;
    public Transform hero2Transform;
    public Transform enemy0Transform;
    public Transform enemy1Transform;
    public Transform enemy2Transform;
    public GameObject Objhero0;
    public GameObject Objenemy0;
    public GameObject Objenemy1;
    private void Start()
    {
        state = BattleState.Start;
        characterInfo = characterInfoBox.GetComponent<TextMeshProUGUI>();

        //create prefab for player and enemy

        Character pigMan = Instantiate(Objenemy0,enemy0Transform.position, Quaternion.identity).GetComponent<Character>();
        Skill Mons0SkillA = gameObject.AddComponent<Skill>();
        Mons0SkillA.name = "Bite";
        Mons0SkillA.damage = -5;
        pigMan.skillList.Add(Mons0SkillA);
        characterList.Add(pigMan);

        Character fishMan = Instantiate(Objenemy1, enemy1Transform.position, Quaternion.identity).GetComponent<Character>();
        Skill Mons1SkillA = gameObject.AddComponent<Skill>();
        Mons1SkillA.name = "Sharp";
        Mons1SkillA.damage = -5;
        fishMan.skillList.Add(Mons1SkillA);
        characterList.Add(fishMan);

        Character hero0 = Instantiate(Objhero0,hero0Transform.position,Quaternion.identity).GetComponent<Character>();
        Skill hero0skillA = gameObject.AddComponent<Skill>();
        Skill hero0skillB = gameObject.AddComponent<Skill>();
        Skill hero0skillC = gameObject.AddComponent<Skill>();
        Skill hero0skillD = gameObject.AddComponent<Skill>();
        hero0skillA.damage = -5;
        hero0skillA.name = "Cut";
        hero0skillB.damage = -10;
        hero0skillB.name = "Push";
        hero0skillC.damage = -15;
        hero0skillC.name = "Slip";
        hero0skillD.damage = -105;
        hero0skillD.name = "Dump";
        hero0.skillList.Add(hero0skillA);
        hero0.skillList.Add(hero0skillB);
        hero0.skillList.Add(hero0skillC);
        hero0.skillList.Add(hero0skillD);
        characterList.Add(hero0);

        //根据速度进行排序
        state = BattleState.NextTurn;
        StartCoroutine(NextTurn());


    }
    
    public IEnumerator NextTurn()
    {
        if (state == BattleState.Win || state == BattleState.Lose) yield break;
        CharacterSort();
        state = BattleState.NextTurn;
        yield return new WaitForSeconds(1);
        switch (tempCharacter.identity)
        {
            case 0:
               state = BattleState.PlayerTurn;
                break;
            case 1:
                state = BattleState.EnemyTurn;
                StartCoroutine(EnemyTurn());
                break;
        }
    }


    public void CharacterSort()
    {
        List<Character> tempList = new List<Character>();
        tempList = characterList.FindAll(c => c.hasMoved == false);
        if (tempList.Count == 0)//所有人都动过了就重置
        { 
            foreach(Character character in characterList.ToList())
            {
                character.hasMoved = false;
            }
            tempList = characterList;
            Debug.Log(characterList.Count);
        }
        //Character speed sorting
        if (tempList.Count > 1)
        {
            foreach (Character character in tempList.ToList())
            {
                for (int i = 0; i < tempList.Count - 1; i++)
                {
                    Character temp = tempList[i];
                    if (tempList[i].speed < tempList[i + 1].speed)
                    {
                        tempList[i] = tempList[i + 1];
                        tempList[i + 1] = temp;
                    }
                }
            }
        }
        tempCharacter = tempList[0];//配制好当前角色
        
        tempSkillList = tempList[0].skillList;
    } 

    IEnumerator HeroTurn()
    {
        //播英雄动作动画
        yield return new WaitForSeconds(1);
        BattleEndJudge();
        StartCoroutine(NextTurn());
    }
    public void StartHeroTurn()
    {
        StartCoroutine(HeroTurn());   
    }


    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(3);
        List<Character> herolist = characterList.FindAll(c => c.identity == 0);
        int h = Random.Range(0, herolist.Count);
        tempInfoCharacter = herolist[h];
        int i = Random.Range(0, tempSkillList.Count);
        tempSkillList[i].TakeEffect(tempInfoCharacter);
        tempCharacter.hasMoved = true;
        ShowCharacterInfo();
        BattleEndJudge();
        StartCoroutine(NextTurn());
    }
    public void BattleEndJudge()
    {
       characterList.Remove(characterList.Find(c => c.alive == false));

        List<Character> tempHeroList = new List<Character>();
        tempHeroList.AddRange(characterList.FindAll(character =>
        {
            if (character.identity == 0)
            {
                return true;
            }
            else { return false; }
        }));

        List<Character> tempEnemyList = new List<Character>();
        tempEnemyList.AddRange(characterList.FindAll(character =>
        {
            if (character.identity == 1)
            { return true; }
            else { return false; }
        }));

        bool allHeroDie = tempHeroList.TrueForAll(character =>
        { 
                if(character.alive == false)
                {
                    return true;
                }
                else { return false; }
        });
        bool allEnemyDie = tempEnemyList.TrueForAll(character =>
        {
            if(!character.alive) { return true; }
            else { return false; }
        });

        if(allHeroDie )
        {
            state = BattleState.Lose;
            Debug.Log("You lose");
        }
        else if(allEnemyDie )
        {
            state = BattleState.Win;
            Debug.Log("You win");
        }
       
    }
    public void ResetSkill()
    {
       foreach(Skill skill in tempSkillList)
        {
            skill.ready = false;
        }
    }
    public void UseSkillA()
    {
        ResetSkill();
        tempSkillList[0].ready = true;
        Debug.Log("SkillA is ready");
    }
    public void UseSkillB()
    {
        ResetSkill();
        tempSkillList[1].ready = true;
        Debug.Log("SkillB is ready");
    }

    public void UseSkillC()
    {
        ResetSkill();
        tempSkillList[2].ready = true;
        Debug.Log("SkillC is ready");
    }
    public void UseSkillD()
    {
        ResetSkill();
        tempSkillList[3].ready = true;
        Debug.Log("SkillD is ready");
    }// Update is called once per frame


    public void ShowCharacterInfo()
    {
        characterInfo.text = tempInfoCharacter.name + "\r\n"
                    + "Hp  " + tempInfoCharacter.hP + "\r\nSpeed   " + tempInfoCharacter.speed
                    ;
    }
}
