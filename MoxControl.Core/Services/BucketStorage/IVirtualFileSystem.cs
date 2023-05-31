using Microsoft.Extensions.FileProviders;

namespace YATM.Core.Services.BucketStorage
{
    public interface IVirtualFileSystem
    {
        IEnumerable<IFileInfo> EnumerateChildObjects(string path);
        IFileInfo GetObject(string path);
    }
}
