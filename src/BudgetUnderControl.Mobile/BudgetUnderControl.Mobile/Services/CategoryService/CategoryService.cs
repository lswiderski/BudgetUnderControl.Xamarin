using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.MobileDomain;
using BudgetUnderControl.MobileDomain.Repositiories;
using BudgetUnderControl.CommonInfrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetUnderControl.CommonInfrastructure;

namespace BudgetUnderControl.Mobile.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IIconService iconService;

        public CategoryService(ICategoryRepository categoryRepository, IIconService iconService)
        {
            this.categoryRepository = categoryRepository;
            this.iconService = iconService;
        }

        public async Task<ICollection<CategoryListItemDTO>> GetCategoriesAsync()
        {
            var categories = await this.categoryRepository.GetCategoriesAsync();

            var dtos = categories.Select(x => new CategoryListItemDTO
            {
                Id = x.Id,
                Name = x.Name,
                Icon = this.iconService.GetIcon(x.Icon),
                ExternalId = Guid.Parse(x.ExternalId)
            })
               .ToList();

            return dtos;
        }

        public async Task<CategoryListItemDTO> GetCategoryAsync(Guid id)
        {
            var category = await this.categoryRepository.GetCategoryAsync(id.ToString());

            var dto = new CategoryListItemDTO
            {
                Id = category.Id,
                Name = category.Name,
                Icon = this.iconService.GetIcon(category.Icon),
                ExternalId = Guid.Parse(category.ExternalId)
            };

            return dto;
        }

        public async Task<bool> IsValidAsync(int categoryId)
        {
            var currencies = await this.categoryRepository.GetCategoriesAsync();
            return currencies.Any(x => x.Id == categoryId);
        }
    }
}
