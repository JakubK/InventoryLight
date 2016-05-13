using UnityEngine;
using System.Collections;
namespace Assets.Scripts.UI
{
    public class UIOpener : MonoBehaviour
    {
        public KeyCode KeyToReopen;
        public GameObject Target;
        bool enabled = false;

        void Start()
        {
            Target.SetActive(enabled);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyToReopen))
            {
                enabled = !enabled;
                Target.SetActive(enabled);
            }
        }
    }
}
