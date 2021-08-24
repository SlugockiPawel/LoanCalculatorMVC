﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanCalculatorMVC.Models
{
    public class Loan
    {
        public decimal LoanAmount { get; set; }
        public decimal Rate { get; set; }
        public int Term { get; set; }
        public decimal Payment { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal TotalCost { get; set; }
        public List<LoanPayment> LoanPayments { get; set; } = new();
    }
}