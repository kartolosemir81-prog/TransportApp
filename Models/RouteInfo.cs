namespace TransportApp.Models
{
    public class RouteInfo
    {
        public int Id { get; set; }
        public string RouteName { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public string PlateNumber { get; set; } = string.Empty;
        public string DriverName { get; set; } = string.Empty;
        public string DriverPhone { get; set; } = string.Empty;
        public string StartPoint { get; set; } = string.Empty;
        public string EndPoint { get; set; } = string.Empty;
        public string DepartureTime { get; set; } = string.Empty;
        public string ArrivalTime { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public string Stops { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
