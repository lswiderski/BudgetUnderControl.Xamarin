using BudgetUnderControl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BudgetUnderControl.ViewModel
{
    public interface IFiltersViewModel
    {
        TransactionsFilter Filter { get; }

        TransactionsFilter BuildFilter();

        TransactionsFilter ResetFilter();

        void SetFilter(TransactionsFilter filter = null);

        ObservableCollection<TagDTO> SelectedTags { get; set; }

        ObservableCollection<AccountListItemDTO> SelectedAccounts { get; set; }

        ObservableCollection<CategoryListItemDTO> SelectedCategories { get; set; }
    }
}
