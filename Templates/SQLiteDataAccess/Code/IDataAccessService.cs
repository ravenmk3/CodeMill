using System;
using System.Collections.Generic;
using System.Data;
using WebPirates.Cilizhu.Core.Domain.Entities;

namespace WebPirates.Cilizhu.Core.Data
{
    public interface IDataAccessService : IDisposable
    {
        void Begin(IsolationLevel level);

        void Commit();

        void Rollback();

        void Initialize();
    }
}
