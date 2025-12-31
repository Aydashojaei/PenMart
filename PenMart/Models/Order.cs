using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PenMart.Models;

namespace PenMart.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public bool IsFinaly { get; set; }
        [Required]
        public DateTime CreatDate { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } //Navigation Property
        public List<OrderDetail> orderDetails { get; set; }  //Navigation Property
    }
}
