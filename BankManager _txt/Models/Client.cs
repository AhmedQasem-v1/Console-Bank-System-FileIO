namespace BankProject
{
    public class Client
    {
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException($"Name cannot be empty Invalid Name: '{value}'");
                }
                bool hasDigit = value.Any(char.IsDigit);
                if (hasDigit)
                {
                    throw new ArgumentException($"Name cannot contain numbers Invalid Name: '{value}'");

                }
                _name = value;


            }
        }
        private string _id = string.Empty;
        public string Id
        {
            get => _id; 
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException($"ID cannot be empty or whitespace Invalid ID: '{value}'");

                if (value.Length != 6)
                    throw new ArgumentException($"ID must be exactly 6 digits Invalid ID: '{value}'");

                bool hasNonDigit = value.All(c => char.IsDigit(c));

                if (!hasNonDigit)
                    throw new ArgumentException($"ID must contain only digits Invalid ID: '{value}'");

                _id = value;





            }
        }
        private decimal _balance;
        public decimal Balance
        {
            get => _balance; set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Balance cannot be negative");

                }

                _balance = value;

            }
        }
        public string IBAN => $"TR00BANK000000{Id}";

        public Client (string name, string id, decimal Balance)
        {
            this.Name = name;
            this.Id = id;
            this.Balance = Balance;

        }
        public bool Deposit(decimal amount)
        {
            if (amount <= 0) return false;



            Balance += amount;
            Logger.LogTransaction($"Id: {Id}, Name: {Name} | Action: Deposit | Amount: {amount}");
            return true;
        }


        public bool Withdraw(decimal amount)
        {
            //bool canWithdraw = amount <= Balance; تم تغييرلها باعتقادي ان الطريقه المستخدمه افضل واكثر احترافيه 

            if (amount > Balance) return false;


            if (amount <= 0) return false;

            Balance -= amount;

            Logger.LogTransaction($"Id: {Id}, Name: {Name} | Action: Withdraw | Amount: {amount}");

            return true;
        }

        public static Client ToClient(string line)
        {


            string[] parts = line.Split('|');
            if (parts.Length != 4)
            {
                throw new IndexOutOfRangeException("Corrupted line format.");

            }
            string name = parts[0].Trim();
            string id = parts[1].Trim();
            string IBAN = parts[2].Trim();
            // decimal balanceString = decimal.Parse(parts[3].Trim());
            string balanceString = parts[3].Trim();
            if (!decimal.TryParse(balanceString, out var balance))
            {
                throw new FormatException($"Invalid Balance format. Bad value: '{balanceString}'");
            }

            return new Client(name, id, balance);


        }



        public string ToStringFile()
        {
            return $"{Name}|{Id}|{IBAN}|{Balance}";
        }

        public override string ToString()
        {
            return $" Name: {Name} \n ID: {Id} \n IBAN: {IBAN} \n Balance: {Balance:C}";
        }

    }//Client
}
