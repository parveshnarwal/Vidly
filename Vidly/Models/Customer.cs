using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage= "Please enter full name of customer.")]
        [StringLength(255)]
        public string Name { get; set; }

        public bool IsSubscribedToNewsletter { get; set; }

        public int MembershipTypeId { get; set; }

        [Display(Name = "Membership Type")]
        public MembershipType MembershipType { get; set; }

        [Display(Name="Birth Date")]
        [Min18YearsIfAMember]
        public DateTime? DateOfBirth { get; set; }
    }
}