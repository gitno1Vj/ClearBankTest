using ClearBank.DeveloperTest.Helpers;
using ClearBank.DeveloperTest.Interfaces;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FizzWare.NBuilder;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentServiceTests
    {
        private readonly Mock<IConfigHelper> _configHelper;
        private readonly Mock<IDataStore> _dataStore;
        private readonly Mock<IDataStoreHelper> _dataStoreHelper;
        private readonly Mock<ICalculatorService> _calculatorService;
        private readonly PaymentsValidatorHelper _paymentsValidatorHelper;


        public PaymentServiceTests()
        {
            _configHelper = new Mock<IConfigHelper>();
            _dataStore = new Mock<IDataStore>();
            _dataStoreHelper = new Mock<IDataStoreHelper>();
            _calculatorService = new Mock<ICalculatorService>();

            _paymentsValidatorHelper = new PaymentsValidatorHelper();
        }

        #region Bacs Scheme Tests

        [Fact]
        public void MakePayment_Bacs_Valid_But_AccountDoesNotExist_Return_False()
        {
            //arrange
            //No Account
            _dataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => null);
            _dataStoreHelper.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_dataStore.Object);
            _calculatorService.Setup(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()));

            var paymentService = new PaymentService(new AccountService(_dataStoreHelper.Object, _configHelper.Object),
                _paymentsValidatorHelper, _calculatorService.Object);

            MakePaymentRequest paymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.Bacs)
                .Build();

            //act
            var result = paymentService.MakePayment(paymentRequest);

            //assert
            _configHelper.Verify(x => x.DataStoreType, Times.Exactly(1));
            _dataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _dataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Never);

            Assert.False(result.Success);
        }

        [Fact]
        public void MakePayment_Bacs_InValid_And_AccountExist_Return_False()
        {
            // arrange
            var account = Builder<Account>.CreateNew()
                .With(x => x.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps)
                .With(x => x.Balance = 2000)
                .Build();

            _dataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => account);
            _dataStoreHelper.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_dataStore.Object);
            _calculatorService.Setup(x => x.DeductAmountFromAccount(account, It.IsAny<decimal>()));

            var paymentService = new PaymentService(new AccountService(_dataStoreHelper.Object, _configHelper.Object),
               _paymentsValidatorHelper, _calculatorService.Object);

            MakePaymentRequest paymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.Bacs)
                .Build();

            //act
            var result = paymentService.MakePayment(paymentRequest);

            //assert
            _configHelper.Verify(x => x.DataStoreType, Times.Exactly(1));
            _dataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _dataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Never);
            Assert.False(result.Success);
            Assert.Equal(2000, account.Balance);
        }

        [Fact]
        public void MakePayment_Bacs_Valid_And_AccountExist_Return_True_And_Update_Balance()
        {
            //arrange
            var account = Builder<Account>.CreateNew()
                .With(x => x.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs)
                .With(x => x.Balance = 2000)
                .Build();

            _dataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => account);
            _dataStoreHelper.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_dataStore.Object);

            MakePaymentRequest makePaymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.Bacs)
                .With(x => x.Amount = 1000)
                .Build();

            _calculatorService.Setup(x => x.DeductAmountFromAccount(account, It.IsAny<decimal>()))
                .Callback(() => account.Balance -= makePaymentRequest.Amount);

            var paymentService = new PaymentService(new AccountService(_dataStoreHelper.Object, _configHelper.Object),
              _paymentsValidatorHelper, _calculatorService.Object);

            // Act
            var result = paymentService.MakePayment(makePaymentRequest);

            // Assert
            _configHelper.Verify(x => x.DataStoreType, Times.Exactly(2));
            _dataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _dataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Once);
            Assert.True(result.Success);
            Assert.Equal(1000, 1000);
        }

        #endregion

        #region FasterPayments Scheme Tests

        [Fact]
        public void MakePayment_FasterPayments_Valid_But_AccountDoesNotExist_Return_False()
        {
            //arrange
            //No Account
            _dataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => null);
            _dataStoreHelper.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_dataStore.Object);
            _calculatorService.Setup(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()));

            var paymentService = new PaymentService(new AccountService(_dataStoreHelper.Object, _configHelper.Object),
                _paymentsValidatorHelper, _calculatorService.Object);

            MakePaymentRequest paymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.FasterPayments)
                .Build();

            //act
            var result = paymentService.MakePayment(paymentRequest);

            //assert
            _configHelper.Verify(x => x.DataStoreType, Times.Exactly(1));
            _dataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _dataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Never);

            Assert.False(result.Success);
        }

        [Fact]
        public void MakePayment_FasterPayments_InValid_And_AccountExist_Return_False()
        {
            // arrange
            var account = Builder<Account>.CreateNew()
                .With(x => x.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps)
                .With(x => x.Balance = 2000)
                .Build();

            _dataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => account);
            _dataStoreHelper.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_dataStore.Object);
            _calculatorService.Setup(x => x.DeductAmountFromAccount(account, It.IsAny<decimal>()));

            var paymentService = new PaymentService(new AccountService(_dataStoreHelper.Object, _configHelper.Object),
               _paymentsValidatorHelper, _calculatorService.Object);

            MakePaymentRequest paymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.FasterPayments)
                .Build();

            //act
            var result = paymentService.MakePayment(paymentRequest);

            //assert
            _configHelper.Verify(x => x.DataStoreType, Times.Exactly(1));
            _dataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _dataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Never);
            Assert.False(result.Success);
            Assert.Equal(2000, account.Balance);
        }

        [Fact]
        public void MakePayment_FasterPayments_Valid_And_AccountExist_Return_True_And_Update_Balance()
        {
            //arrange
            var account = Builder<Account>.CreateNew()
                .With(x => x.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments)
                .With(x => x.Balance = 2000)
                .Build();

            _dataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => account);
            _dataStoreHelper.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_dataStore.Object);

            MakePaymentRequest makePaymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.FasterPayments)
                .With(x => x.Amount = 1000)
                .Build();

            _calculatorService.Setup(x => x.DeductAmountFromAccount(account, It.IsAny<decimal>()))
                .Callback(() => account.Balance -= makePaymentRequest.Amount);

            var paymentService = new PaymentService(new AccountService(_dataStoreHelper.Object, _configHelper.Object),
              _paymentsValidatorHelper, _calculatorService.Object);

            // Act
            var result = paymentService.MakePayment(makePaymentRequest);

            // Assert
            _configHelper.Verify(x => x.DataStoreType, Times.Exactly(2));
            _dataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _dataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Once);
            Assert.True(result.Success);
            Assert.Equal(1000, 1000);
        }

        [Fact]
        public void MakePayment_FasterPayments_Valid_And_AccountExist_But_Balance_LessThan_RequestAmount_Return_False_And_No_Update_Balance()
        {
            //arrange
            var account = Builder<Account>.CreateNew()
                .With(x => x.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments)
                .With(x => x.Balance = 1000)
                .Build();

            _dataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => account);
            _dataStoreHelper.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_dataStore.Object);

            MakePaymentRequest makePaymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.FasterPayments)
                .With(x => x.Amount = 2000)
                .Build();

            _calculatorService.Setup(x => x.DeductAmountFromAccount(account, It.IsAny<decimal>()))
                .Callback(() => account.Balance -= makePaymentRequest.Amount);

            var paymentService = new PaymentService(new AccountService(_dataStoreHelper.Object, _configHelper.Object),
              _paymentsValidatorHelper, _calculatorService.Object);

            // Act
            var result = paymentService.MakePayment(makePaymentRequest);

            // Assert
            _configHelper.Verify(x => x.DataStoreType, Times.Exactly(1));
            _dataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _dataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Never);
            Assert.False(result.Success);
            Assert.Equal(1000, account.Balance);
        }

        #endregion

        #region Chaps Scheme Tests

        [Fact]
        public void MakePayment_Chaps_Valid_But_AccountDoesNotExist_Return_False()
        {
            //arrange
            //No Account
            _dataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => null);
            _dataStoreHelper.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_dataStore.Object);
            _calculatorService.Setup(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()));

            var paymentService = new PaymentService(new AccountService(_dataStoreHelper.Object, _configHelper.Object),
                _paymentsValidatorHelper, _calculatorService.Object);

            MakePaymentRequest paymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.Chaps)
                .Build();

            //act
            var result = paymentService.MakePayment(paymentRequest);

            //assert
            _configHelper.Verify(x => x.DataStoreType, Times.Exactly(1));
            _dataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _dataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Never);

            Assert.False(result.Success);
        }

        [Fact]
        public void MakePayment_Chaps_InValid_And_AccountExist_Return_False()
        {
            // arrange
            var account = Builder<Account>.CreateNew()
                .With(x => x.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments)
                .With(x => x.Balance = 2000)
                .Build();

            _dataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => account);
            _dataStoreHelper.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_dataStore.Object);
            _calculatorService.Setup(x => x.DeductAmountFromAccount(account, It.IsAny<decimal>()));

            var paymentService = new PaymentService(new AccountService(_dataStoreHelper.Object, _configHelper.Object),
               _paymentsValidatorHelper, _calculatorService.Object);

            MakePaymentRequest paymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.Chaps)
                .Build();

            //act
            var result = paymentService.MakePayment(paymentRequest);

            //assert
            _configHelper.Verify(x => x.DataStoreType, Times.Exactly(1));
            _dataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _dataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Never);
            Assert.False(result.Success);
            Assert.Equal(2000, account.Balance);
        }

        [Fact]
        public void MakePayment_Chaps_Valid_And_AccountExist_Return_True_And_Update_Balance()
        {
            //arrange
            var account = Builder<Account>.CreateNew()
                .With(x => x.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps)
                .With(x => x.Balance = 2000)
                .With(x => x.Status = AccountStatus.Live)
                .Build();

            _dataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => account);
            _dataStoreHelper.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_dataStore.Object);

            MakePaymentRequest makePaymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.Chaps)
                .With(x => x.Amount = 1000)
                .Build();

            _calculatorService.Setup(x => x.DeductAmountFromAccount(account, It.IsAny<decimal>()))
                .Callback(() => account.Balance -= makePaymentRequest.Amount);

            var paymentService = new PaymentService(new AccountService(_dataStoreHelper.Object, _configHelper.Object),
              _paymentsValidatorHelper, _calculatorService.Object);

            // Act
            var result = paymentService.MakePayment(makePaymentRequest);

            // Assert
            _configHelper.Verify(x => x.DataStoreType, Times.Exactly(2));
            _dataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _dataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Once);
            Assert.True(result.Success);
            Assert.Equal(1000, 1000);
        }

        #endregion
    }
}
