using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Erp.Interfaces
{
    public interface IPdfService : IApplicationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<List<string>> GetPDFasHtml(Uri uri);
    }
}
