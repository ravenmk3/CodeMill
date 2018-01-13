using System;

namespace WebPirates.Cilizhu.Core.Data
{
    public interface IDataAccessFactory
    {
        IDataAccessService Create();
    }
}
