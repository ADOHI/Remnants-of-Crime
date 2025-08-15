using UnityEngine;

public class DataManager : MonoBehaviour
{
    public CharacterData[] characters;
    public EvidenceData[] evidences;
    public TestimonyData[] testimonies;

    public CharacterData GetCharacterData(string id)
    {
        foreach (var c in characters)
        {
            if (c.characterID == id) return c;
        }
        return null;
    }
}