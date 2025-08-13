using FFAppMiddleware.Model.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFappMiddleware.ApplicationServices.Services.Abstract
{
    public interface IProductService
    {
        Task<List<ProductApiModel>> RetrieveProducts();
    }
}
