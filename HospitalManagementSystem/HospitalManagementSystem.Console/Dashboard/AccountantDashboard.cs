using System;
using System.Threading.Tasks;
using HospitalManagementSystem.ConsoleApp.Models;
using HospitalManagementSystem.ConsoleApp.Services;
using System.Linq;

namespace HospitalManagementSystem.ConsoleApp.Dashboard
{
    public class AccountantDashboard : IDashboard
    {
        private readonly UserSession _session;
        private readonly IDataService _dataService;

        public AccountantDashboard(UserSession session, IDataService dataService)
        {
            _session = session;
            _dataService = dataService;
        }

        public async Task ShowAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== ACCOUNTANT DASHBOARD ===");
                Console.WriteLine($"Welcome, {_session.FullName} ({_session.UserType})!");
                Console.WriteLine("===============================");
                Console.WriteLine("1. View All Bills");
                Console.WriteLine("2. View All Payments");
                Console.WriteLine("3. View All Transactions");
                Console.WriteLine("4. Approve Pending Transactions");
                Console.WriteLine("5. Generate Financial Reports");
                Console.WriteLine("6. Logout");
                Console.Write("\nSelect option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ViewAllBills();
                        break;
                    case "2":
                        await ViewAllPayments();
                        break;
                    case "3":
                        await ViewAllTransactions();
                        break;
                    case "4":
                        await ApprovePendingTransactions();
                        break;
                    case "5":
                        await GenerateFinancialReports();
                        break;
                    case "6":
                        Console.WriteLine("\nLogging out...");
                        await Task.Delay(1000);
                        return;
                    default:
                        Console.WriteLine("\nInvalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private async Task ViewAllBills()
        {
            Console.Clear();
            Console.WriteLine("=== ALL BILLS ===\n");

            var bills = await _dataService.GetAllBillsAsync();

            Console.WriteLine($"{"Bill ID",-8} | {"Patient ID",-11} | {"Date",-12} | {"Amount",-10} | {"Status",-10} | {"Description"}");
            Console.WriteLine(new string('-', 90));

            foreach (var bill in bills)
            {
                Console.WriteLine($"{bill.BillId,-8} | {bill.PatientId,-11} | {bill.Date:yyyy-MM-dd} | ${bill.Amount,8:N2} | {bill.Status,-10} | {bill.Description}");
            }

            var totalBilled = bills.Sum(b => b.Amount);
            var totalPaid = bills.Where(b => b.Status == "Paid").Sum(b => b.Amount);
            var totalPending = bills.Where(b => b.Status == "Pending").Sum(b => b.Amount);

            Console.WriteLine($"\nTotal Billed: ${totalBilled:N2}");
            Console.WriteLine($"Total Paid: ${totalPaid:N2}");
            Console.WriteLine($"Total Pending: ${totalPending:N2}");

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task ViewAllPayments()
        {
            Console.Clear();
            Console.WriteLine("=== ALL PAYMENTS ===\n");

            var payments = await _dataService.GetAllPaymentsAsync();

            Console.WriteLine($"{"Payment ID",-12} | {"Patient ID",-11} | {"Date",-12} | {"Amount",-10} | {"Method",-15} | {"Reference"}");
            Console.WriteLine(new string('-', 90));

            foreach (var payment in payments)
            {
                Console.WriteLine($"{payment.PaymentId,-12} | {payment.PatientId,-11} | {payment.Date:yyyy-MM-dd} | ${payment.Amount,8:N2} | {payment.Method,-15} | {payment.Reference}");
            }

            var totalRevenue = payments.Sum(p => p.Amount);

            Console.WriteLine($"\nTotal Revenue: ${totalRevenue:N2}");
            Console.WriteLine($"Total Payments: {payments.Count}");

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task ViewAllTransactions()
        {
            Console.Clear();
            Console.WriteLine("=== ALL TRANSACTIONS ===\n");

            var transactions = await _dataService.GetTransactionsAsync();

            Console.WriteLine($"{"ID",-6} | {"Patient ID",-11} | {"Amount",-10} | {"Type",-12} | {"Status",-10} | {"Date",-12} | {"Approved By"}");
            Console.WriteLine(new string('-', 100));

            foreach (var t in transactions)
            {
                var approvedBy = t.ApprovedBy.HasValue ? $"User {t.ApprovedBy}" : "N/A";
                Console.WriteLine($"{t.TransactionId,-6} | {t.PatientId,-11} | ${t.Amount,8:N2} | {t.Type,-12} | {t.Status,-10} | {t.Date:yyyy-MM-dd} | {approvedBy}");
            }

            Console.WriteLine("\nTotal Transactions: " + transactions.Count);
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task ApprovePendingTransactions()
        {
            Console.Clear();
            Console.WriteLine("=== APPROVE PENDING TRANSACTIONS ===\n");

            var transactions = await _dataService.GetTransactionsAsync("Pending");

            if (transactions.Count == 0)
            {
                Console.WriteLine("No pending transactions.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"{"ID",-6} | {"Patient ID",-11} | {"Amount",-10} | {"Type",-12} | {"Date",-12} | {"Notes"}");
            Console.WriteLine(new string('-', 80));

            for (int i = 0; i < transactions.Count; i++)
            {
                var t = transactions[i];
                Console.WriteLine($"{i + 1}. {t.TransactionId,-5} | {t.PatientId,-11} | ${t.Amount,8:N2} | {t.Type,-12} | {t.Date:yyyy-MM-dd} | {t.Notes ?? "N/A"}");
            }

            Console.Write("\nEnter transaction number to approve (or 0 to cancel): ");
            if (int.TryParse(Console.ReadLine(), out var index) && index > 0 && index <= transactions.Count)
            {
                var transaction = transactions[index - 1];
                var approved = await _dataService.ApproveTransactionAsync(transaction.TransactionId, _session.UserId);

                if (approved)
                {
                    Console.WriteLine($"\nTransaction #{transaction.TransactionId} approved successfully!");
                    await _dataService.LogAuditAsync(_session.UserId, _session.Username, "Approved transaction", "Transaction", transaction.TransactionId, $"Approved transaction: ${transaction.Amount:N2}");
                }
                else
                {
                    Console.WriteLine("\nFailed to approve transaction.");
                }
            }
            else
            {
                Console.WriteLine("\nNo changes made.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task GenerateFinancialReports()
        {
            Console.Clear();
            Console.WriteLine("=== FINANCIAL REPORTS ===\n");

            var bills = await _dataService.GetAllBillsAsync();
            var payments = await _dataService.GetAllPaymentsAsync();
            var transactions = await _dataService.GetTransactionsAsync();

            var totalBilled = bills.Sum(b => b.Amount);
            var totalPaid = payments.Sum(p => p.Amount);
            var pendingAmount = bills.Where(b => b.Status == "Pending").Sum(b => b.Amount);

            Console.WriteLine($"Total Billed: ${totalBilled:N2}");
            Console.WriteLine($"Total Paid: ${totalPaid:N2}");
            Console.WriteLine($"Pending Amount: ${pendingAmount:N2}");
            Console.WriteLine($"Collection Rate: {(totalPaid * 100.0m / totalBilled):F1}%");

            Console.WriteLine("\nBy Payment Method:");
            var byMethod = payments.GroupBy(p => p.Method).ToDictionary(g => g.Key, g => g.Sum(p => p.Amount));
            foreach (var kvp in byMethod)
            {
                Console.WriteLine($"  {kvp.Key}: ${kvp.Value:N2}");
            }

            Console.WriteLine($"\nTotal Transactions: {transactions.Count}");
            Console.WriteLine($"Approved Transactions: {transactions.Count(t => t.Status == "Approved")}");
            Console.WriteLine($"Pending Transactions: {transactions.Count(t => t.Status == "Pending")}");

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
