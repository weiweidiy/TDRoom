using Unity.Netcode;

public struct WaveData : INetworkSerializable
{
    public ushort waveNumber;
    public ushort waveDuration;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref waveNumber);
        serializer.SerializeValue(ref waveDuration);
    }
}
