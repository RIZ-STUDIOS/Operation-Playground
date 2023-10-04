using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RicTools;
using RicTools.Editor.Windows;
using UnityEditor;

namespace OperationPlayground
{
    public class EnemyRoundEditorWindow : GenericEditorWindow<EnemyRoundScriptableObject, AvailableEnemyRoundsScriptableObject>
    {
        [MenuItem("Window/RicTools Windows/EnemyRoundEditorWindow")]
    	public static EnemyRoundEditorWindow ShowWindow()
        {
            return GetWindow<EnemyRoundEditorWindow>("EnemyRoundEditorWindow");
        }

        protected override void DrawGUI()
        {
        
        }

        protected override void LoadScriptableObject(EnemyRoundScriptableObject so, bool isNull)
        {
        
        }

        protected override void CreateAsset(ref EnemyRoundScriptableObject asset)
        {
        
        }
    }
}