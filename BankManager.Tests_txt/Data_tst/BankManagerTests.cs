using Xunit;


namespace BankProject.Tests
{
    public class BankManagerTests
    {
        [Fact]
        public void AddClient_WhenValidClient_ShouldReturnTrueAndIncreaseCount()
        {
            BankManager manager = new BankManager("TestBank", "testbank.txt");
            var client = new Client("ahmed", "123456", 1000);
            bool result = manager.AddClient(client);
            Assert.True(result);
            Assert.Single(manager.PrintAllClients());
            if (File.Exists("testbank.txt"))
            {
                File.Delete("testbank.txt");
            }
        }
        [Fact]
        public void AddClient_WhenIdIsDuplicate_ShouldThrowException()
        {
            BankManager manager = new BankManager("TestBank", "Tastbank.txt");
            Client client1 = new Client("ahmed", "123456", 1000);
            Client client2 = new Client("salah", "123456", 2000);
            manager.AddClient(client1);
            Assert.Throws<InvalidOperationException>(() => manager.AddClient(client2));
            if (File.Exists("Tastbank.txt"))
            {
                File.Delete("Tastbank.txt");
            }

        }
        [Fact]
        public void RemoveClient_WhenValidClient_ShouldReturnTrueAndEmptyList()
        {
            BankManager manager = new BankManager("alyaseen", "alyaseenTest.txt");
            Client client = new Client("alya", "123123", 1200);
            manager.AddClient(client);
            bool result = manager.RemoveClient(client);
            Assert.True(result);
            Assert.Empty(manager.PrintAllClients());
            if (File.Exists("alyaseenTest.txt")) File.Delete("alyaseenTest.txt");
        }
        [Fact]
        public void RemoveClient_WhenClientNotInList_ShouldReturnFalse()
        {
            BankManager manager = new BankManager("alyassen ", "alyassenTest.txt");
            Client client = new Client("salah", "321321", 1000);
            bool result = manager.RemoveClient(client);
            Assert.False(result);

            if (File.Exists("alyassenTest.txt")) File.Delete("alyassenTest.txt");




        }
        [Fact]
        public void RemoveClient_WhenClientIsNull_ShouldThrowException()
        {
            BankManager manager = new BankManager("alyasen", "alyasenTest.txt");

            Assert.Throws<ArgumentNullException>(() => manager.RemoveClient(null!));
            if (File.Exists("alyasenTest.txt")) File.Delete("alyasenTest.txt");


        }
        [Fact]
        public void SearchClient_WhenClientExists_ShouldReturnClient()
        {
            BankManager manager = new BankManager("alyasseen", "alyasenTest.txt");
            var client = new Client("salah", "123321", 1500);
            manager.AddClient(client);

            Client? result = manager.searchClient(client.Id);
            Assert.NotNull(result);
            Assert.Equal("123321", (result.Id));
            if (File.Exists("alyasenTest.txt")) File.Delete("alyasenTest.txt");
        }
        [Fact]
        public void SearchClient_WhenClientDoesNotExist_ShouldReturnNull()
        {
            BankManager manager = new BankManager("alyasseen", "alyasenTest.txt");
            var client = manager.searchClient("nonexistentId");
            Assert.Null(client);
            if (File.Exists("alyasenTest.txt")) File.Delete("alyasenTest.txt");

        }
        [Fact]
        public void UndoDelete_WhenClientWasDeleted_ShouldRestoreClientTest_1()
        {
            BankManager manager = new BankManager("Test_1", "Test_1.txt");
            Client client = new Client("salah", "123321", 1500);
            manager.AddClient(client);
            manager.RemoveClient(client);
            bool result = manager.UndoDelete(client);
            Assert.True(result);
            Assert.Single(manager.PrintAllClients());
            Assert.NotNull(manager.searchClient("123321"));
            if (File.Exists("Test_1.txt")) File.Delete("Test_1.txt");

        }
        [Fact]
        public void UndoDelete_WhenClientWasDeleted_ShouldRestoreClientTest_2()
        {
            BankManager manager = new BankManager("Test_2", "Test_2.txt");
            Client client = new Client("salah", "123321", 1500);
            manager.AddClient(client);
            bool result = manager.UndoDelete(client);
            Assert.False(result);
            Assert.Single(manager.PrintAllClients());
            Assert.NotNull(manager.searchClient("123321"));
            if (File.Exists("Test_2.txt")) File.Delete("Test_2.txt");

        }
        [Fact]
        public void UndoDelete_WhenClientWasDeleted_ShouldRestoreClientTest_3()
        {
            BankManager manager = new BankManager("Test_3", "Test_3.txt");
            bool result = manager.UndoDelete(null!);
            Assert.False(result);
            Assert.Empty(manager.PrintAllClients());


            if (File.Exists("Test_3.txt")) File.Delete("Test_3.txt");
        }
        [Fact]
        public void WithdrawManager_WhenValid_ShouldDecreaseBalance()
        {
            BankManager manager = new BankManager("WithdrawTest", "WithdrawTest.txt");
            string message;
            var client = new Client("salah", "123321", 1500);
            manager.AddClient(client);
            bool result = manager.WithdrawManager("123321", 500, out message);
            Assert.True(result);
            Assert.Equal(1000, manager.searchClient("123321")!.Balance);
            Assert.Contains("successfully", message, StringComparison.OrdinalIgnoreCase);
            if (File.Exists("WithdrawTest.txt")) File.Delete("WithdrawTest.txt");
        }
        [Fact]
        public void WithdrawManager_WhenInsufficientFunds_ShouldReturnFalse()
        {
            BankManager manager = new BankManager("WithdrawTest2", "WithdrawTest2.txt");
            Client client = new Client("salah", "123321", 1500);
            manager.AddClient(client);
            bool result = manager.WithdrawManager("123321", 5000, out string message);
            Assert.False(result);
            Assert.Equal(1500, manager.searchClient("123321")!.Balance);
            Assert.Contains("insufficient funds", message, StringComparison.OrdinalIgnoreCase);
            if (File.Exists("WithdrawTest2.txt")) File.Delete("WithdrawTest2.txt");

        }
        [Fact]
        public void WithdrawManager_WhenClientNotFound_ShouldReturnFalse()
        {
            BankManager manager = new BankManager("WithdrawTest3", "WithdrawTest3.txt");

            bool result = manager.WithdrawManager(null!, 500, out string message);
            Assert.False(result);
            Assert.Empty(manager.PrintAllClients());
            Assert.Contains("client not found", message, StringComparison.OrdinalIgnoreCase);
            if (File.Exists("WithdrawTest3.txt")) File.Delete("WithdrawTest3.txt");

        }
        [Theory]
        [InlineData(-100)]
        [InlineData(0)]
        [InlineData(-1)]
        public void WithdrawManager_WhenAmountIsInvalid_ShouldReturnFalse(decimal badBalance)
        {
            BankManager manager = new BankManager("WithdrawTest4", "WithdrawTest4.txt");
            Client client = new Client("salah", "123321", 1500);
            manager.AddClient(client);
            bool result = manager.WithdrawManager("123321", badBalance, out string message);
            Assert.False(result);
            Assert.Equal(1500, manager.searchClient("123321")!.Balance);
            Assert.Contains("Please enter an amount greater than zero", message, StringComparison.OrdinalIgnoreCase);
            if (File.Exists("WithdrawTest4.txt")) File.Delete("WithdrawTest4.txt");
        }

        [Fact]
        public void DepositManager_WhenSuccssful_ReturnTrue()
        {
            BankManager manager = new BankManager("DepositManager", "DepositManager.txt");
            Client client = new Client("soso", "123456", 4500);
            manager.AddClient(client);
            bool result = manager.DepositManager("123456", 2000, out string msg);
            Assert.True(result);
            Assert.Equal(6500, manager.searchClient("123456")!.Balance);
            Assert.Contains("successfully and saved", msg, StringComparison.OrdinalIgnoreCase);
            if (File.Exists("DepositManager.txt")) File.Delete("DepositManager.txt");
        }
        [Fact]
        public void DepositManager_WhenClientNotFound_ReturnFalse()
        {
            BankManager manager = new BankManager("DepositManager", "DepositManager1.txt");
            bool result = manager.DepositManager("null or eny think", 2000, out string msg);
            Assert.False(result);
            Assert.Empty(manager.PrintAllClients());
            Assert.Contains("Client not found", msg, StringComparison.OrdinalIgnoreCase);
            if (File.Exists("DepositManager1.txt")) File.Delete("DepositManager1.txt");
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-10)]

        public void DepoditManager_Whenzero_Or_Negative_ReturnFalse(decimal badBalance)
        {
            BankManager manager = new BankManager("DepoditManager", "DepoditManager2.txt");
            Client client = new Client("farah", "456456", 1000);
            manager.AddClient(client);
            bool result = manager.DepositManager("456456", badBalance, out string msg);
            Assert.False(result);
            Assert.Equal(1000, manager.searchClient("456456")!.Balance);
            Assert.Contains("Please enter an amount greater than zero", msg, StringComparison.OrdinalIgnoreCase);
            if (File.Exists("DepoditManager2.txt")) File.Delete("DepoditManager2.txt");
        }

        [Fact]
        public void Transfer_WhenSuccessful_ShouldReturnTrueAndUpdateBalances()
        {
            BankManager manager = new BankManager("TransferTest", "TransferTest.txt");
            Client sender = new Client("salah", "123321", 1500);
            Client recipient = new Client("ahmed", "654321", 1000);
            manager.AddClient(sender);
            manager.AddClient(recipient);
            bool result = manager.Transfer("123321", "654321", 500, out string msg);
            Assert.True(result);
            Assert.Equal(1000, manager.searchClient("123321")!.Balance);
            Assert.Equal(1500, manager.searchClient("654321")!.Balance);
            Assert.Contains("successful", msg, StringComparison.OrdinalIgnoreCase);
            if (File.Exists("TransferTest.txt")) File.Delete("TransferTest.txt");

        }
        [Fact]
        public void Transfer_WhenSenderNotFound_ShouldReturnFalse()
        {
            BankManager manager = new BankManager("TransferTest99", "TransferTest9.txt");
            Client recipient = new Client("ahmed", "545321", 1000);
            manager.AddClient(recipient);
            bool result = manager.Transfer("nonexistentId", "654321", 500, out string msg);
            Assert.False(result);
            Assert.Equal(1000, manager.searchClient("545321")!.Balance);
            Assert.Contains("sender not found", msg, StringComparison.OrdinalIgnoreCase);
            if (File.Exists("TransferTest99.txt")) File.Delete("TransferTest9.txt");
        }
        [Fact]
        public void Transfer_WhenRecipientNotFound_ShouldReturnFalse()
        {
            BankManager manager = new BankManager("TransferTest3", "TransferTest3.txt");
            Client sender = new Client("salah", "123321", 1500);
            manager.AddClient(sender);
            bool result = manager.Transfer("123321", "nonexistentId", 500, out string msg);
            Assert.False(result);
            Assert.Equal(1500, manager.searchClient("123321")!.Balance);
            Assert.Contains("recipient not found", msg, StringComparison.OrdinalIgnoreCase);
            if (File.Exists("TransferTest3.txt")) File.Delete("TransferTest3.txt");

        }
        [Fact]
        public void Transfer_WhenSender_EqualsRecipient_ShouldReturnFalse()
        {
            BankManager manager = new BankManager("TransferTest4", "TransferTest4.txt");
            Client sender = new Client("salah", "123321", 1500);
            manager.AddClient(sender);
            bool result = manager.Transfer("123321", "123321", 500, out string msg);
            Assert.False(result);
            Assert.Equal(1500, manager.searchClient("123321")!.Balance);
            Assert.Contains("Sender and recipient cannot be the same", msg, StringComparison.OrdinalIgnoreCase);
            if (File.Exists("TransferTest4.txt")) File.Delete("TransferTest4.txt");


        }
        [Fact]
        public void Transfer_WhenAmount_BiggerThanSenderBalance_ShouldReturnFalse()
        {
            BankManager manager = new BankManager("TransferTest5", "TransferTest5.txt");
            Client sender = new Client("salah", "123321", 1500);
            Client recipient = new Client("ahmed", "654321", 1000);
            manager.AddClient(sender);
            manager.AddClient(recipient);
            bool result = manager.Transfer("123321", "654321", 2000, out string msg);
            Assert.False(result);
            Assert.Equal(1500, manager.searchClient("123321")!.Balance);
            Assert.Equal(1000, manager.searchClient("654321")!.Balance);
            Assert.Contains("insufficient funds", msg, StringComparison.OrdinalIgnoreCase);
            if (File.Exists("TransferTest5.txt")) File.Delete("TransferTest5.txt");

        }
        [Theory]
        [InlineData(0)]
        [InlineData(-50)]
        public void Transfer_WhenAmountIsInvalid_ShouldReturnFalse(decimal badBalance)
        {
            BankManager manager = new BankManager("TransferTest6", "TransferTest6.txt");
            Client sender = new Client("salah", "123321", 1500);
            Client recipient = new Client("ahmed", "654321", 1000);
            manager.AddClient(sender);
            manager.AddClient(recipient);
            bool result = manager.Transfer("123321", "654321", badBalance, out string msg);
            Assert.False(result);
            Assert.Equal(1500, manager.searchClient("123321")!.Balance);
            Assert.Equal(1000, manager.searchClient("654321")!.Balance);
            Assert.Contains("Transfer amount must be positive", msg, StringComparison.OrdinalIgnoreCase);
            if (File.Exists("TransferTest6.txt")) File.Delete("TransferTest6.txt");
        }
        [Fact]
        public void AmendmentName_WhenValid_ShouldUpdateName_ReturnTrue()
        {
            BankManager manager = new BankManager("AmendmentTest", "AmendmentTest.txt");
            Client client = new Client("salah", "123321", 1500);
            manager.AddClient(client);
            bool result = manager.AmendmentName("123321", "ali");
            Assert.True(result);
            Assert.Equal("ali", manager.searchClient("123321")!.Name);
            if (File.Exists("AmendmentTest.txt")) File.Delete("AmendmentTest.txt");

        }
        [Fact]
        public void AmendmentName_WhenClientNotFound_ShouldReturnFalse()
        {
            BankManager manager = new BankManager("AmendmentTest2", "AmendmentTest2.txt");
            bool result = manager.AmendmentName("111111", "123321");
            Assert.False(result);
            Assert.Empty(manager.PrintAllClients());
            if (File.Exists("AmendmentTest2.txt")) File.Delete("AmendmentTest2.txt");
        }
        [Fact]
        public void AmendmentBalance_WhenValid_ShouldUpdateBalance_ReturnTrue()
        {
            BankManager manager = new BankManager("AmendmentTest3", "AmendmentTest3.txt");
            Client client = new Client("salah", "123321", 1500);
            manager.AddClient(client);
            bool result = manager.AmendmentBalance("123321", 2000);
            Assert.True(result);
            Assert.Equal(2000, manager.searchClient("123321")!.Balance);
            if (File.Exists("AmendmentTest3.txt")) File.Delete("AmendmentTest3.txt");
        }
        [Fact]
        public void AmendmentBalance_WhenClientNotFound_ShouldReturnFalse()
        {
            BankManager manager = new BankManager("AmendmentTest4", "AmendmentTest4.txt");
            bool result = manager.AmendmentBalance("222222", 1200);
            Assert.False(result);
            Assert.Empty(manager.PrintAllClients());
            if (File.Exists("AmendmentTest4.txt")) File.Delete("AmendmentTest4.txt");

        }
        [Fact]
        public void AmendmentBalance_WhenNegative_ShouldReturnFalse()
        {
            BankManager manager = new BankManager("AmendmentTest7", "AmendmentTest7.txt");
            Client client = new Client("salah", "123321", 1500);
            manager.AddClient(client);
            bool result = manager.AmendmentBalance("123321", -500);
            Assert.False(result);
            Assert.Equal(1500, manager.searchClient("123321")!.Balance);
            if (File.Exists("AmendmentTest7.txt")) File.Delete("AmendmentTest7.txt");
        }
        [Fact]
        public void AmendmentId_WhenValid_ShouldUpdateId_ReturnTrue()
        {
            BankManager manager = new BankManager("AmendmentTest5", "AmendmentTest5.txt");
            Client client = new Client("salah", "123321", 1500);
            manager.AddClient(client);
            bool result = manager.AmendmentId("123321", "654321");
            Assert.True(result);
            Assert.Equal("654321", manager.searchClient("654321")!.Id);
            if (File.Exists("AmendmentTest5.txt")) File.Delete("AmendmentTest5.txt");
        }
        [Fact]
        public void AmendmentId_WhenClientNotFound_ShouldReturnFalse()
        {
            BankManager manager = new BankManager("AmendmentTest6", "AmendmentTest6.txt");
            bool result = manager.AmendmentId("333333", "123123");
            Assert.False(result);
            Assert.Empty(manager.PrintAllClients());
            if (File.Exists("AmendmentTest6.txt")) File.Delete("AmendmentTest6.txt");
        }
        [Fact]
        public void PrintAllClients_WhenClientEmpty_ShouldReturn_EmptyList()
        {
            BankManager manager = new BankManager("PrintTest", "PrintTest.txt");
            List<Client> result = manager.PrintAllClients();
            Assert.Empty(result);

            if (File.Exists("PrintTest.txt")) File.Delete("PrintTest.txt");
        }
        [Fact]
        public void PrintAllClients_WhenClientsFound_ShouldReturn_List()
        {
            BankManager manager = new BankManager("PrintTest2", "PrintTest2.txt");
            Client client1 = new Client("ali", "456654", 1200);
            Client client2 = new Client("ali", "789987", 1200);
            manager.AddClient(client1);
            manager.AddClient(client2);
            List<Client> result = manager.PrintAllClients();
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            if (File.Exists("PrintTest2.txt")) File.Delete("PrintTest2.txt");


        }
        [Fact]
        public void GetRichestClient_WhenClientFound_ShouldReturn_Client()
        {
            BankManager manager = new BankManager("RichestTest", "RichestTest.txt");
            manager.AddClient(new Client("ali", "456654", 1200));
            manager.AddClient(new Client("salah", "789987", 1500));
            manager.AddClient(new Client("ahmed", "321321", 1500));
            manager.AddClient(new Client("farah", "654654", 1000));
            manager.AddClient(new Client("mohamed", "987987", 1499.98m));
            List<Client> result = manager.GetRichestClient();
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(1500m, result[0].Balance);
            Assert.Equal(1500m, result[1].Balance);
            if (File.Exists("RichestTest.txt")) File.Delete("RichestTest.txt");


        }

        [Fact]
        public void GetRichestClient_WhenNoClients_ShouldReturnEmptyList()
        {
            BankManager manager = new BankManager("GetRichestClient Test", "RichestClient.txt");
            List<Client> result = manager.GetRichestClient();
            Assert.Empty(result);
            if (File.Exists("RichestClient.txt")) File.Delete("RichestClient.txt");

        }

        [Fact]
        public void SortClients_WhenClientsFound_ShouldReturn_SortedList()
        {
            BankManager manager = new BankManager("SortTest", "SortTest.txt");
            manager.AddClient(new Client("ali", "456654", 1200));
            manager.AddClient(new Client("salah", "789987", 1500));
            manager.AddClient(new Client("ahmed", "321321", 1500));
            manager.AddClient(new Client("farah", "654654", 1000));
            manager.AddClient(new Client("mohamed", "987987", 1499.98m));
            manager.AddClient(new Client("zaki", "111111", 1499.99m));
            List<Client> result = manager.SortClients();
            Assert.NotEmpty(result);
            Assert.Equal(6, result.Count);
            Assert.Equal(1500, result[0].Balance);
            Assert.Equal(1500, result[1].Balance);
            Assert.Equal(1499.99m, result[2].Balance);
            Assert.Equal(1499.98m, result[3].Balance);
            Assert.Equal(1200, result[4].Balance);
            Assert.Equal(1000, result[5].Balance);
            if (File.Exists("SortTest.txt")) File.Delete("SortTest.txt");

        }

        [Fact]
        public void SortClients_WhenNoClients_ShouldReturnEmptyList()
        {
            BankManager manager = new BankManager("SortTest2", "SortTest2.txt");
            List<Client> result = manager.SortClients();
            Assert.Empty(result);
            if (File.Exists("SortTest2.txt")) File.Delete("SortTest2.txt");

        }

        [Fact]
        public void file_that_Stores_Error_Lines_Will_Returna_exception()
        {
            File.WriteAllLines("BadFile.txt", new string[] {"Ahmed|123456|TR001|1220",
            "ali|ad1234|TR002|1200",
            "salah|123554|TR003|a10",
            "|122654|TR004|1200",
            "a12|123354|TR005|10"});

            var exception = Assert.Throws<InvalidDataException>(() => new BankManager("BadFile", "BadFile.txt"));


            Assert.Contains("a12", exception.Message);
            Assert.Contains("ad1234", exception.Message);
            Assert.Contains("a10", exception.Message);
            Assert.Contains("", exception.Message);
            Assert.DoesNotContain("Ahmed", exception.Message);
            if (File.Exists("BadFile.txt")) File.Delete("BadFile.txt");

        }



    }
}