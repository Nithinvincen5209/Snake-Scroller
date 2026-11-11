using UnityEngine;

namespace SnakeScroller
{
    // The special effect a food can have. The effects themselves come in Step 10.
    public enum FoodEffectType
    {
        None,
        SpeedUp,
        SlowDown,
        DoubleScore,
        Shrink
    }

    // Holds the data for one food type, so new foods are new assets, not new code.
    [CreateAssetMenu(fileName = "FoodData", menuName = "SnakeScroller/Food Data")]
    public class FoodData : ScriptableObject
    {
        [SerializeField] private string _foodName = "Apple";
        [SerializeField] private Color _color = Color.white;
        [SerializeField, Min(0)] private int _scoreValue = 10;
        [SerializeField, Min(0f)] private float _spawnWeight = 1f;
        [SerializeField] private FoodEffectType _effectType = FoodEffectType.None;
        [SerializeField] private Sprite _sprite;
        [SerializeField, Min(0f)] private float _lifetimeSeconds = 0f;
        [SerializeField, Min(0f)] private float _effectDuration = 3f;
        [SerializeField, Min(0.01f)] private float _effectMultiplier = 2f;

        // Name shown to the player or in logs.
        public string FoodName => _foodName;

        // Tint for the food sprite. White keeps the sprite's own colours.
        public Color Color => _color;

        // Points the player gets for eating this food.
        public int ScoreValue => _scoreValue;

        // How often this food appears compared to others (used in Step 10).
        public float SpawnWeight => _spawnWeight;

        // Which special effect this food has.
        public FoodEffectType EffectType => _effectType;

        // The picture drawn for this food.
        public Sprite Sprite => _sprite;

        // Seconds before the food moves away. 0 means it stays until eaten.
        public float LifetimeSeconds => _lifetimeSeconds;

        // How long the effect lasts after eating, in seconds.
        public float EffectDuration => _effectDuration;

        // How strong the effect is. For SpeedUp, 2 means twice as fast.
        public float EffectMultiplier => _effectMultiplier;
    }
}
