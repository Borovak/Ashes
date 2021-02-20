using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CutsceneSkipController : MonoBehaviour
    {
        public const float SkipDelay = 2f;
        private const float SkipVisibilityTimeout = 3f;
    
        public Image fillImage;

        private float _skipTimer;
        private bool _visibility;
        private bool _previousVisibility;

        public void Reset()
        {
            _skipTimer = SkipVisibilityTimeout;
            _previousVisibility = false;
            _visibility = true;
        }

        // Update is called once per frame
        void Update()
        {
            //Change fill amount on circle
            fillImage.fillAmount = MenuInputs.okButtonPressedTimer / SkipDelay;
            if (fillImage.fillAmount > 0)
            {
                _skipTimer = SkipVisibilityTimeout;
            }
            //Update timer
            _visibility = _skipTimer > 0f;
            if (_skipTimer > 0f)
            {
                _skipTimer -= Time.deltaTime;
            }
            //Change children visibility if necessary
            if (_visibility && !_previousVisibility)
            {
                ChangeChildrenVisibility(true);
            } 
            else if (!_visibility && _previousVisibility)
            {
                ChangeChildrenVisibility(false);
            }
            _previousVisibility = _visibility;
        }

        private void ChangeChildrenVisibility(bool visibility)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(visibility);
            }
        }
    }
}
