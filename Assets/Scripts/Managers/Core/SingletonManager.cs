using CustomHelper;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers.Core
{
    [HideMonoScript]
    public abstract class SingletonManager<T> : MonoBehaviour where T: Object
    {
        protected abstract T Self { set; get; }
        protected abstract void Init();

        private void Awake()
        {
            var managerController = FindObjectOfType<ManagerController>();

            if (!managerController)
            {
                this.LogError("Not initialized, no manager controller found.");
                Destroy(gameObject);
                return;
            }

            if (Self != null)
            {
                Destroy(gameObject);
                return;
            }
                
            transform.SetParent(managerController.transform);
            Self = this as T;
            Init();
        }
    }
}