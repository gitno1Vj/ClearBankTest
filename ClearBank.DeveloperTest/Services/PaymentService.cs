using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Interfaces;
using ClearBank.DeveloperTest.Helpers;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountService _accountService;
        private readonly IPaymentsValidatorHelper _paymentsvalidatorHelper;
        private readonly ICalculatorService _calculatorService;

        public PaymentService(IAccountService accountService, IPaymentsValidatorHelper paymentsValidatorHelper, ICalculatorService calculatorService)
        {
            _accountService = accountService;
            _paymentsvalidatorHelper = paymentsValidatorHelper;
            _calculatorService = calculatorService;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            /*
                 Following Seperations/Implementations are required
                 Finding the Data Store Type
                 Query Account Details based on Data Store Type
                 Validate Payment Schemes
                 Calculate Account Balance
                 Update Account 
             */

            //Get Account
            var account = _accountService.GetAccount(request.DebtorAccountNumber);

            //Validate
            var result = _paymentsvalidatorHelper.Validate(request.PaymentScheme, account, request.Amount);

            if (!result.Success) return result;

            //Calculate
            _calculatorService.DeductAmountFromAccount(account, request.Amount);

            //Update Account
            _accountService.UpdateAccount(account);

            return result;
        }
    }
}
