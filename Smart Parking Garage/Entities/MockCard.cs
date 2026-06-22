namespace Smart_Parking_Garage.Entities;

    public class MockCard
    {
        public int MockCardId { get; set; }

        public string ApplicationUserId { get; set; } = string.Empty;

        public string CardHolderName { get; set; } = string.Empty;

        public string CardNumber { get; set; } = string.Empty;

        public string ExpiryDate { get; set; } = string.Empty;

        public string CVV { get; set; } = string.Empty;

        public decimal Balance { get; set; }

        public bool IsBlocked { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }

