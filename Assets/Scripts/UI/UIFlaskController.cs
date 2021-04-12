using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

    public class UIFlaskController : MonoBehaviour
    {
        public enum FlaskTypes
        {
            Health,
            Mana
        }

        public RawImage[] images;
        public float offsetScale = 1f;
        public int index;
        public float widthPerPoint = 100f;
        public Color color;
        public Image textBackImage;
        public TextMeshProUGUI valueText;
        public RectTransform barBackRectTransform;
        public RectTransform barMaskRectTransform;
        public Image liquidLineImage;
        public Image liquidBottom;
        public FlaskTypes flaskType;

        private float _value;
        private float _maxValue;

        // Start is called before the first frame update
        void Start()
        {
            switch (flaskType)
            {
                case FlaskTypes.Health:
                    PlayerLifeController.HpChanged += OnValueChanged;
                    PlayerLifeController.MaxHpChanged += OnMaxValueChanged;
                    break;
                case FlaskTypes.Mana:
                    ManaController.MpChanged += OnValueChanged;
                    ManaController.MaxMpChanged += OnMaxValueChanged;
                    break;
            }
            InitLiquidAnimation();
            textBackImage.color = color;
            liquidLineImage.color = color;
            liquidBottom.color = color;
        }

        private void InitLiquidAnimation()
        {
            index = Convert.ToInt32(UnityEngine.Random.Range(0f, 5000f));
            for (int i = 0; i < images.Length; i++)
            {
                var image = images[i];
                var offset = image.uvRect;
                offset.x = UnityEngine.Random.Range(0f, 1f);
                offset.y = UnityEngine.Random.Range(0f, 1f);
                image.uvRect = offset;
            }
        }

        // Update is called once per frame
        void Update()
        {
            valueText.text = Math.Min(_value, 9999).ToString("0");
            barBackRectTransform.sizeDelta = new Vector2(_maxValue * widthPerPoint, barBackRectTransform.rect.height);
            barMaskRectTransform.sizeDelta = new Vector2(_value * widthPerPoint - 4f, barMaskRectTransform.rect.height);
            UpdateLiquidAnimation();
        }

        private void UpdateLiquidAnimation()
        {
            for (int i = 0; i < images.Length; i++)
            {
                var image = images[i];
                var offset = image.uvRect;
                var direction = GetDirection(i);
                offset.x += (Mathf.PerlinNoise(Convert.ToSingle(index * i), 0f) - 0.5f) * offsetScale * direction.x;
                offset.y += (Mathf.PerlinNoise(Convert.ToSingle(index * i) - 1000f, 0f) - 0.5f) * offsetScale * direction.y;
                image.uvRect = offset;
            }
            index++;
            if (index > 100000)
            {
                index = 0;
            }
        }

        private void OnValueChanged(float value)
        {
            _value = value;
        }

        private void OnMaxValueChanged(float maxValue)
        {
            _maxValue = maxValue;
        }

        private Vector2 GetDirection(int index)
        {
            switch (index)
            {
                case 0:
                    return new Vector2(1f, 1f);
                case 1:
                    return new Vector2(1f, -1f);
                case 2:
                    return new Vector2(-1f, 1f);
            }
            return new Vector2(-1f, -1f);
        }
    }
