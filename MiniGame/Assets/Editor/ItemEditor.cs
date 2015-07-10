using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Item))] //This tells the script what type of script it is modifying
public class ItemEditor : Editor {

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        /// <summary>
        /// Draws the default inspector of the Item script
        /// </summary>
        DrawDefaultInspector();

        /// <summary>
        /// This grabs the Item script that we will be modifying or mapping values to from out custom inspector
        /// </summary>
        Item myItem = (Item)target; 

        switch(myItem.Type)
        {
            case Item.ItemType.Ammo:
                break;
            case Item.ItemType.OneShot:
                //EditorGUILayout.ObjectField("FireTip", null, typeof(Transform));
                break;
            case Item.ItemType.Melee:
                break;
            case Item.ItemType.Special:
                break;
        }
        //myItem.Damage= EditorGUILayout.IntField("Experience", myItem.Damage);
    }
}
