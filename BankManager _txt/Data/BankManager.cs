namespace BankProject
{
    public class BankManager
    {
        public string bankName { get; private set; }
        private List<Client> Clients = new List<Client>();
        private List<Client> _undoDelete = new List<Client>();
        private List<string> LoadingErrors = new List<string>();
        private readonly string _bankNameFile;

        public BankManager(string bankName, string bankNameFile)
        {
            this.bankName = bankName;
            _bankNameFile = bankNameFile;
            LoadData();

        }

        private void LoadData()
        {
            
            if (File.Exists(_bankNameFile))
            {
                int lineNumber = 0; 

                try
                {
                    
                    string[] lines = File.ReadAllLines(_bankNameFile);

                    foreach (string line in lines)
                    {
                        lineNumber++; 
                        if (string.IsNullOrWhiteSpace(line))
                            continue;
                        
                        try
                        {
                            
                            Client c = Client.ToClient(line);



                            if (c != null)
                            { 
                                Client? originalClient = Clients.FirstOrDefault(existing => existing.Id == c.Id);

                                if (originalClient != null) 
                                {
                                    LoadingErrors.Add($"Critical Error Line {lineNumber}: Client '{c.Name}' has a duplicate ID '{c.Id}' which is already registered to '{originalClient.Name} with this id :{originalClient.Id} '!");
                                }
                                else 
                                {
                                    Clients.Add(c); 
                                }

                            }
                        }

                        catch (Exception ex)
                        {
                            LoadingErrors.Add($"Critical Error Line {lineNumber}: Unexpected error - {ex.Message}");
                        }
                    }
                    if (LoadingErrors.Count > 0)
                    {

                        string allErrors = string.Join("\n", LoadingErrors);
                        throw new InvalidDataException($"Found {LoadingErrors.Count} corrupted lines:\n{allErrors}");
                    }
                }

                catch (IOException ex)
                {
                    LoadingErrors.Add($"Fatal Error: Could not read file. {ex.Message}");
                }
            }
        }
        private void saveClient(Client forClient)
        {



            using (StreamWriter sw = new StreamWriter(_bankNameFile, append: true))
            {
                sw.WriteLine(forClient.ToStringFile());
            }


        }
        private void RewriteFile()
        {


            using (StreamWriter sw = new StreamWriter(_bankNameFile, append: false))
            {
                foreach (Client c in Clients)
                {
                    sw.WriteLine(c.ToStringFile());
                }
            }



        }
        public bool AddClient(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client", "Client cannot be null.");
            }
            bool isDuplicat = Clients.Any(c => c.Id == client.Id);
            if (isDuplicat)
            {

                throw new InvalidOperationException($"Security Alert: A client with ID '{client.Id}' already exists in the system!");
            }
            saveClient(client);
            Clients.Add(client);


            Logger.LogTransaction($"[ADD] Client: {client.Name} | ID: {client.Id} | Balance: {client.Balance}");

            return true;

        }

        public bool RemoveClient(Client Delete)
        {


            if (Delete == null)
            {
                throw new ArgumentNullException(nameof(Delete), "Client Is Null  ");

            }
            bool result = Clients.Remove(Delete);
            if (result)
            {
                RewriteFile();
                _undoDelete.Add(Delete);

                Logger.LogTransaction($"[DELETE] Client: {Delete.Name} | ID: {Delete.Id} | Balance: {Delete.Balance}");
            }
            return result;
        }

        public bool WithdrawManager(string clientId, decimal amount, out string message)
        {
            var client = Clients.FirstOrDefault(x => x.Id == clientId);

            if (client == null)
            {
                message = "Error: Client not found.";
                return false;
            }
            if (amount <= 0)
            {
                message = "Error: Please enter an amount greater than zero.";
                return false;
            }
            if (amount > client.Balance)
            {
                message = $"Error: Insufficient funds. Current balance is {client.Balance:C}.";
                return false;
            }
            client.Withdraw(amount);
            RewriteFile();
            message = "Withdrawal completed successfully and saved.";
            return true;
        }

        public bool DepositManager(string clientId, decimal amount, out string message)
        {
            var client = Clients.Find(x => x.Id == clientId);
            if (client == null)
            {
                message = "Error: Client not found.";
                return false;
            }
            if (amount <= 0)
            {
                message = "Error: Please enter an amount greater than zero.";
                return false;
            }
            client.Deposit(amount);
            RewriteFile();
            message = "Deposit completed successfully and saved.";
            return true;


        }
        public bool UndoDelete(Client lastDelete)
        {
            if (lastDelete == null) return false;

            if (_undoDelete.Contains(lastDelete))
            {
                Clients.Add(lastDelete);
                _undoDelete.Remove(lastDelete);
                Logger.LogTransaction($"[UNDO] Client Restored: {lastDelete.Name}");

                return true;
            }

            return false;
        }
        public Client? searchClient(string searchId)
        {
            return Clients.FirstOrDefault(c => c.Id == searchId);

        }

        public List<Client> PrintAllClients()
        {

            return Clients;
        }
        public List<Client> GetRichestClient()
        {
            List<Client> ret = new List<Client>();
            if (Clients.Count == 0)
            {
                return ret;

            }
            decimal maxBalance = Clients[0].Balance;
            foreach (Client c in Clients)
            {
                if (c.Balance > maxBalance)
                {
                    maxBalance = c.Balance;
                    ret.Clear();
                    ret.Add(c);
                }
                else if (c.Balance == maxBalance)
                {
                    ret.Add(c);

                }
            }

            return ret;

        }
        public List<Client> SortClients()
        {

            var tempClient = new List<Client>(Clients);
            if (Clients.Count == 0)
            {
                return tempClient;
            }
            for (var i = 0; i < tempClient.Count; i++)
            {
                for (var j = 0; j < i + 1; j++)
                {
                    if (tempClient[i].Balance > tempClient[j].Balance)
                    {
                        var temp = tempClient[i];
                        tempClient[i] = tempClient[j];
                        tempClient[j] = temp;
                    }


                }
            }

            return tempClient;

        }

        public bool Transfer(string senderId, string recipientId, decimal amount, out string message)
        {

            var sender = Clients.FirstOrDefault(c => c.Id == senderId);
            if (sender == null)
            {
                message = "Sender not found.";
                return false;
            }
            var recipient = Clients.FirstOrDefault(c => c.Id == recipientId);
            if (recipient == null)
            {
                message = " recipient not found ";
                return false;
            }
            if (sender == recipient)
            {
                message = " Sender and recipient cannot be the same.";
                return false;
            }
            if (amount <= 0)
            {
                message = "Transfer amount must be positive.";
                return false;
            }
            if (sender.Balance < amount)
            {
                message = "Insufficient funds for transfer.";
                return false;
            }

            sender.Withdraw(amount);
            recipient.Deposit(amount);
            RewriteFile();
            message = $"✅ Transfer Successful: {amount:C} has been sent to {recipient.Name}.";
            Logger.LogTransaction($"[TRANSFER] From: {sender.Name} ({senderId}) To: {recipient.Name} ({recipientId}) | Amount: {amount}");

            return true;
        }

        public bool AmendmentName(string id, string newName)
        {
            Client? c = Clients.Find(x => x.Id == id);
            if (c != null)
            {
                c.Name = newName;
                RewriteFile();
                return true;


            }
            return false;

        }
        public bool AmendmentBalance(string id, decimal newBalance)
        {
            if (newBalance < 0) return false;
            Client? c = Clients.Find(x => x.Id == id);

            if (c != null)
            {
                c.Balance = newBalance;
                RewriteFile();
                return true;
            }
            return false;

        }
        public bool AmendmentId(string id, string newId)
        {
            Client? c = Clients.Find(x => x.Id == id);
            if (c != null)
            {
                c.Id = newId;
                RewriteFile();
                return true;

            }
            return false;
        }

    }
}
