﻿using Core.Domain.Base;
using Core.Domain.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Finance
{
    public partial class CostCenter : BaseEntity
    {
        public CostCenter()
        {
            Expense = new HashSet<Expense>();
            Revenue = new HashSet<Revenue>();
            this.Active = true;
        }

        public int CostCenterId { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Active { get; set; }

        
        public ICollection<Expense> Expense { get; set; }
        public ICollection<Revenue> Revenue { get; set; }
    }
}