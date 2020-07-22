using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.CommonInfrastructure.Commands;
using BudgetUnderControl.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public interface ITransactionsViewModel
    {
        ObservableCollection<ObservableGroupCollection<string, TransactionListItemDTO>> Transactions { get; set; }
        TransactionListItemDTO SelectedTransaction { get; set; }
        string ActualRange { get; }

        Task LoadTransactionsAsync();
        Task SetNextMonth();
        Task SetPreviousMonth();

        TransactionsFilter Filter { get; set; }
    }
}
