using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BattleManager;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    GameObject character;
    // Start is called before the first frame update
    public int id;
    public new string name;
    public int hP;
    public int maxHp;
    public bool alive;
    public int accuracy;
    public int speed;
    public int defend;
    public bool stuck;
    public List<Skill> skillList;

    public bool hasMoved;
    public int identity;//0 is hero 1 is enemy

    public void Start()
    {
        maxHp = hP;
    }
    public void OnClick()
    {


            BattleManager.instance.tempInfoCharacter = this;
            BattleManager.instance.ShowCharacterInfo();
        
        if (BattleManager.instance.state != BattleState.PlayerTurn)
        {
            return;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Skill readySkill = BattleManager.instance.tempSkillList.Find(skill => { if (skill.ready) return true; return false; });
          if(readySkill.ready == true ) { 
            readySkill.TakeEffect(this);
            BattleManager.instance.tempCharacter.hasMoved = true;
            BattleManager.instance.ResetSkill();
            BattleManager.instance.StartHeroTurn();
        }
    
}
        BattleManager.instance.ShowCharacterInfo();
        
    }

    public void DieTrigger()
    {
        StartCoroutine(Die());
    }
    public IEnumerator Die()
    {
        yield return new  WaitForSeconds(1);
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        Color tempColor = new Color();
        tempColor.a = 1;
        for(float i = 1; i > 0; )
        {
            i -= Time.deltaTime * 0.1f;
            tempColor.a = i;
            spriteRenderer.color = tempColor;
        }
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}

