namespace onwardslib
{
    public abstract class Manager<T> where T : IMaestro
    {
        protected T Maestro { get; }
        protected Manager(T maestro)
        {
            Maestro = maestro;
        }
    }
}
