using IDKEY = System.Int64;

namespace YATM.Core.Interfaces
{
    public interface IEntity
    {
        IDKEY Id { get; set; }
    }
}
