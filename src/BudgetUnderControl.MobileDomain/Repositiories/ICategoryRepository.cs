using BudgetUnderControl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.MobileDomain.Repositiories
{
    public interface ICategoryRepository
    {
        Task<ICollection<Category>> GetCategoriesAsync();
        Task<ICollection<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryAsync(string externalId);

        Task<Category> GetCategoryByNameAsync(string name);
        Task UpdateAsync(Category category);
        Task AddCategoryAsync(Category category);
        Task AddCategoriesAsync(IEnumerable<Category> categories);
        Task HardRemoveCategoriesAsync(IEnumerable<Category> categories);
    }
}
