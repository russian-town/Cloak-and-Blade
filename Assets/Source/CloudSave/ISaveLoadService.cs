public interface ISaveLoadService
{
    public void AddDataWriters(IDataWriter[] dataWriters);

    public void AddDataReaders(IDataReader[] dataReaders);

    public void Save(PlayerData playerData);

    public void Load();
}
