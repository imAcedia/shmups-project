using UnityEngine;

namespace Shmup
{
    [CreateAssetMenu(menuName = "Enemy Data/Simple", order = AssetMenu.order)]
    public class SimpleEnemyData : ScriptableObject
    {
        public RuntimeAnimatorController controller;
        public Weapon weapon;
    }
}
