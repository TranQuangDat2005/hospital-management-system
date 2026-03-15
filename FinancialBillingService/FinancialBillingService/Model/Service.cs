namespace FinancialBillingService.Model
{
    public class Service
    {
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }

        public Service(int serviceID, string serviceName, string category, decimal price, string status)
        {
            ServiceID = serviceID;
            ServiceName = serviceName;
            Category = category;
            Price = price;
            Status = status;
        }

        public Service()
        {
        }
    }
}
