using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers.Core
{
    [HideMonoScript]
    public class ManagerController : MonoBehaviour
    {
        private static ManagerController _instance;
        
        private void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(this);
        }
    }
}
