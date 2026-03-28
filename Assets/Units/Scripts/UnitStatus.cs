using System.Text;
using UnityEngine;


public class UnitStatus : MonoBehaviour
{
    public enum CauseOfDeath
    {
        None,
        Cold,
        Hunger,
        Enemy,
        Animal,
        Tree
    }

    private CauseOfDeath causeOfDeath;

    public int hp=10;
    public int maxHp=10;
    public bool isDead=false;

    public bool warm=false;

    public string unitName = "Goblin";
    #region //syllables
    string[] syllables =
{
        "ba", "bo", "boo", "bu", "bi","da", "do", "di", "di","fa","no","bi","gu",
        "fo","foo", "fi", "fe","ga", "go", "gi","la", "li", "lo","ma", "mi", "mo",
        "na", "ni", "no","pa", "po", "pi","ra", "ri", "ro", "ti","di","ke","cho",
        "ta", "ti", "to","vi", "vo","chu", "bu", "ly", "ni", "fi","ke","ki"
    };
    #endregion

    UnitAnimation anim;
    Unit unit;

    private void Start()
    {
        SetName();
        anim=GetComponent<UnitAnimation>();
        unit=GetComponent<Unit>();

        EventLogManager.inst.AddToLog(unitName+" was born!");
    }

    public void TakeDamage(int damage)
    {
        anim.HurtAnim();
        hp-=damage;
    }

    public void TakeDamageFromSource(int damage, CauseOfDeath cause)
    {
        anim.HurtAnim();
        hp-=damage;
        
        if (hp <= 0 && !isDead)
        {
            anim.DeathAnimation();
            unit.Die();
            isDead = true;
            //print(unitName + " has died of "+ cause);
            EventLogManager.inst.AddToLog(unitName + " has died of " + cause+".");
        }

        if (cause == CauseOfDeath.Cold)
        {
            anim.ColdAnim();
        }
        if (cause == CauseOfDeath.Enemy)
        {
            if (hp > 0)
            {
                unit.FightOrFLight();
            }
        }

    }

    void SetName()
    {
        int syllableCount = 2;// Random.Range(2, 4); // 2–3 syllables
        StringBuilder name = new StringBuilder();

        for (int i = 0; i < syllableCount; i++)
        {
            string syllable = syllables[Random.Range(0, syllables.Length)];
            name.Append(syllable);
        }

        // Capitalize first letter
        name[0] = char.ToUpper(name[0]);
        unitName = name.ToString();
    }
}
