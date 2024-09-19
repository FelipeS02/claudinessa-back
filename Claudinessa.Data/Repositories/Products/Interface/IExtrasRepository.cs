using Claudinessa.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claudinessa.Data.Repositories.Products.Interface
{
    public interface IExtrasRepository
    {
        Task<bool> CreateExtra(NExtra extra);
        Task<bool> UpdateExtra(NExtra extra);
        Task<bool> DeleteExtra(int IdExtra);
    }
}
