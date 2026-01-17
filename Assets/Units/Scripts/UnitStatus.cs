using System.Text;
using UnityEngine;


public class UnitStatus : MonoBehaviour
{
    public int hp=10;
    public int maxHp=10;
    public bool isDead=false;

    public float hungry=1f; //0=starving
    public float cold=1f; //0=freezing


    public string unitName = "Goblin";
    #region //syllables
    string[] syllables =
{
        "ba", "bo", "boo", "bu", "bi","da", "do","doo", "di", "di","fa","no","noo","bi","gu",
        "fo","foo", "fi", "fe","ga", "go","goo", "gi","la", "li", "lo","ma", "mi", "mo","moo",
        "na", "ni", "no","pa", "po", "pi","ra", "ri", "ro","roo", "ti","di","ke","too","choo",
        "ta", "ti", "to","vi", "vo","chu", "bu", "ly", "ni", "fi","ke","ki","koo"
    };
    #endregion

    UnitAnimation anim;
    Unit unit;

    private void Start()
    {
        SetName();
        anim=GetComponent<UnitAnimation>();
        unit=GetComponent<Unit>();
    }

    private void Update()
    {
        if (hp <= 0 && !isDead)
        {
            anim.DeathAnimation();
            unit.Die();
            isDead = true;
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
