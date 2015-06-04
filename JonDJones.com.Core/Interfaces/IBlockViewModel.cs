
using EPiServer.Core;

namespace JonDJones.Com.Core.Interfaces
{
    public interface IBlockViewModel<out T> where T : BlockData
    {
        T CurrentBlock { get; }
    }
}