using System.Text;
using UnityEngine;

public class UnitStatus : MonoBehaviour
{
    public int hp=10;
    public int maxHp=10;

    public string unitName = "Goblin";
    #region //syllables
    string[] syllables =
{
        "ba", "bo", "bu", "bi","da", "do", "di", "di","fa","no","bi","gu",
        "fo", "fi", "fe","ga", "go", "gi","la", "li", "lo","ma", "mi", "mo",
        "na", "ni", "no","pa", "po", "pi","ra", "ri", "ro", "ti","di","ke",
        "ta", "ti", "to","vi", "vo","chu", "bu", "ly", "ni", "fi","ke","ki"
    };
    #endregion


    private void Start()
    {
        SetName();
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
