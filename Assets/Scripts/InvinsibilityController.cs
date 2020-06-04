using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvinsibilityController : MonoBehaviour
{
    public float invinsibilityPeriodAfterDamage = 2f;
    public Color damageColor;
    public float invinsibilityFlashRate;

    private float _invinsibleFor;
    private float _invinsibilityFlash = 0f;
    private SpriteRenderer[] _spriteRenderers;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_invinsibleFor > 0)
        {
            _invinsibleFor -= Time.deltaTime;
            if (_invinsibilityFlash == 0f)
            {
                _invinsibilityFlash = 2f;
            }
        }
        _invinsibilityFlash = Mathf.Max(_invinsibilityFlash - invinsibilityFlashRate * Time.deltaTime, 0f);
        var c1 = _invinsibilityFlash > 1f ? damageColor : Color.white;
        var c2 = _invinsibilityFlash > 1f ? Color.white : damageColor;
        var c = Color.Lerp(c1, c2, _invinsibilityFlash % 1f);
        foreach (var spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.color = c;
        }
    }

    public void TakeDamage()
    {
        if (_invinsibleFor > 0) return;
        _invinsibleFor = invinsibilityPeriodAfterDamage;
        PlayerPlatformerController.playerData.Hp -= 1;
        if (PlayerPlatformerController.playerData.Hp <= 0)
        {
            if (CampsiteController.campsites.TryGetValue(PlayerPlatformerController.playerData.CampsiteId, out var campsite))
            {
                transform.position = campsite.transform.position;
                PlayerPlatformerController.playerData.Hp = PlayerPlatformerController.playerData.MaxHp;
            }
        }
    }

}
