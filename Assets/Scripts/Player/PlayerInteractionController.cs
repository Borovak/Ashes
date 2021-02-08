using System.Linq;
using Static;
using UnityEngine;

namespace Player
{
    public class PlayerInteractionController : MonoBehaviour
    {

        public static PlayerInteractionController Instance;
        public InteractionController interactionController;

        private PlayerInputs _inputs;

        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
            _inputs = GetComponent<PlayerInputs>();
            _inputs.Interact += Interact;
        }

        // Update is called once per frame
        void Update()
        {
            var interactionColliders = Physics2D.OverlapCircleAll(transform.position, 1f, LayerManagement.Interaction);
            if (interactionColliders.Length == 0 || !interactionColliders.First().gameObject.TryGetComponent<InteractionController>(out interactionController))
            {
                interactionController = null;
            }
        }

        public void Interact()
        {
            if (interactionController == null) return;
            interactionController.Interact();
        }
    }
}
