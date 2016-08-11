using System;
using System.ComponentModel.DataAnnotations;

namespace OnboardingConsumer.Models
{
    [Serializable]
    public class OnboardingPlanDetails
    {
        //External Provider Info
        [Required]
        public string ExternalCustomerId { get; set; }

        [Required]
        public string ExternalPlanId { get; set; }

        [Required]
        public int PlanType { get; set; }
    }
}