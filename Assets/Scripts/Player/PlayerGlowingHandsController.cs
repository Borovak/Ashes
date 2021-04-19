using Static;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Player
{
    public class PlayerGlowingHandsController : MonoBehaviour
    {
        public Light2D[] lights;
        public float intensityWhenActive = 1f;
        public float changeRate = 1f;
        public bool state;
    
        // Update is called once per frame
        void Update()
        {
            var targetIntensity = state ? intensityWhenActive : 0f;
            var delta = changeRate * Time.deltaTime * (state ? 1f : -1f);
            foreach (var light in lights)
            {
                light.intensity = GlobalFunctions.Bound(light.intensity + delta, 0f, intensityWhenActive);
            }
        }
    }
}
