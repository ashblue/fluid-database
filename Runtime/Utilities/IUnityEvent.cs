using UnityEngine.Events;

namespace CleverCrow.Fluid.Databases.Utilities {
    public interface IUnityEvent {
        void AddListener (UnityAction call);
        void RemoveListener (UnityAction call);
        void Invoke ();
    }

    public interface IUnityEvent<T1> {
        void AddListener (UnityAction<T1> call);
        void Invoke (T1 arg1);
    }

    public interface IUnityEvent<T1, T2> {
        void AddListener (UnityAction<T1, T2> call);
        void Invoke (T1 arg1, T2 arg2);
    }
}
