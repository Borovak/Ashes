using System;
using UnityEngine;

namespace Player
{
    public class PlayerLifeController : LifeController
    {
        public static event Action<float> HpChanged;
        public static event Action<float> MaxHpChanged;
        private float _hp;
        private float _maxHp;
        public AudioClip deathMusic;

        protected override void AfterStart()
        {
            SaveSystem.GameSaved += OnGameSaved;
            SetHp(SaveSystem.LastLoadedSave.Hp);
            SetMaxHp(SaveSystem.LastLoadedSave.MaxHp);
        }

        // Update is called once per frame
        protected override void AfterUpdate()
        {
        }

        protected override void SetMaxHp(float value)
        {
            _maxHp = value;
            MaxHpChanged?.Invoke(_maxHp);
        }

        public override float GetMaxHp()
        {
            return _maxHp;
        }

        protected override void SetHp(float value)
        {
            _hp = value;
            HpChanged?.Invoke(_hp);
        }

        public override float GetHp()
        {
            return _hp;
        }

        protected override void OnDeath()
        {
            DeathScreenController.Show();
        }

        private void OnGameSaved(bool healOnSave)
        {
            if (!healOnSave) return;
            SetHp(SaveSystem.LastLoadedSave.MaxHp);
        }
    }
}
