using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiBankingExercise.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Async lifecycle method that runs when the ViewModel appears.
        /// Override in derived classes to load data asynchronously.
        /// </summary>
        public virtual Task OnAppearingAsync()
        {
            // Default: return a completed task
            return Task.CompletedTask;
        }
    }
}
