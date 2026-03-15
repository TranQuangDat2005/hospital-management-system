namespace FinancialBillingService.Model
{
    public class ServiceOrder
    {
        public int OrderID { get; set; }
        public int VisitID { get; set; }
        public int ServiceID { get; set; }
        public int DoctorID { get; set; }
        public decimal UnitPriceAtOrder { get; set; }
        public DateTime OrderTime { get; set; }
        public string Status { get; set; }

        public ServiceOrder(int orderID, int visitID, int serviceID, int doctorID, decimal unitPriceAtOrder, DateTime orderTime, string status)
        {
            OrderID = orderID;
            VisitID = visitID;
            ServiceID = serviceID;
            DoctorID = doctorID;
            UnitPriceAtOrder = unitPriceAtOrder;
            OrderTime = orderTime;
            Status = status;
        }

        public ServiceOrder()
        {
        }
    }
}
