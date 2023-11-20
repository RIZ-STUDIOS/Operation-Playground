using OperationPlayground.ScriptableObjects;
using OperationPlayground.ScriptableObjects.Projectiles;
using RicTools;
using RicTools.Editor.Utilities;
using RicTools.Editor.Windows;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OperationPlayground.Editor.Windows
{
    public class WeaponEditorWindow : GenericEditorWindow<WeaponScriptableObject, AvailableWeaponsScriptableObject>
    {
        public EditorContainer<GameObject> prefab = new EditorContainer<GameObject>();
        public EditorContainer<float> cooldown = new EditorContainer<float>(1);
        public EditorContainer<int> maxAmmo = new EditorContainer<int>(1);
        public EditorContainer<ProjectileScriptableObject> projectileSo;
        public EditorContainer<Vector3> slotOffset;
        public EditorContainer<Sprite> weaponSprite = new EditorContainer<Sprite>();

        [MenuItem("Operation Playground/Weapon Editor")]
        public static WeaponEditorWindow ShowWindow()
        {
            return GetWindow<WeaponEditorWindow>("Weapon Editor");
        }

        protected override void DrawGUI()
        {
            {
                var element = rootVisualElement.AddObjectField(prefab, "Prefab");
                RegisterLoadChange(element, prefab);
                RegisterCheckCompletion(element);
            }

            {
                var element = rootVisualElement.AddObjectField(weaponSprite, "Sprite");
                RegisterLoadChange(element, weaponSprite);
                RegisterCheckCompletion(element);
            }

            {
                var element = rootVisualElement.AddIntField(maxAmmo, "Max Ammo");
                RegisterLoadChange(element, maxAmmo);
                RegisterCheckCompletion(element);
            }

            {
                var element = rootVisualElement.AddFloatField(cooldown, "Cooldown");
                RegisterLoadChange(element, cooldown);
                RegisterCheckCompletion(element);
            }

            {
                var element = rootVisualElement.AddObjectField(projectileSo, "Projectile");
                RegisterLoadChange(element, projectileSo);
                RegisterCheckCompletion(element);
            }

            {
                var element = rootVisualElement.AddVector3Field(slotOffset, "Slot Offset");
                RegisterLoadChange(element, slotOffset);
            }
        }

        protected override void LoadScriptableObject(WeaponScriptableObject so, bool isNull)
        {
            if (isNull)
            {
                cooldown.Reset();
                prefab.Reset();
                projectileSo.Reset();
                slotOffset.Reset();
                maxAmmo.Reset();
                weaponSprite.Reset();
            }
            else
            {
                cooldown.Value = so.cooldown;
                prefab.Value = so.prefab;
                projectileSo.Value = so.projectileScriptableObject;
                slotOffset.Value = so.slotOffset;
                maxAmmo.Value = so.maxAmmo;
                weaponSprite.Value = so.weaponSprite;
            }
        }

        protected override void CreateAsset(ref WeaponScriptableObject asset)
        {
            asset.cooldown = cooldown;
            asset.prefab = prefab;
            asset.projectileScriptableObject = projectileSo;
            asset.slotOffset = slotOffset;
            asset.maxAmmo = maxAmmo;
            asset.weaponSprite = weaponSprite;
        }

        protected override IEnumerable<CompleteCriteria> GetCompleteCriteria()
        {
            yield return new CompleteCriteria(!prefab.IsNull(), "Need prefab");
            yield return new CompleteCriteria(cooldown > 0, "Cooldown must be higher than 0");
            yield return new CompleteCriteria(!projectileSo.IsNull(), "Need Projectile");
        }
    }
}