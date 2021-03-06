﻿using BudgetUnderControl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BudgetUnderControl.ViewModel
{
    public interface IEditTransactionViewModel
    {
        double? Longitude { get; set; }
        double? Latitude { get; set; }

        byte[] ImageByteArray { get; set; }
        ImageSource ImageSource { get; set; }
        bool HasImage { get; set; }
        bool HasNoImage { get; set; }

        Task EditTransactionAsync();
        Task GetTransactionAsync(Guid transactionId);
        Task DeleteTransactionAsync();

        TagDTO SelectedTag { get; set; }
        ObservableCollection<TagDTO> Tags { get; set; }
        Task AddTagAsync(Guid tagId);
        Task RemoveTagFromListAsync(Guid tagId);
    }
}
