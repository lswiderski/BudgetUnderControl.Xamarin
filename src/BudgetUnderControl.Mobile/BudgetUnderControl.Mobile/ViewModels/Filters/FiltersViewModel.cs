using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.CommonInfrastructure;
using BudgetUnderControl.Mobile;
using BudgetUnderControl.Mobile.Keys;
using BudgetUnderControl.Mobile.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BudgetUnderControl.ViewModel
{
    public class FiltersViewModel : BaseViewModel, IFiltersViewModel
    {

        TransactionsFilter filter;
        public TransactionsFilter Filter
        {
            get { return filter; }
            set { SetProperty(ref filter, value); }
        }


        private DateTime fromDate;
        public DateTime FromDate
        {
            get => fromDate;
            set { SetProperty(ref fromDate, value); }
        }

        private DateTime toDate;
        public DateTime ToDate
        {
            get => toDate;
            set { SetProperty(ref toDate, value); }
        }

        private string text;
        public string Text
        {
            get => text;
            set { SetProperty(ref text, value); }
        }

        ObservableCollection<AccountListItemDTO> accounts;
        public ObservableCollection<AccountListItemDTO> Accounts
        {
            get => accounts;
            set { SetProperty(ref accounts, value); }
        }

        public ObservableCollection<AccountListItemDTO> SelectedAccounts { get; set; }

        ObservableCollection<TagDTO> tags;
        public ObservableCollection<TagDTO> Tags
        {
            get => tags;
            set { SetProperty(ref tags, value); }
        }

        public ObservableCollection<TagDTO> SelectedTags { get; set; }

        private object selectedTagIndices;
        public object SelectedTagIndices
        {
            get => selectedTagIndices;
            set
            {
                SetProperty(ref selectedTagIndices, value);
            }
        }

        private object selectedCategoryIndices;
        public object SelectedCategoryIndices
        {
            get => selectedCategoryIndices;
            set
            {
                SetProperty(ref selectedCategoryIndices, value);
            }
        }

        private object selectedAccountIndices;
        public object SelectedAccountIndices
        {
            get => selectedAccountIndices;
            set
            {
                SetProperty(ref selectedAccountIndices, value);
            }
        }

        ObservableCollection<CategoryListItemDTO> categories;
        public ObservableCollection<CategoryListItemDTO> Categories
        {
            get => categories;
            set { SetProperty(ref categories, value); }
        }

        public ObservableCollection<CategoryListItemDTO> SelectedCategories { get; set; }

        IAccountMobileService accountService;
        ICategoryService categoryService;
        ITagService tagService;
        private readonly IMemoryCache memoryCache;


        public FiltersViewModel(IAccountMobileService accountService, ICategoryService categoryService, ITagService tagService, IMemoryCache memoryCache)
        {
            this.accountService = accountService;
            this.categoryService = categoryService;
            this.tagService = tagService;
            this.memoryCache = memoryCache;

            this.GetDropdowns();
        }

        async void GetDropdowns()
        {
            Accounts = new ObservableCollection<AccountListItemDTO>(await accountService.GetAccountsForSelect());
            Categories = new ObservableCollection<CategoryListItemDTO>(await categoryService.GetCategoriesAsync());
            Tags = new ObservableCollection<TagDTO>(await tagService.GetActiveTagsAsync());
        }

        public void SetFilter(TransactionsFilter filter = null)
        {
            Filter = filter ?? new TransactionsFilter();
            GetDropdowns();
            ToDate = filter.ToDate ?? DateTime.Now.Date;
            FromDate = filter.FromDate ?? DateTime.Now.Date;
            Text = filter.SearchQuery;
            
            if (this.Filter.AccountsIds != null)
            {
                var accountIndexes = new List<int>();
                foreach (var id in this.Filter.AccountsIds)
                {
                    var index = this.Accounts.ToList().FindIndex(x => x.Id == id);
                    accountIndexes.Add(index);
                }
                this.SelectedAccountIndices = accountIndexes;
            }
            
            if(this.Filter.CategoryIds != null)
            {
                var categoryIndexes = new List<int>();
                foreach (var id in this.Filter.CategoryIds)
                {
                    var index = this.Categories.ToList().FindIndex(x => x.Id == id);
                    categoryIndexes.Add(index);
                }
                this.SelectedCategoryIndices = categoryIndexes;
            }
            
            if(this.Filter.TagIds != null)
            {
                var tagIndexes = new List<int>();
                foreach (var id in this.Filter.TagIds)
                {
                    var index = this.Tags.ToList().FindIndex(x => x.Id == id);
                    tagIndexes.Add(index);
                }
                this.SelectedTagIndices = tagIndexes;
            }
            memoryCache.Set(CacheKeys.Filters, this.Filter, DateTimeOffset.Now.AddMinutes(5));
        }

        public TransactionsFilter BuildFilter()
        {
            this.Filter.FromDate = FromDate;
            this.Filter.ToDate = ToDate;
            this.Filter.SearchQuery = Text;
            this.Filter.TagIds = this.SelectedTags?.Select(x => x.Id).ToList();
            this.Filter.AccountsIds = this.SelectedAccounts?.Select(x => x.Id).ToList();
            this.Filter.CategoryIds = this.SelectedCategories?.Select(x => x.Id).ToList();
            return this.Filter;
        }

        public TransactionsFilter ResetFilter()
        {
            var now = DateTime.UtcNow;
            FromDate = new DateTime(now.Year, now.Month, 1, 0, 0, 0);
            ToDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month), 23, 59, 59);
            Text = string.Empty;
            this.Filter = new TransactionsFilter { FromDate = FromDate, ToDate = ToDate, SearchQuery = Text };
            memoryCache.Set(CacheKeys.Filters, this.Filter, DateTimeOffset.Now.AddMinutes(5));
            return this.Filter;
        }
    }
}
