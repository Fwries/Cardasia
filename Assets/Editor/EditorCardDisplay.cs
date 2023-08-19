using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SC_Card))]
public class EditorCardDisplay : Editor
{
    private SerializedProperty CardList;

    private void OnEnable()
    {
        CardList = serializedObject.FindProperty("CardList");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        SC_Card CardGUI = target as SC_Card;

        if (CardGUI.CardType == SC_Card.Type.Consumable || CardGUI.CardType == SC_Card.Type.Skill)
        {
            EditorGUILayout.PropertyField(CardList, true);
        }
        else if (CardGUI.CardType == SC_Card.Type.Weapon || CardGUI.CardType == SC_Card.Type.Armour)
        {
            CardGUI.CardAtk = EditorGUILayout.IntField("Card Attack", CardGUI.CardAtk);
            CardGUI.CardHp = EditorGUILayout.IntField("Card Health", CardGUI.CardHp);
        }
    }
}
