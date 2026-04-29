using Xunit;

namespace BankProject.Tests
{
    public class ClientTests
    {
        [Theory]
        [InlineData(-100)]
        [InlineData(-999990)]

        public void Balance_WhenNegative_ShouldThrowException(decimal badBalance)
        {
            string name = "Ahmed";
            string id = "123456";


            Action attack = () => new Client(name, id, badBalance);

            Assert.Throws<ArgumentException>(attack);
        }

        [Theory]
        [InlineData("ahmed 24")]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData("123")]
        public void Name_WhenInvalid_ShouldThrowException(string badName)
        {
            string id = "123456";
            decimal Balance = 500;
            Action attack = () => new Client(badName, id, Balance);
            Assert.Throws<ArgumentException>(attack);
        }
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("a123")]
        [InlineData("1234 3")]
        [InlineData("1234 32")]
        [InlineData("1234432")]
        [InlineData("123")]
        public void Id_WhenInvalid_ShouldThrowException(string badId)
        {
            string name = "Ahmed";
            decimal Balance = 500;
            Action attack = () => new Client(name, badId, Balance);
            Assert.Throws<ArgumentException>(attack);


        }
        [Theory]
        [InlineData(450, 50)]
        [InlineData(55.05, 444.95)]
        [InlineData(1.11, 498.89)]

        public void Withdraw_WhenValidAmount_ShouldReturnTrue(decimal withdrawAmount, decimal withdrawBalance)
        {
            Client client = new Client("Ahmed", "123456", 500);
            bool result = client.Withdraw(withdrawAmount);
            Assert.True(result);
            Assert.Equal(withdrawBalance, client.Balance);

        }
        [Theory]
        [InlineData(500.10)]
        [InlineData(1000)]
        [InlineData(0)]
        [InlineData(-100)]
        public void Withdraw_WhenInvalidAmount_ShouldReturnFalse(decimal badBalance)
        {
            Client client = new Client("Ahmed", "123456", 500);
            bool result = client.Withdraw(badBalance);
            Assert.False(result);
            Assert.Equal(500, client.Balance);
        }
        [Theory]
        [InlineData(10)]
        [InlineData(0.50)]
        [InlineData(100.90)]
        public void Deposit_WhenValidAmount_ShouldReturnTrue(decimal depositAmount)
        {
            Client client = new Client("Ahmed", "123321", 500);
            bool result = client.Deposit(depositAmount);
            Assert.True(result);
            Assert.Equal(500 + depositAmount, client.Balance);

        }
        [Theory]
        [InlineData(0)]
        [InlineData(-50)]

        public void Deposit_WhenInvalidAmount_ShouldReturnFalse(decimal badDeposit)
        {
            Client client = new Client("ali", "654321", 500);
            bool isInvalid = client.Deposit(badDeposit);
            Assert.False(isInvalid);
            Assert.Equal(500, client.Balance);

        }
    }
}