using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace FinaFlowUI.Pages
{
    public class Transaction
    {
        public int Id { get; set; }
        public String MyAccount { get; set; }
        public String ExternalAccount { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }


    public sealed partial class RecordEditPage : Page
    {
        public ObservableCollection<string> MyAccounts;
        public ObservableCollection<string> ExAccounts;

        private List<Transaction> Transactions { get; set; }
        private int pageNumber;
        private int numColumns;
        private int numRows;
        private const int ROWS_PER_PAGE = 10;

        public RecordEditPage()
        {
            this.InitializeComponent();
            Transactions = new List<Transaction>();
            MyAccounts = new ObservableCollection<string>();
            ExAccounts = new ObservableCollection<string>();
            numColumns = 0;
            numRows = 0;
        }

        public bool AddRecord(Transaction transaction)
        {
            if (TransactionExists(transaction))
                return false;

            Transactions.Add(transaction);
            return true;
        }

        public bool LoadPage(int pageIndex)
        {
            if (pageIndex < 0) return false;
            if (pageIndex > Transactions.Count / ROWS_PER_PAGE) return false;

            ClearTranTable();
            this.pageNumber = pageIndex;
            pageSelectNumber.Text = (pageIndex + 1).ToString();

            SetTranTableHeaders();
            SetTranTableRecords();

            return true;
        }

        private void ClearTranTable()
        {
            tranTable.Children.Clear();
            numColumns = 0;
            numRows = 0;
        }

        private void SetTranTableHeaders()
        {
            AddTranTableHeader("Id");
            AddTranTableHeader("My Account");
            AddTranTableHeader("External Account");
            AddTranTableHeader("Amount");
            AddTranTableHeader("Date");
        }

        private void AddTranTableHeader(string headerTitle)
        {
            TextBlock header = new TextBlock { Text = headerTitle, FontWeight = FontWeights.Bold };
            tranTable.Children.Add(header);
            Grid.SetRow(header, 0);
            Grid.SetColumn(header, numColumns);
            numColumns++;
        }

        private void SetTranTableRecords()
        {
            int tranIndexStart = ROWS_PER_PAGE * pageNumber;
            int tranIndexEnd = Math.Min(ROWS_PER_PAGE * (pageNumber + 1), Transactions.Count);
            for (int i = tranIndexStart; i < tranIndexEnd; i++)
            {
                AddTranTableRecord(i);
            }
            AddPaddingRowsToTranTable();
        }

        private void AddTranTableRecord(int recordIndex)
        {
            TextBlock idText = new TextBlock { Text = Transactions[recordIndex].Id.ToString() };
            TextBlock myAccText = new TextBlock { Text = Transactions[recordIndex].MyAccount };
            TextBlock exAccText = new TextBlock { Text = Transactions[recordIndex].ExternalAccount };
            TextBlock amtText = new TextBlock { Text = Transactions[recordIndex].Amount.ToString() };
            TextBlock dateText = new TextBlock { Text = Transactions[recordIndex].Date.ToLongDateString() };
            tranTable.Children.Add(idText);
            tranTable.Children.Add(myAccText);
            tranTable.Children.Add(exAccText);
            tranTable.Children.Add(amtText);
            tranTable.Children.Add(dateText);
            Grid.SetRow(idText, numRows + 1);
            Grid.SetRow(myAccText, numRows + 1);
            Grid.SetRow(exAccText, numRows + 1);
            Grid.SetRow(amtText, numRows + 1);
            Grid.SetRow(dateText, numRows + 1);
            Grid.SetColumn(idText, 0);
            Grid.SetColumn(myAccText, 1);
            Grid.SetColumn(exAccText, 2);
            Grid.SetColumn(amtText, 3);
            Grid.SetColumn(dateText, 4);
            numRows++;
        }

        private void AddPaddingRowsToTranTable()
        {
            for (int i = numRows; i < ROWS_PER_PAGE; i++)
            {
                TextBlock padText = new TextBlock { Text = "-" };
                tranTable.Children.Add(padText);
                Grid.SetRow(padText, i + 1);
                Grid.SetColumn(padText, 0);
            }
        }

        private bool TransactionExists(Transaction transaction)
        {
            foreach (var t in Transactions)
            {
                if (t.Id == transaction.Id)
                    return true;
            }
            return false;
        }

        private bool TransactionExists(int id)
        {
            foreach (var t in Transactions)
            {
                if (t.Id == id)
                    return true;
            }
            return false;
        }

        private Transaction GetTransaction(int id)
        {
            foreach (var t in Transactions)
            {
                if (t.Id == id)
                    return t;
            }
            throw new IndexOutOfRangeException("no transaction with the given id " + id.ToString() + "exists. ");
        }

        private List<string> GetMyAccounts()
        {
            return new List<string>
            {
                "Current",
                "Savings",
                "Test",
            };
        }

        private List<string> GetExAccounts()
        {
            return new List<string>
            {
                "Tesco",
                "Work",
                "Test",
            };
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //temp, should load from database
            AddRecord(new Transaction { Id = 0, MyAccount = "Current", ExternalAccount = "Tesco", Amount = -12.34, Date = new DateTime(2021, 10, 8) });
            AddRecord(new Transaction { Id = 1, MyAccount = "Current", ExternalAccount = "Work", Amount = 56.78, Date = new DateTime(2021, 10, 8) });
            AddRecord(new Transaction { Id = 2, MyAccount = "Savings", ExternalAccount = "Current", Amount = 10.00, Date = new DateTime(2021, 10, 8) });
            AddRecord(new Transaction { Id = 3, MyAccount = "Test", ExternalAccount = "Test", Amount = 100.00, Date = new DateTime(2021, 10, 17) });
            AddRecord(new Transaction { Id = 4, MyAccount = "Test", ExternalAccount = "Test", Amount = 100.00, Date = new DateTime(2021, 10, 17) });
            AddRecord(new Transaction { Id = 5, MyAccount = "Test", ExternalAccount = "Test", Amount = 100.00, Date = new DateTime(2021, 10, 17) });
            AddRecord(new Transaction { Id = 6, MyAccount = "Test", ExternalAccount = "Test", Amount = 100.00, Date = new DateTime(2021, 10, 17) });
            AddRecord(new Transaction { Id = 7, MyAccount = "Test", ExternalAccount = "Test", Amount = 100.00, Date = new DateTime(2021, 10, 17) });
            AddRecord(new Transaction { Id = 8, MyAccount = "Test", ExternalAccount = "Test", Amount = 100.00, Date = new DateTime(2021, 10, 17) });
            AddRecord(new Transaction { Id = 9, MyAccount = "Test", ExternalAccount = "Test", Amount = 100.00, Date = new DateTime(2021, 10, 17) });
            AddRecord(new Transaction { Id = 10, MyAccount = "Test", ExternalAccount = "Test", Amount = 100.00, Date = new DateTime(2021, 10, 17) });
            AddRecord(new Transaction { Id = 11, MyAccount = "Test", ExternalAccount = "Test", Amount = 100.00, Date = new DateTime(2021, 10, 17) });

            tranDate.Date = DateTime.Now;
            foreach (var account in GetMyAccounts())
                MyAccounts.Add(account);
            foreach (var account in GetExAccounts())
                ExAccounts.Add(account);

            LoadPage(0);
        }

        private void tranSubmit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void pageSelectLeft_Click(object sender, RoutedEventArgs e)
        {
            LoadPage(pageNumber - 1);
        }

        private void pageSelectRight_Click(object sender, RoutedEventArgs e)
        {
            LoadPage(pageNumber + 1);
        }

        private void reloadId_Click(object sender, RoutedEventArgs e)
        {
            if (tranId.Text == "")
            {
                tranMyAccount.SelectedIndex = -1;
                tranExternalAccount.SelectedIndex = -1;
                tranAmount.Text = "";
                tranDate.Date = DateTime.Now;
                return;
            }
            bool isValid = int.TryParse(tranId.Text, out int id);
            if (isValid)
            {
                if (TransactionExists(id))
                {
                    Transaction transaction = GetTransaction(id);
                    tranMyAccount.SelectedItem = transaction.MyAccount;
                    tranExternalAccount.SelectedItem = transaction.ExternalAccount;
                    tranAmount.Text = transaction.Amount.ToString();
                    tranDate.Date = transaction.Date;
                }
            }
        }

        private void tranId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tranId.Text == "")
            {
                tranSubmit.Content = "Create";
                reloadId.Content = "clear";
                return;
            }
            else
            {
                reloadId.Content = "reload";
                bool isValid = int.TryParse(tranId.Text, out int id);
                if (isValid)
                {
                    if (TransactionExists(id))
                    {
                        tranSubmit.Content = "Modify";
                        return;
                    }
                }
            }
            tranSubmit.Content = "Invalid Id";
        }
    }
}
