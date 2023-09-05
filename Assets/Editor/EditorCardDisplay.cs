using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SC_Card))]
public class EditorCardDisplay : Editor
{
    private SerializedProperty CardTrait, CardRariety, CardClass,
        CardManaType, DoesTarget, CardTarget, CardSkill, CardList,
        StrongAgainst, IsDragable,
        CardCost, GoldCost;

    private void OnEnable()
    {
        CardManaType = serializedObject.FindProperty("CardManaType");
        CardRariety = serializedObject.FindProperty("CardRariety");
        CardClass = serializedObject.FindProperty("CardClass");

        DoesTarget = serializedObject.FindProperty("DoesTarget");
        CardTarget = serializedObject.FindProperty("CardTarget");
        CardTrait = serializedObject.FindProperty("CardTrait");
        CardSkill = serializedObject.FindProperty("CardSkill");
        CardList = serializedObject.FindProperty("CardList");

        StrongAgainst = serializedObject.FindProperty("StrongAgainst");
        IsDragable = serializedObject.FindProperty("IsDragable");

        CardCost = serializedObject.FindProperty("CardCost");
        GoldCost = serializedObject.FindProperty("GoldCost");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SC_Card CardGUI = target as SC_Card;

        if (CardGUI.CardType == SC_Card.Type.Consumable || CardGUI.CardType == SC_Card.Type.Skill)
        {
            EditorGUILayout.PropertyField(CardRariety, true);
            EditorGUILayout.PropertyField(CardClass, true);
            EditorGUILayout.PropertyField(CardTrait, true);

            EditorGUILayout.PropertyField(CardManaType, true);
            EditorGUILayout.PropertyField(DoesTarget, true);
            EditorGUILayout.PropertyField(CardTarget, true);

            EditorGUILayout.PropertyField(CardSkill, true);
            EditorGUILayout.PropertyField(CardList, true);
        }
        else if (CardGUI.CardType == SC_Card.Type.Weapon)
        {
            EditorGUILayout.PropertyField(CardRariety, true);
            EditorGUILayout.PropertyField(CardClass, true);
            EditorGUILayout.PropertyField(CardTrait, true);

            EditorGUILayout.PropertyField(StrongAgainst, true);
            EditorGUILayout.PropertyField(IsDragable, true);

            if (CardGUI.IsDragable == true)
            {
                EditorGUILayout.PropertyField(CardManaType, true);
                EditorGUILayout.PropertyField(DoesTarget, true);
                EditorGUILayout.PropertyField(CardTarget, true);
                EditorGUILayout.PropertyField(CardList, true);
            }
            else
            {
                CardGUI.CardAtk = EditorGUILayout.IntField("Card Attack", CardGUI.CardAtk);
                CardGUI.CardHp = EditorGUILayout.IntField("Card Health", CardGUI.CardHp);
            }

            EditorGUILayout.PropertyField(CardSkill, true);
        }

        EditorGUILayout.PropertyField(CardCost, true);
        EditorGUILayout.PropertyField(GoldCost, true);

        serializedObject.ApplyModifiedProperties();
    }
}
