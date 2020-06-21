namespace Wowsome {
  namespace Serialization {
    public interface IPersister {
      void Save(IDataSerializer serializer);
      void Reset(IDataSerializer serializer);
      void Init(IDataSerializer serializer);
      void StartPersister(GamePersisters persisters);
    }

    public interface IPersister<T> : IPersister { }
  }
}