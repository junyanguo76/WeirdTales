using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    // Start is called before the first frame update
    public new string name;
    public int damage;
    public int dotEffect;
    public float damageEffect;
    public int accuracyEffect;
    public int speedEffect;
    public int defendEffect;
    public bool stuckEffect;

    public bool ready;

    public void TakeEffect(Character character)
    {
        character.hP = Mathf.Clamp(character.hP + damage, 0, character.maxHp);
        if (character.hP == 0)
        {
            character.alive = false;
            character.DieTrigger();
        }
        

    }
}
