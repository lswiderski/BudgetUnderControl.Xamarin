﻿using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public interface IAddTransactionViewModel
    {
        void AddTransacion();
        
        List<CategoryListItemDTO> Categories { get; }
        List<AccountListItemDTO> Accounts { get; }
        ICollection<string> Types { get; }
        int SelectedTypeIndex { get; set; }
        TimeSpan TransferTime { get; set; }
        DateTime TransferDate { get; set; }
        string TransferRate { get; set; }
        string TransferAmount { get; set; }
        int SelectedTransferAccountIndex { get; set; }
        int SelectedAccountIndex { get; set; }
        int SelectedCategoryIndex { get; set; }
        string Amount { get; set; }
        string Comment { get; set; }
        string Name { get; set; }
        DateTime Date { get; set; }
        TimeSpan Time { get; set; }
        TransactionType Type { get; }

        bool IsItTransfer { get; }
        bool IsTransferOptionsVisible { get; }
        bool IsTransferInOtherCurrency { get; }
        bool IsValid { get; }
    }
}