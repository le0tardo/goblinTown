using UnityEngine;

[CreateAssetMenu(
    fileName = "MacroResource",
    menuName = "Forageables/Macro Resource"
)]
public class MacroResourceCategory : ScriptableObject
{
    public enum MacroCat
    {
        Wood,
        Food,
        Stone,
        Clay
    }
    public MacroCat macroCat;
    public Sprite macroIcon;
}
