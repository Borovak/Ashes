using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CutsceneSkipController : MonoBehaviour
    {
        private const float SkipVisibilityTimeout = 3f;

        public UIControllerButton uiControllerButton;

        private float _skipTimer;
        private bool _visibility;
        private bool _previousVisibility;

        void Start()
        {
            _skipTimer = SkipVisibilityTimeout;
            _previousVisibility = false;
            _visibility = true;
        }

        // Update is called once per frame
        void Update()
        {
            //Change fill amount on circle
            if (uiControllerButton.IsPressed)
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
