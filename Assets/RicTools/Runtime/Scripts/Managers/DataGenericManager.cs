using RicTools.ScriptableObjects;

namespace RicTools.Managers
{
    public abstract class DataGenericManager<T, D> : SingletonGenericManager<T> where T : DataGenericManager<T, D> where D : DataManagerScriptableObject
    {
        public D data;
    }
}
