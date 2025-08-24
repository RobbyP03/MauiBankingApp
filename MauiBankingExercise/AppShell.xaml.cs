using MauiBankingExercise.Views;

namespace MauiBankingExercise

{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute("CustomerDashboardroute", typeof(CustomerDashboardView));
            Routing.RegisterRoute("updateTransactionroute", typeof(TransactionView));
        }
    }
}
