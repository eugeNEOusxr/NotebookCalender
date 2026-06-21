using UnityEngine;

namespace WorldWeaver.Core
{
    /// <summary>
    /// Expansion hook: user-owned neighborhoods, public districts, shared spaces.
    /// Single-player prototype uses the default district only.
    /// </summary>
    [CreateAssetMenu(fileName = "DistrictContext", menuName = "WorldWeaver/District Context")]
    public class DistrictContext : ScriptableObject
    {
        [Header("Identity (future)")]
        public string districtId = "inkling-lane-default";
        public string ownerUserId = "";
        public DistrictVisibility visibility = DistrictVisibility.LocalPrototype;

        [Header("Runtime")]
        public string districtDisplayName = "Inkling Lane";
        public TextAsset catalogJson;
    }

    public enum DistrictVisibility
    {
        LocalPrototype,
        Private,
        Public,
        SharedSpace
    }
}
