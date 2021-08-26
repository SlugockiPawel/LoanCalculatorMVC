using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoanCalculatorMVC.Models;

namespace LoanCalculatorMVC.Helpers
{
    public class LoanHelper
    {
        public Loan GetPayments(Loan loan)
        {
            // calculate monthly payment
            loan.Payment = CalculateMonthlyPayment(loan.Amount, loan.Rate, loan.Term);

            // default values:
            var remainingBalance = loan.Amount;
            var totalInterest = 0.0m;
            var monthlyInterest = 0.0m;
            var monthlyPrincipal = 0.0m;
            var monthlyRate = CalculateMonthlyRate(loan.Rate);

            //  loop through 1 to the term value
            for (int month = 1; month <= loan.Term; month++)
            {
                // calculate monthly payment schedule (basically a LoanPayment object)
                monthlyInterest = CalculateMonthlyInterest(remainingBalance, monthlyRate);
                totalInterest += monthlyInterest;
                monthlyPrincipal = loan.Payment - monthlyInterest;
                remainingBalance -= monthlyPrincipal;

                // push filled LoanPayment to Loan.Payments
                LoanPayment loanPayment = new()
                {
                    Payment = loan.Payment,
                    Month = month,
                    MonthlyInterest = monthlyInterest,
                    MonthlyPrincipal = monthlyPrincipal,
                    RemainingBalance = remainingBalance,
                    TotalInterest = totalInterest,
                };

                loan.Payments.Add(loanPayment);
            }

            // calculate totals
            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.Amount + totalInterest;

            // return loan with payments filled to the view
            return loan;
        }

        private decimal CalculateMonthlyPayment(decimal amount, decimal rate, int term)
        {
            decimal payment = 0.0m;
            decimal monthlyRate = CalculateMonthlyRate(rate);

            // Math.Pow works for doubles, not decimals therefore conversion is needed
            double monthlyRateD = Convert.ToDouble(monthlyRate);
            double amountD = Convert.ToDouble(amount);

            double paymentD = (amountD * monthlyRateD) / (1 - Math.Pow(1 + monthlyRateD, -term));

            return Convert.ToDecimal(paymentD);
        }

        private decimal CalculateMonthlyRate(decimal rate)
        {
            return rate / 1200; 
        }

        private decimal CalculateMonthlyInterest(decimal balance, decimal monthlyRate)
        {
            return balance * monthlyRate;
        }
    }
}
